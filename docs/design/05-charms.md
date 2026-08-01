# Charm List v1

10 passive charms (the equivalent of Balatro's Joker cards). Each charm has an
automatic, passive effect; the player never triggers them manually.

| Charm | Rarity | Price | Effect |
|---|---|---|---|
| Bird Feather | Common | 12 | Bird symbols are worth +2 |
| Lucky Evil Eye | Common | 12 | Negative combos containing the Eye symbol have no effect |
| Loyal Friend | Common | 12 | If the same symbol appears twice side by side, +5 flat bonus |
| Thrifty Fortune | Common | 12 | Market prices drop by 15% |
| First Combo Multiplier | Uncommon | 22 | The multiplier of the first combo triggered in the arrangement becomes ×2 |
| Thick Coffee Grounds | Uncommon | 22 | 1 extra symbol appears in the cup (6-8 range) |
| Black Cat Charm | Uncommon | 22 | Negative combo penalties are reduced by 50% (e.g. ×0.6 → ×0.8) |
| Wheel of Fortune | Rare | 38 | The draw weight of Rare and Epic symbols increases by 50% |
| The Headman's Favorite | Rare | 38 | Payment is ×1.5 on the Headman customer |
| Key of Fate | Epic | 65 | The Crown symbol is guaranteed to appear in every hand |

## Effect Target Classification (for Step 5 of 03-scoring.md)

In step 5 of the scoring formula ("Charm Effects Are Applied"), it must be
clear which stage each charm intervenes in:

| Effect Target | Charms |
|---|---|
| Symbol value (enters the base score) | Bird Feather |
| Flat bonus (added to the base score) | Loyal Friend |
| Combo multiplier (at the multiplier stage) | First Combo Multiplier, Black Cat Charm |
| Negative combo suppression | Lucky Evil Eye |
| Draw / RNG (at the cup flip stage, before scoring) | Thick Coffee Grounds, Wheel of Fortune, Key of Fate |
| Economy (at the market/payment stage, after scoring) | Thrifty Fortune, The Headman's Favorite |

## Design Notes

- There is at least one charm from every rarity class; the prices match the
  market price table in 04-economy.md exactly.
- "Black Cat Charm" and "Lucky Evil Eye" offer a direct balance/counter-strategy
  against the deliberately negative combos in 02-combos.md — whether the player
  manages negative combos purely through arrangement discipline or compensates
  for them by investing in charms was designed to be a choice that is part of
  their deck-building identity.
- In Phase 1 each charm will be a `CharmData` ScriptableObject:
  `id, displayName, rarity, price, effectTarget (enum), effectValue`.
- The MVP target of ~10 charms has been met (ROADMAP.md Phase 1). Before the
  charm count is increased in Phase 2, it must be verified that the existing 10
  actually create the "what if I tried that deck with that charm" feeling in the
  paper/digital prototype (ROADMAP.md Phase 1 exit criterion).
