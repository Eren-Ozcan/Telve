using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Telve.Data;
using Telve.Gameplay;
using Telve.Meta;
using UnityEditor;
using UnityEngine;

namespace Telve.EditorTools
{
    /// <summary>
    /// ROADMAP.md Faz 3 "Denge turu: 20+ tam koşu verisiyle sembol/tılsım/
    /// ekonomi ayarı". Gerçek oyuncu playtesti yerine geçmez, ama mevcut
    /// ekonomi/kombo sayılarının kabaca makul olup olmadığını (kazanma
    /// oranı, ortalama koşu uzunluğu) ve Faz 0'ın en kritik varsayımını
    /// ("dizilim kararı gerçekten önemli mi") hızlıca sinyaller.
    /// Pazar alışverişi bu v1'de simüle edilmiyor — dolayısıyla sonuçlar
    /// "pazarsız en kötü durum" tabanlı, gerçek oyuncu performansının
    /// alt sınırı olarak okunmalı.
    /// </summary>
    public static class BalanceSimulator
    {
        public static void RunSimulation(string resultPath, int runCount)
        {
            var allSymbols = LoadAll<SymbolData>("Assets/Resources/Data/Symbols");
            var allCombos = LoadAll<ComboData>("Assets/Resources/Data/Combos");
            var startingDeck = allSymbols.Where(s => s.rarity == SymbolRarity.Common).ToList();

            var allCharms = LoadAll<CharmData>("Assets/Resources/Data/Charms");

            var naive = SimulateStrategy(runCount, seedOffset: 0, startingDeck, allCombos, NaiveOrder, useMarket: false, allSymbols, allCharms);
            var optimal = SimulateStrategy(runCount, seedOffset: 1_000_000, startingDeck, allCombos, OptimalOrder, useMarket: false, allSymbols, allCharms);
            var optimalWithMarket = SimulateStrategy(runCount, seedOffset: 2_000_000, startingDeck, allCombos, OptimalOrder, useMarket: true, allSymbols, allCharms);

            var sb = new StringBuilder();
            sb.AppendLine($"BalanceSimulator: {runCount} koşu / strateji.");
            sb.AppendLine();
            AppendStrategyReport(sb, "Naif, pazarsız (alt sınır)", naive);
            sb.AppendLine();
            AppendStrategyReport(sb, "Optimal dizilim, pazarsız", optimal);
            sb.AppendLine();
            AppendStrategyReport(sb, "Optimal dizilim + açgözlü pazar alışverişi (gerçekçi üst sınır)", optimalWithMarket);
            sb.AppendLine();

            float upliftPct = naive.AvgScorePerCup > 0
                ? (optimal.AvgScorePerCup - naive.AvgScorePerCup) / naive.AvgScorePerCup * 100f
                : 0f;
            sb.AppendLine($"Dizilim kararının değeri: optimal dizilim naif diziliminden ortalama %{upliftPct:0.#} daha yüksek skor üretiyor.");
            sb.AppendLine(upliftPct >= 15f
                ? "-> Faz 0 varsayımı destekleniyor: dizilim kararı skoru anlamlı şekilde değiştiriyor."
                : "-> Uyarı: dizilim kararının etkisi düşük görünüyor, kombo matrisi/pozisyon bonusları güçlendirilmeli (bkz. ROADMAP.md Faz 0 riski).");

            File.WriteAllText(resultPath, sb.ToString());
            Debug.Log("BALANCE_SIMULATION_DONE");
        }

