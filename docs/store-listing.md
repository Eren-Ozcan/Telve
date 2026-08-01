# Google Play Store Listing

The texts to be entered into the Play Console. Mind the character limits: title
30, short description 80, full description 4000. The "Default language" in the
Play Console was set to English (en-US) (2026-07-31), which is why the main text
is in English. Before the Turkey soft launch (ROADMAP.md Phase 5), a Turkish
translation must be added as a separate language entry — the in-game TR/EN
localization already exists (`ContentLocalization`), so the store text can be
adapted from there.

## App name (30 characters)

```
Telve
```

## Short description (80 characters)

```
Turkish coffee fortune roguelike — flip the cup, read the symbols, build luck.
```

## Full description

```
Flip the cup. Read the grounds. Build your fortune.

Telve is a roguelike deckbuilder built around tasseography — the old art of
reading fortunes in Turkish coffee grounds. Draw symbols from your cup,
arrange them into a reading order, and watch adjacent symbols trigger
combos for bigger payouts. One cup, endless combinations.

FEATURES

☕ Authentic tasseography theme — 24 symbols drawn from real Turkish coffee
fortune-telling tradition, each with its own meaning

🔮 37 combos to discover — some multiply your score, some are deliberately
risky, and figuring out which symbols to keep apart is half the game

✨ 10 passive charms — shape your reading strategy across a run, no two
decks play quite the same

👥 Five customer archetypes — the hasty, the skeptical, the troubled, the
generous, and the regular customer, each nudging your strategy differently

👑 The Headman — the toughest customer of your day, unimpressed by weak
fortunes

📖 Fortune-teller's Journal — every combo you discover gets recorded, with
a golden frame for your first find

🌟 Wisdom points & unlockable fortune-tellers — three fortune-tellers with
different starting decks and passives, unlocked with points earned from
past runs

🌙 Atmosphere first — candlelight, steam, porcelain clinks, and a table
that feels like it's waiting for you

🌐 English & Turkish — full localization, switch anytime

🔌 No account required — play offline, your progress stays on your device

Simple to learn, hard to put down. Every cup is a new fortune — what will
yours say?
```

## Category

Games > Card (shown as "Kağıt" in the Turkish Play Console UI)

## Tags

The Play Console only allows selecting from a predefined tag list (not free
text) — there is no exact match for tags like "tarot", "coffee" or "fortune".
Selected: **Puzzle, Casual, Roguelike games** (chosen in the Turkish Play
Console UI as "Bulmaca, Gündelik, Roguelike oyunlar").

## Graphic requirements

- [ ] App icon 512×512 PNG — not produced yet
- [ ] Feature graphic 1024×500 PNG (24-bit, no alpha) — not produced yet
- [x] Screenshots (at least 2, 4-8 recommended) — 4 real Unity Editor Play Mode
  captures were taken (coplay-mcp + `ScreenCapture`, at the portrait 1080×1920
  Game View size): full cup (`01_cup_full.png`), reading order selection
  (`02_reading_order.png`), result/score screen (`03_result.png`), market screen
  (`04_market.png`). They are under
  `docs/store-assets-originals/screenshots/` (gitignored).
  Retaking them on a real Android device can be merged with the "real device
  test" item of Phase 4, but this is enough for the Play Console entry.
- Per the CLAUDE.md rule, the visuals to be produced will not be saved in this
  public repo but in `docs/store-assets-originals/` (gitignored) + the private
  `Eren-Ozcan/pictures` repo (`pictures/Telv/`)

## Ads / Data Safety notes (Play Console) — as of 2026-07-31

- The app shows rewarded ads through LevelPlay (ironSource) mediation
  (`LevelPlayRewardedAdService` — end-of-run "second chance" and "wisdom ×2").
  There is no real LevelPlay account/app key yet (ROADMAP.md Phase 4).
- **App content → Ads**: "My app contains ads" = Yes was selected and saved. ✅
- **Advertising ID (Ad ID) declaration**: "Yes, it uses it" + purpose =
  "Advertising or marketing" was selected and saved. ✅
- **Data safety form**: Device or other IDs (advertising ID), collected +
  shared, purpose = Advertising or marketing, not processed ephemerally,
  collection is required, transfer is encrypted. After the privacy policy URL
  was added it was **fully submitted** (no longer a draft). ✅ No location data
  is collected and there is no account system ("My app does not allow users to
  create an account" was selected).
- **Privacy policy**: `https://yilkgames.com/privacy-policy/` was entered into
  the Play Console and saved. ✅ This is a single studio-wide page (covering
  Reefy, Little Grand Hotel, Çengel Bulmaca, Dleverse, Telve, Lightwake, Domina
  and CosmicRumble) — it is updated automatically when a new game is added, and
  no Telv-specific page is required.
- **Content rating questionnaire**: completed and submitted. ✅ It came out at
  the lowest/General category across all authorities (AG, L, E, 3, 0, 3+, 3 —
  ESRB/PEGI/USK/ClassInd etc.), the same profile as cengeBulmaca.
- **Target audience**: set to 13-15, 16-17 and 18+ and saved. ✅ (consistent
  with cengeBulmaca — the children's categories were excluded to avoid the
  extra "Designed for Families" restrictions in the ads/data policies)
- **Government apps**: No, saved. ✅
- **Financial features**: saved as "Not provided". ✅
- **Health apps**: saved as "None". ✅
- IAP: cosmetic cup/tablecloth items (`CosmeticItem`, `UnityIAPPurchaseService`)
  — they do not sell power, only looks. The Play Console product catalog has not
  been created yet.
- App category: Card ("Kağıt"). Tags: Puzzle, Casual, Roguelike games (selected
  from the Play Console's predefined tag list — there are no free-text tags like
  "tarot"/"coffee"/"fortune").
- Store listing text (primary language English): app name, short description and
  full description were entered and **saved as a draft**. ✅ (draft)

## Remaining

- ⏳ Store visuals: the 512×512 icon and the 1024×500 feature graphic have not
  been produced yet (the screenshots are done, see above) — without them the
  store listing cannot be fully submitted with "Save" in the Play Console and
  stays a draft. Prompt drafts: `docs/store-visual-prompts.md`.
- ⏳ Internal test track — the keystore was created (`android-keystore/telve-upload.jks`,
  gitignored, backed up in the private `pictures/Telv/android-keystore/` repo)
  and the first signed test AAB was produced successfully (0 errors, 55.7 MB,
  `android-keystore/build-output/telve-test.aab`, see ROADMAP.md Phase 4).
  Remaining: verifying that this AAB installs and runs on a real device, and
  actually uploading it to the Play Console.
- ⏳ The IAP product catalog and a real LevelPlay account.
- ⏳ Turkish store text (to be added as a secondary language).
