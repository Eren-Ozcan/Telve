using System;
using System.Collections.Generic;
using System.Linq;
using Telve.Data;

namespace Telve.Gameplay
{
    /// <summary>
    /// Fincan çevirme: docs/design/00-core-loop.md adım 1 ("5-7 sembol
    /// rastgele belirir, seed'li RNG") + docs/design/05-charms.md'deki
    /// DrawRng grubu tılsım etkileşimleri (Kahve Telvesi Yoğun, Şans
    /// Tekerleği, Kader Anahtarı). System.Random parametre olarak alınır
    /// ki haftalık seed (ROADMAP.md Faz 6) ve test tekrarlanabilirliği
    /// aynı mekanizmaya dayansın — sınıf kendi RNG'sini oluşturmaz.
    /// </summary>
    public static class CupDraw
    {
        const int BaseMinSymbols = 5;
        const int BaseMaxSymbols = 7; // inclusive

        public static List<SymbolData> Draw(
            IReadOnlyList<SymbolData> symbolPool,
            Random rng,
            IReadOnlyList<CharmData> activeCharms = null)
        {
            if (symbolPool == null || symbolPool.Count == 0)
                throw new ArgumentException("symbolPool boş olamaz.", nameof(symbolPool));
            if (rng == null)
                throw new ArgumentNullException(nameof(rng));

            activeCharms ??= Array.Empty<CharmData>();

            int min = BaseMinSymbols;
            int max = BaseMaxSymbols;
            if (HasCharm(activeCharms, "kahve_telvesi_yogun"))
            {
                // "Fincanda 1 sembol daha fazla çıkar (6-8 arası)".
                min += 1;
                max += 1;
            }

            int count = rng.Next(min, max + 1); // Random.Next üst sınırı exclusive.

            bool rareBoost = HasCharm(activeCharms, "sans_tekerlegi");
            var weights = symbolPool.Select(s => EffectiveWeight(s, rareBoost)).ToArray();

            var result = new List<SymbolData>(count);
            for (int i = 0; i < count; i++)
            {
                result.Add(WeightedPick(symbolPool, weights, rng));
            }

            if (HasCharm(activeCharms, "kader_anahtari") && !result.Any(s => s.symbolId == "tac"))
            {
                var tac = symbolPool.FirstOrDefault(s => s.symbolId == "tac");
                if (tac != null)
                {
                    // Toplam sembol sayısını değiştirmeden rastgele bir
                    // pozisyonun yerini alır.
                    result[rng.Next(result.Count)] = tac;
                }
            }

            return result;
        }

        static bool HasCharm(IReadOnlyList<CharmData> charms, string charmId) =>
            charms.Any(c => c.charmId == charmId);

        // Şans Tekerleği: "Rare ve Epic sembol çekiliş ağırlığı %50 artar".
        static int EffectiveWeight(SymbolData symbol, bool rareBoost)
        {
            if (!rareBoost) return symbol.drawWeight;

            bool isRareOrEpic = symbol.rarity == SymbolRarity.Rare || symbol.rarity == SymbolRarity.Epic;
            return isRareOrEpic ? (int)MathF.Round(symbol.drawWeight * 1.5f) : symbol.drawWeight;
        }

        static SymbolData WeightedPick(IReadOnlyList<SymbolData> pool, int[] weights, Random rng)
        {
            int total = weights.Sum();
            int roll = rng.Next(total); // 0..total-1

            int cumulative = 0;
            for (int i = 0; i < pool.Count; i++)
            {
                cumulative += weights[i];
                if (roll < cumulative) return pool[i];
            }

            return pool[^1]; // Yuvarlama hatasına karşı güvenlik ağı.
        }
    }
}