        static List<T> LoadAll<T>(string folder) where T : UnityEngine.Object
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { folder });
            return guids.Select(g => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(g))).ToList();
        }

        class StrategyStats
        {
            public int Wins;
            public int Losses;
            public List<int> CustomersReached = new();
            public List<int> FinalGold = new();
            public List<int> WisdomEarned = new();
            public float AvgScorePerCup;
            public List<float> MuhtarScores = new();
            public int MuhtarThreshold;
        }

        static StrategyStats SimulateStrategy(
            int runCount, int seedOffset, List<SymbolData> startingDeck, List<ComboData> allCombos,
            Func<List<SymbolData>, List<ComboData>, List<SymbolData>> chooseOrder,
            bool useMarket, List<SymbolData> allSymbols, List<CharmData> allCharms)
        {
            var stats = new StrategyStats();
            var scoreSamples = new List<float>();

            for (int run = 0; run < runCount; run++)
            {
                var rng = new System.Random(seedOffset + run * 7919 + 1);
                var day = new DaySession(startingGold: 20, rng);
                var ownedSymbols = new List<SymbolData>(startingDeck);
                var activeCharms = new List<CharmData>();

                while (!day.DayLost && !day.DayComplete)
                {
                    var cup = CupDraw.Draw(ownedSymbols, rng, activeCharms);
                    var order = chooseOrder(cup, allCombos);
                    var profile = day.CurrentProfile();
                    var result = CustomerEncounter.Resolve(profile, order, allCombos, activeCharms);
                    scoreSamples.Add(result.Score.FinalScore);
                    bool wasMuhtar = profile.IsMuhtar;
                    if (wasMuhtar)
                    {
                        stats.MuhtarScores.Add(result.Score.FinalScore);
                        stats.MuhtarThreshold = profile.Threshold;
                    }
                    day.SubmitEncounter(result);

                    if (useMarket && !wasMuhtar && !day.DayLost && !day.DayComplete)
                    {
                        GreedyMarketPurchase(day, ownedSymbols, activeCharms, allSymbols, allCharms, rng);
                    }
                }

                if (day.DayComplete) stats.Wins++; else stats.Losses++;
                stats.CustomersReached.Add(day.History.Count);
                stats.FinalGold.Add(day.Gold);
                stats.WisdomEarned.Add(WisdomReward.CalculateRunReward(day.Gold, day.DayComplete, 0));
            }

            stats.AvgScorePerCup = scoreSamples.Count > 0 ? scoreSamples.Average() : 0f;
            return stats;
        }

        // Basit açgözlü pazar AI'ı: karşılanabilir en ucuz teklifi alır
        // (erken oyunda deste büyütmeyi tılsım biriktirmeye tercih eder).
        static void GreedyMarketPurchase(
            DaySession day, List<SymbolData> ownedSymbols, List<CharmData> activeCharms,
            List<SymbolData> allSymbols, List<CharmData> allCharms, System.Random rng)
        {
            var offers = MarketSystem.GenerateOffers(allSymbols, ownedSymbols, allCharms, activeCharms, rng, activeCharms);
            var affordable = offers.Where(o => o.Price <= day.Gold).OrderBy(o => o.Price).FirstOrDefault();
            if (!affordable.IsSymbol && affordable.Charm == null) return; // hiçbir teklif karşılanamıyor

            if (day.TrySpendGold(affordable.Price))
            {
                if (affordable.IsSymbol) ownedSymbols.Add(affordable.Symbol);
                else activeCharms.Add(affordable.Charm);
            }
        }

        static void AppendStrategyReport(StringBuilder sb, string label, StrategyStats stats)
        {
            int total = stats.Wins + stats.Losses;
            sb.AppendLine($"[{label}]");
            sb.AppendLine($"  Muhtar'ı geçme oranı: {stats.Wins}/{total} (%{(total > 0 ? stats.Wins * 100f / total : 0):0.#})");
            sb.AppendLine($"  Ortalama ulaşılan müşteri sayısı: {stats.CustomersReached.Average():0.#} / 9 (8 sıradan + muhtar)");
            sb.AppendLine($"  Ortalama bitiş altını: {stats.FinalGold.Average():0.#}");
            sb.AppendLine($"  Ortalama kazanılan bilgelik puanı: {stats.WisdomEarned.Average():0.#}");
            sb.AppendLine($"  Ortalama skor / fincan: {stats.AvgScorePerCup:0.#}");
            if (stats.MuhtarScores.Count > 0)
            {
                sb.AppendLine($"  Muhtar eşiği: {stats.MuhtarThreshold}, ortalama muhtar skoru: {stats.MuhtarScores.Average():0.#} (fark: {stats.MuhtarScores.Average() - stats.MuhtarThreshold:0.#})");
            }
        }

        static List<SymbolData> NaiveOrder(List<SymbolData> cup, List<ComboData> combos) =>
            cup.OrderByDescending(s => s.baseValue).ToList();

        static List<SymbolData> OptimalOrder(List<SymbolData> cup, List<ComboData> combos)
        {
            List<SymbolData> best = cup;
            float bestScore = float.MinValue;

            foreach (var permutation in Permutations(cup))
            {
                float score = ScoreCalculator.Calculate(permutation, combos).FinalScore;
                if (score > bestScore)
                {
                    bestScore = score;
                    best = new List<SymbolData>(permutation);
                }
            }

            return best;
        }

        static IEnumerable<List<SymbolData>> Permutations(List<SymbolData> items)
        {
            var array = items.ToArray();
            int n = array.Length;
            var indices = new int[n];
            for (int i = 0; i < n; i++) indices[i] = i;

            yield return array.ToList();

            var c = new int[n];
            int index = 0;
            while (index < n)
            {
                if (c[index] < index)
                {
                    if (index % 2 == 0) Swap(array, 0, index);
                    else Swap(array, c[index], index);

                    yield return array.ToList();

                    c[index]++;
                    index = 0;
                }
                else
                {
                    c[index] = 0;
                    index++;
                }
            }
        }

        static void Swap(SymbolData[] array, int i, int j) => (array[i], array[j]) = (array[j], array[i]);
    }
}
