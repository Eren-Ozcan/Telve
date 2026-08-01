# Symbol Set v1

24 symbols, 4 rarity classes. Rarity determines both the draw weight and the
base value range (a rare symbol = low draw chance but a high base score).

Draw weight (the `drawWeight` ScriptableObject field in Phase 1):
Common 100, Uncommon 45, Rare 15, Epic 4.

The Turkish source name of each symbol is given in parentheses — the assets in
the project are authored in Turkish and mapped to English via
`ContentLocalization`.

## Common — base value 2-4 (12 symbols)

| Symbol | Value | Fortune Meaning |
|---|---|---|
| Road (Yol) | 3 | Journey, change, a new direction |
| Bird (Kuş) | 3 | News, glad tidings |
| Heart (Kalp) | 3 | Love, an emotional bond |
| Eye (Göz) | 2 | The evil eye, protection |
| Tree (Ağaç) | 3 | Family, roots, growth |
| Fish (Balık) | 3 | Abundance, money |
| Cloud (Bulut) | 2 | Uncertainty, worry |
| Flower (Çiçek) | 3 | Good news, a proposal |
| Moon (Ay) | 3 | Change, a hidden feeling |
| Ladder (Merdiven) | 4 | Rising, promotion |
| Door (Kapı) | 3 | A new beginning, opportunity |
| Letter (Mektup) | 2 | Awaited news |

## Uncommon — base value 4-5 (7 symbols)

| Symbol | Value | Fortune Meaning |
|---|---|---|
| Snake (Yılan) | 5 | Enemy, betrayal, gossip |
| Ship (Gemi) | 5 | A distant journey |
| Key (Anahtar) | 4 | A new opportunity, a solution |
| Cat (Kedi) | 4 | Jealousy, deceit |
| Bridge (Köprü) | 5 | A transition period, a moment of decision |
| Bell (Çan) | 4 | News of a wedding/ceremony |
| Arrow (Ok) | 5 | Direction, a firm decision |

## Rare — base value 7-8 (4 symbols)

| Symbol | Value | Fortune Meaning |
|---|---|---|
| Mountain (Dağ) | 8 | A great obstacle, hardship |
| Star (Yıldız) | 7 | A wish, hope |
| Sun (Güneş) | 8 | Success, happiness |
| Well (Kuyu) | 7 | A deep secret |

## Epic — base value 12 (1 symbol)

| Symbol | Value | Fortune Meaning |
|---|---|---|
| Crown (Taç) | 12 | Great power, a turning point of fate |

## Notes

- 24 symbols in total: within the 20-25 MVP target range.
- The values are deliberately kept low — the real score explosion should come
  from combo multipliers (see 03-scoring.md), otherwise the single strategy of
  "put the highest-value symbols in the order" dominates and the arrangement
  decision becomes meaningless.
- In Phase 1 each symbol will be a `SymbolData` ScriptableObject:
  `id, displayName, baseValue, rarity, drawWeight, spriteRef,
  falMeaningText`.
- In Phase 2, a separate final visual + an "emerging" effect per symbol is
  planned (ROADMAP.md Phase 2).
