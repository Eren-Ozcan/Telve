# Core Loop

> Phase 0 exit item: "One page, unambiguously clear."

## One Customer Turn (a Single Hand)

```
1. FLIP THE CUP
   The customer drinks their coffee, turns the cup upside down onto the
   saucer, waits, then flips it back.
   → 5-7 symbols appear at random in the grounds (from the deck's symbol
     pool, a rarity-weighted draw, seeded RNG).

2. ARRANGE THE READING ORDER
   The player drags the revealed symbols into a reading order
   (left to right, read from the rim of the cup down to its bottom —
   fortune-telling tradition). This is the equivalent of Balatro's
   "select the cards in your hand" decision: which symbol you place
   where determines which combo you trigger.

3. COMBO DETECTION
   Adjacent symbol pairs/triples in the arrangement are scanned against
   the predefined combo matrix (see 02-combos.md). Each combo triggers a
   name card and an effect (flat bonus or multiplier).

4. SCORE CALCULATION
   Base score (sum of symbol values) → combo effects → active charm
   effects are applied in that order → the Customer Satisfaction score
   comes out (see 03-scoring.md).

5. PAYMENT / REACTION
   The satisfaction score is compared against that customer's threshold.
   If the threshold is beaten, full+bonus payment and a positive
   reaction; if not, low payment and a negative reaction (see
   04-economy.md). Newly discovered combos are recorded in the
   fortune-teller's journal with a golden frame.

6. NEXT CUSTOMER
   The market can be visited between customers: with the gold earned,
   new symbols or charms are bought to strengthen the deck (see
   04-economy.md).
```

## Day Loop

```
Customer 1 → [market] → Customer 2 → [market] → ... → Customer 8
→ [market] → Headman (end-of-day boss, a special high-threshold customer)
→ End-of-Day Summary → Next Day
```

- A day has 8-10 ordinary customers + 1 Headman.
- Difficulty rises within the day: customer thresholds increase with order.
- If the Headman is not beaten, the day is lost (the roguelike death condition — the run ends).

## Why This Loop Works With the Balatro Analogy

| Balatro | Telve |
|---|---|
| Draw a hand of cards | Flip the cup → symbols appear |
| Select a poker hand (which cards are played) | Arrange the reading order (which symbol goes where) |
| Poker hand type (flush, straight...) | Combo (Road+Bird = "News Is Coming") |
| Chips × Mult | Symbol Value × Combo Multiplier × Charm |
| Blind threshold | Customer Satisfaction threshold |
| Joker cards | Charms |
| Shop | Market |
| Boss blind | Headman |

The difference: in Balatro the *selection* of the hand matters, while in Telve
the *order* of the hand (the arrangement) matters — the adjacency-based combo
system puts the ordering decision at the center of the game. This is the most
critical assumption to be tested in the Phase 0 paper prototype (see the
Phase 0 exit criterion in ROADMAP.md).

## Open Questions (To Be Answered in the Paper Prototype)

- Should there be time pressure while dragging, or free thinking time?
  (Suggestion: free in Phase 1, a timer can be added later as a
  difficulty mode.)
- Is the symbol count fixed (always 6) or variable (5-7)? (Suggestion:
  variable — it opens up an interaction space with charms, see the
  "Thick Coffee Grounds" charm.)
- Can there be symbols that go unused (left in the grounds but not placed
  into the order)? (Suggestion: no, in the MVP every drawn symbol must be
  arranged — for simplicity.)
