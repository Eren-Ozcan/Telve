# Scoring Formula

The equivalent of Balatro's "Chips × Mult":

```
Customer Satisfaction =
    ( Σ Symbol Values  +  Σ Flat Combo Bonuses  +  Σ Flat Charm Bonuses )
    × ( Π Combo Multipliers )
    × ( Π Charm Multipliers )
```

## Step-by-Step Calculation Order

1. **Base Score**: The sum of the `baseValue` of every symbol in the arrangement.
2. **Combo Detection**: All adjacent pairs/triples in the arrangement are
   scanned against the matrix in 02-combos.md. The overlap rule is applied
   (the combo with the highest multiplier wins, lower combos are suppressed —
   see 02-combos.md).
3. **Flat Bonuses Are Summed**: All `+N` effects from the triggered combos are
   added to the base score.
4. **Combo Multipliers Are Applied In Order**: All `×N` effects from the
   triggered combos are multiplied in their trigger order within the
   arrangement (the order becomes important with charm effects — see the
   "First Combo Multiplier" charm, 05-charms.md).
5. **Charm Effects Are Applied**: Active charms apply their effects in turn
   (this varies by whether they target the symbol value, the combo multiplier
   or the final result — each charm's effect target is defined in
   05-charms.md).
6. **Result**: The Customer Satisfaction score comes out → it becomes the input
   to the payment formula in 04-economy.md.

## Example Calculation 1 — Without Charms

Arrangement: **Road(3) → Bird(3) → Letter(2)**

- Base score: 3 + 3 + 2 = **8**
- Combo scan: Road-Bird, Bird-Letter, Road-Bird-Letter (triple) all match.
  Overlap rule: since the triple combo ("Certain News", ×2.5) has the highest
  multiplier, the pair combos are suppressed.
- Flat bonus: none (the +3 "News on the Road" from the suppressed pair combos
  does not count either, because it belongs to the same triple symbol group —
  the overlap rule applies to flat bonuses as well).
- Multiplier application: 8 × 2.5 = **20**
- **Customer Satisfaction = 20**

## Example Calculation 2 — With a Charm

The same arrangement + the **"First Combo Multiplier" charm** (doubles the
multiplier of the first triggered combo):

- Base score: 8
- The only triggered combo, "Certain News" (×2.5), is already the first (and
  only) combo in the arrangement → the charm doubles it: 2.5 × 2 = 5.0
  effective multiplier.
- 8 × 5.0 = **40**
- **Customer Satisfaction = 40**

## Example Calculation 3 — The Risk of a Negative Combo

Arrangement: **Snake(5) → Mountain(8) → Cloud(2)**

- Base score: 5 + 8 + 2 = **15**
- Combo: the triple "Black Day" triggers (×0.6) — the pair Mountain+Cloud
  (×0.8) is suppressed.
- 15 × 0.6 = **9**
- **Customer Satisfaction = 9** (arranging the same symbols in a different
  order — e.g. putting the Snake in the middle instead of at the very front to
  break up the triple — would have avoided this penalty. This is the concrete
  proof of why the arrangement decision matters.)

## Design Principles

- **Flat bonuses first, multipliers after**: small flat values feel meaningful
  in the early game, while stacks of multipliers create the explosion in the
  late game (the same ordering logic as Balatro's "chips then mult"
  philosophy).
- **Multipliers are multiplicative (order independent), flat charms are
  additive**: the calculation stays simple and predictable; the player should
  be able to approximate it in their head (Balatro's readability lesson).
- All coefficients (`1.5`, `×2`, `+3`, etc.) will be kept as ScriptableObject
  fields in Phase 1, so they can be balanced without code changes (ROADMAP.md
  design rule).
- This formula is a **draft** — the final balance will be tuned in Phase 3 with
  data from 20+ full runs (ROADMAP.md Phase 3).
