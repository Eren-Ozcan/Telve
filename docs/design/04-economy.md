# Economy Draft

> This is a **draft**, not final numbers. It will be rebalanced in ROADMAP.md
> Phase 3 with data from 20+ full runs. The purpose here is for the loop to be
> **playable** end to end — since every number will live in a ScriptableObject,
> changing them will not require code changes.

## Starting State

- Starting gold: **20**
- Starting deck: a basic symbol subset to be defined in Phase 1
  (e.g. the 10 most common Commons out of the 24 symbols) + 0 charms.

## Customer Thresholds (Within-Day Difficulty Curve)

8 ordinary customers + 1 Headman (end-of-day boss).

```
Threshold(n) = 12 + n × 4        (n = customer order, 1-8)
Payment(n)   = 6 + n × 2 base    + (Satisfaction - Threshold) × 0.3  [if the threshold is beaten]
```

| Customer | Threshold | Base Payment | Note |
|---|---|---|---|
| 1 | 16 | 8 | Tutorial difficulty |
| 2 | 20 | 10 | |
| 3 | 24 | 12 | |
| 4 | 28 | 14 | |
| 5 | 32 | 16 | |
| 6 | 36 | 18 | |
| 7 | 40 | 20 | |
| 8 | 44 | 22 | |
| **Headman** | **66** (Threshold(8) × 1.5) | **35** | End-of-day boss |

- If the threshold **is beaten**: `Base Payment + (Satisfaction - Threshold) × 0.3`
  gold is earned, the customer reacts positively, and the journal/reputation
  score is updated.
- If the threshold **is not beaten**: **40%** of the base payment is given (the
  fortune still says something, but the customer is not satisfied), and the
  negative reaction animation plays.
- **If the Headman is not beaten**: the day is lost and the run ends (the
  roguelike death condition).

## Market Prices

The deck is strengthened by visiting the market between customers.

| Rarity | Symbol Price | Charm Price |
|---|---|---|
| Common | 8 | 12 |
| Uncommon | 15 | 22 |
| Rare | 28 | 38 |
| Epic | 50 | 65 |

- 3 options are offered on each market visit (ROADMAP.md Phase 1: "buy a
  symbol/charm from 3 options").
- The options are drawn at random from the rarity pool unlocked so far (the
  Epic pool may be locked in the first days — unlocking it via meta-progression
  will be evaluated in Phase 3).

## End-of-Day Target Balance

The total potential earnings of one day (base payments of 8 customers + the
Headman, excluding bonuses):

```
8 + 10 + 12 + 14 + 16 + 18 + 20 + 22 + 35 = 155 gold (base, end-of-day total)
```

- This corresponds to a budget where the player can make roughly **1-2 Common**
  or **1 Uncommon** level purchases within a day — the aim is a sense of
  concrete but slow deck growth every day.
- The target range is consistent with the principle in ROADMAP.md: neither too
  generous (the deck saturates immediately and decisions become meaningless)
  nor too stingy (the player can never upgrade).

## Open Questions (To Be Settled in the Phase 3 Balance Pass)

- Does unspent gold carry over at the end of the day, or is it reset?
  (Suggestion: it carries over — a saving strategy should be a valid playstyle
  too.)
- How does the Headman's special mechanic ("doesn't believe in bad fortunes" —
  negative combos are penalized, ROADMAP.md Phase 2) reflect on the economy?
  For now it is only represented by the threshold multiplier (×1.5); the
  special mechanic will be added in Phase 2.
- How does the rewarded ad "second chance" (ROADMAP.md Phase 4) affect the
  economy? Probably a one-time threshold pardon on a lost day — that is outside
  the scope of this draft and will be designed in Phase 4.
