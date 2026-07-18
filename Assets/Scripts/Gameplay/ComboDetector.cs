using System.Collections.Generic;
using Telve.Data;

namespace Telve.Gameplay
{
    public readonly struct ComboMatch
    {
        public readonly ComboData Combo;
        public readonly int StartIndex;

        public ComboMatch(ComboData combo, int startIndex)
        {
            Combo = combo;
            StartIndex = startIndex;
        }
    }

    /// <summary>
    /// Scans a reading-order sequence of symbolIds for adjacent kombo
    /// matches (docs/design/02-combos.md). Implements the çakışma kuralı:
    /// when candidate combos' symbol spans overlap, only the longest
    /// (üçlü over ikili) survives — among equal-length overlaps, the one
    /// with the higher effectValue wins. Suppressed alternatives are
    /// simply absent from the result, not counted.
    /// </summary>
    public static class ComboDetector
    {
        public static List<ComboMatch> Detect(IReadOnlyList<string> readingOrder, IReadOnlyList<ComboData> comboLibrary)
        {
            var candidates = new List<ComboMatch>();

            for (int start = 0; start < readingOrder.Count; start++)
            {
                foreach (var combo in comboLibrary)
                {
                    int len = combo.requiredSymbolIds?.Length ?? 0;
                    if (len < 2 || start + len > readingOrder.Count) continue;

                    bool matches = true;
                    for (int i = 0; i < len; i++)
                    {
                        if (readingOrder[start + i] != combo.requiredSymbolIds[i])
                        {
                            matches = false;
                            break;
                        }
                    }

                    if (matches) candidates.Add(new ComboMatch(combo, start));
                }
            }

            candidates.Sort((a, b) =>
            {
                int lenA = a.Combo.requiredSymbolIds.Length;
                int lenB = b.Combo.requiredSymbolIds.Length;
                if (lenA != lenB) return lenB.CompareTo(lenA); // longer span wins ties
                return b.Combo.effectValue.CompareTo(a.Combo.effectValue);
            });

            var occupied = new HashSet<int>();
            var result = new List<ComboMatch>();

            foreach (var candidate in candidates)
            {
                int len = candidate.Combo.requiredSymbolIds.Length;
                bool overlaps = false;
                for (int i = candidate.StartIndex; i < candidate.StartIndex + len; i++)
                {
                    if (occupied.Contains(i))
                    {
                        overlaps = true;
                        break;
                    }
                }

                if (overlaps) continue;

                for (int i = candidate.StartIndex; i < candidate.StartIndex + len; i++)
                {
                    occupied.Add(i);
                }

                result.Add(candidate);
            }

            // Left-to-right order for deterministic multiplier application
            // (docs/design/03-scoring.md adım 4).
            result.Sort((a, b) => a.StartIndex.CompareTo(b.StartIndex));
            return result;
        }
    }
}
