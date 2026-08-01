# Combo Matrix v1

37 combos (31 pairs + 6 triples). Adjacency = symbols standing side by side in
the reading order (positions i and i+1). Triple combos cover 3 consecutive
positions.

**Overlap rule:** If the same symbol group triggers both a pair and a triple
combo (e.g. when Road-Bird-Letter are consecutive it looks like both "News Is
Coming" and "Certain News" could trigger), only the **combo with the highest
multiplier** counts — the lower combos are suppressed. No double counting. For
details see 03-scoring.md.

**Effect types:** `×N` (multiplier, applied in sequence) or `+N` (flat, added
to the base score before multipliers).

## News Group

| Combo | Name | Effect |
|---|---|---|
| Road + Bird | News Is Coming | ×1.5 |
| Bird + Letter | The Awaited Letter | ×1.5 |
| Letter + Road | News on the Road | +3 |
| Bird + Bell | Wedding News | ×1.8 |

## Love Group

| Combo | Name | Effect |
|---|---|---|
| Heart + Star | Destined Love | ×2.0 |
| Heart + Moon | Secret Love | ×1.6 |
| Heart + Heart | Love Triangle | ×1.7 |
| Heart + Door | New Love | +4 |
| Heart + Snake | Betrayed Love | ×1.4 |

## Warning Group (some are deliberately negative — a lesson in risky arrangement)

| Combo | Name | Effect |
|---|---|---|
| Snake + Cat | Two-Faced Enemy | ×1.9 |
| Mountain + Cloud | A Hard Obstacle | ×0.8 |
| Snake + Eye | The Evil Eye Breaks | ×1.3 |
| Cat + Letter | Gossip | ×0.9 |

## Abundance / Success Group

| Combo | Name | Effect |
|---|---|---|
| Fish + Sun | Plentiful Earnings | ×2.2 |
| Fish + Key | Door to a New Opportunity | +5 |
| Sun + Crown | The Summit of Victory | ×3.0 |
| Ladder + Sun | Rising Star | ×1.8 |

## Journey Group

| Combo | Name | Effect |
|---|---|---|
| Road + Ship | A Distant Journey | ×1.6 |
| Ship + Fish | Bountiful Sea | +6 |
| Bridge + Door | Transition Period | ×1.5 |
| Road + Mountain | A Hard Journey | ×1.2, +2 |
| Key + Door | The Door of Fate | ×2.0 |

## Secret Group

| Combo | Name | Effect |
|---|---|---|
| Well + Moon | Deep Secret | ×1.7 |
| Well + Eye | A Secret Revealed | ×1.4 |
| Moon + Cloud | An Uncertain Future | ×0.85 |

## Family / Growth Group

| Combo | Name | Effect |
|---|---|---|
| Tree + Heart | Family Happiness | ×1.6 |
| Tree + Flower | A New Generation | +5 |
| Flower + Heart | Romantic Surprise | ×1.5 |

## Other Pair Combos

| Combo | Name | Effect |
|---|---|---|
| Eye + Star | A Protected Wish | ×1.4 |
| Door + Road | Setting Out on a New Road | +3 |
| Bell + Flower | A Happy Ceremony | ×1.5 |

## Triple Combos (high risk / high reward)

| Combo (3 consecutive) | Name | Effect |
|---|---|---|
| Road + Bird + Letter | Certain News | ×2.5 |
| Heart + Star + Crown | A Great Love Destiny | ×3.5 |
| Fish + Sun + Crown | Legendary Abundance | ×4.0 |
| Snake + Mountain + Cloud | Black Day | ×0.6 |
| Key + Door + Bridge | A New Path in Life | ×2.8 |
| Eye + Star + Moon | Sky Reading | ×2.2 |

Total: 31 pairs + 6 triples = **37 combos** (within the 30-40 MVP target range).

## Design Notes

- The negative combos (Mountain+Cloud, Cat+Letter, Moon+Cloud, Black Day) are
  deliberate: they pull the player out of the naive optimization of "put every
  symbol next to the strongest combo" and force the decision of *which symbols
  do I need to keep apart*. Whether this decision is actually felt must be
  tested in the paper prototype (ROADMAP.md Phase 0 exit criterion).
- "Black Day" (×0.6) is the harshest penalty in the MVP — it rarely triggers
  because three negative symbols have to be deliberately placed side by side;
  its real function is to teach the strategy of "spreading out" negative
  symbols while building the deck.
- In Phase 1 each combo will be a `ComboData` ScriptableObject:
  `id, requiredSymbolIds (ordered list), displayName, effectType
  (Multiplier/Flat), effectValue, isNegative`.
- Combo detection performance note: a position-based O(n) scan is enough
  (n ≤ 8 symbols), no need for early optimization.
