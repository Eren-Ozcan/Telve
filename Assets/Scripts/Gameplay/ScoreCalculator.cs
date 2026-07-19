using System;
using System.Collections.Generic;
using System.Linq;
using Telve.Data;

namespace Telve.Gameplay
{
    public readonly struct ScoreResult
    {
        public readonly float BaseScore;
        public readonly float FinalScore;
        public readonly IReadOnlyList<ComboMatch> TriggeredCombos;

        public ScoreResult(float baseScore, float finalScore, IReadOnlyList<ComboMatch> triggeredCombos)
        {
            BaseScore = baseScore;
            FinalScore = finalScore;
            TriggeredCombos = triggeredCombos;
        }
    }

    /// <summary>
    /// Implements the docs/design/03-scoring.md pipeline: taban puan →
    /// kombo tespiti → sabit bonuslar → kombo çarpanları → tılsım
    /// etkileri. Charm dispatch below covers the 10 concrete tılsımlar
    /// from docs/design/05-charms.md by charmId (hardcoded on purpose —
    /// there is no generic "charm effect" abstraction to build against
    /// until a second content pass adds more). DrawRng/Economy charms
    /// don't touch scoring and are skipped here; they belong to the
    /// fincan-draw and pazar systems respectively.
    /// </summary>
    public static class ScoreCalculator
    {
        // Muhtar "kötü fala inanmaz" (ROADMAP.md Faz 2): negatif kombo
        // cezası bu oranda şiddetlenir (1'e olan uzaklık ×1.4 büyür).
        const float MuhtarNegativePenaltyBoost = 1.4f;

        public static ScoreResult Calculate(
            IReadOnlyList<SymbolData> readingOrder,
            IReadOnlyList<ComboData> comboLibrary,
            IReadOnlyList<CharmData> activeCharms = null,
            bool punishesNegativeCombos = false)
        {
            activeCharms ??= Array.Empty<CharmData>();

            // Adım 1: taban puan.
            float baseScore = 0f;
            foreach (var symbol in readingOrder)
            {
                baseScore += symbol.baseValue + SymbolValueBonus(symbol, activeCharms);
            }

            // Adım 2: kombo tespiti.
            var symbolIds = readingOrder.Select(s => s.symbolId).ToList();
            var triggered = ComboDetector.Detect(symbolIds, comboLibrary);

            bool hasNazarSuppression = HasCharm(activeCharms, "sansli_nazar");
            float negativePenaltyReduction = HasCharm(activeCharms, "kara_kedi_tilismi") ? 0.5f : 0f;

            // Adım 3: sabit bonuslar (çarpanlardan önce).
            float flatBonus = 0f;
            foreach (var match in triggered)
            {
                if (match.Combo.effectType == ComboEffectType.Flat)
                {
                    flatBonus += match.Combo.effectValue;
                }

                flatBonus += match.Combo.secondaryFlatBonus;
            }

            flatBonus += RepeatedAdjacentSymbolBonus(readingOrder, activeCharms);

            float score = baseScore + flatBonus;

            // Adım 4: kombo çarpanları, dizilim sırasına (soldan sağa) göre.
            bool firstMultiplierBoosted = false;
            bool hasFirstComboCharm = HasCharm(activeCharms, "ilk_kombo_carpani");

            foreach (var match in triggered)
            {
                if (match.Combo.effectType != ComboEffectType.Multiplier) continue;

                float multiplier = AdjustedMultiplier(match.Combo, hasNazarSuppression, negativePenaltyReduction, punishesNegativeCombos);

                if (!firstMultiplierBoosted && hasFirstComboCharm)
                {
                    multiplier *= 2f;
                    firstMultiplierBoosted = true;
                }

                score *= multiplier;
            }

            return new ScoreResult(baseScore, score, triggered);
        }

        static bool HasCharm(IReadOnlyList<CharmData> charms, string charmId) =>
            charms.Any(c => c.charmId == charmId);

        // Kuş Tüyü: Kuş sembollerinin değeri +2.
        static float SymbolValueBonus(SymbolData symbol, IReadOnlyList<CharmData> charms)
        {
            if (symbol.symbolId == "kus" && HasCharm(charms, "kus_tuyu")) return 2f;
            return 0f;
        }

        // Sadık Dost: aynı sembol iki kez yan yana gelirse +5 (her tekrar için).
        static float RepeatedAdjacentSymbolBonus(IReadOnlyList<SymbolData> readingOrder, IReadOnlyList<CharmData> charms)
        {
            if (!HasCharm(charms, "sadik_dost")) return 0f;

            float bonus = 0f;
            for (int i = 0; i < readingOrder.Count - 1; i++)
            {
                if (readingOrder[i].symbolId == readingOrder[i + 1].symbolId) bonus += 5f;
            }

            return bonus;
        }

        // Şanslı Nazar: Göz içeren negatif kombolar tamamen etkisiz olur.
        // Kara Kedi Tılsımı: kalan negatif kombo cezası %50 azalır
        // (örn. ×0.6 → ×0.8 — bkz. docs/design/05-charms.md).
        // Not: mevcut veri setinde negatif kombolar her zaman Multiplier
        // tipinde; Flat tipte negatif kombo yok, bu yüzden bu ayarlama
        // sadece Multiplier dalında uygulanıyor.
        static float AdjustedMultiplier(ComboData combo, bool hasNazarSuppression, float negativePenaltyReduction, bool punishesNegativeCombos)
        {
            if (!combo.isNegative) return combo.effectValue;

            if (hasNazarSuppression && Array.IndexOf(combo.requiredSymbolIds, "goz") >= 0)
            {
                return 1f;
            }

            float value = combo.effectValue;

            if (punishesNegativeCombos)
            {
                value = 1f - (1f - value) * MuhtarNegativePenaltyBoost;
            }

            if (negativePenaltyReduction > 0f)
            {
                value = 1f - (1f - value) * (1f - negativePenaltyReduction);
            }

            return value;
        }
    }
}
