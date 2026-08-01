# 🔮 Telve — Roadmap

> A deck-building roguelike themed around Turkish coffee fortune-telling.
> Balatro's "one deck, endless combos" loop + the narrative power of
> fortune-telling culture. Target platform: mobile (iOS/Android), engine: Unity.

---

## Phase 0 — Design Foundation (1-2 weeks)

What has to be proven on paper before writing any code:

- [x] **Core loop document**: Flip the cup → 5-7 symbols → arrange into a reading order → combos → score/payment. One page, unambiguously clear. *(implemented in code as the CupDraw → ReadingOrder → ComboDetector → ScoreCalculator chain)*
- [x] **Symbol set v1 (~20-25 symbols)**: Each symbol's name, base value, rarity class, fortune-telling meaning. *(24 symbols exist as ScriptableObjects under `Assets/Resources/Data/Symbols`)*
- [x] **Combo matrix v1**: Adjacent pair/triple combinations and their effects. *(37 combos under `Assets/Resources/Data/Combos`)*
- [x] **Scoring formula**: The equivalent of Balatro's "chips × mult". *(ScoreCalculator + its tests)*
- [ ] **Paper prototype**: Play it on a table with cards. Is the combo-arrangement decision genuinely interesting? If not, fix it here, not in code. *(no evidence of a real human playtest in the repo — this line should only be checked once it has actually been played)*
- [x] **Economy draft**: Balance of customer payment → market prices → end-of-day target (boss "Headman" threshold). *(CustomerEconomy, MarketPricing, DaySession)*

**Exit criterion:** Move on once 3 different people said "one more hand" during the paper prototype.

---

## Phase 1 — Digital Prototype / "Proof of the Puzzle" (3-4 weeks)

The goal is not beauty, it is proving that **the fun works in code too**. Placeholder visuals (plain colored circles + the symbol name as text) are enough.

- [x] Unity project setup (2D URP, portrait mode, 1080×1920 reference resolution)
- [x] Data architecture: symbols and combos as **ScriptableObjects** (so balancing does not require code changes)
- [x] Cup flip → random symbol distribution (set up seeded RNG from the start — the weekly seed will build on it)
- [x] Drag-and-drop reading order arrangement (test with a finger on mobile, not a desktop mouse) *(ReadingOrderChip drag/drop; verified with a mouse on desktop, on-device finger testing still needed)*
- [x] Combo detection + score calculation + result screen
- [x] Single customer loop: arrive → read the fortune → pay → leave
- [x] 8-10 customer day loop + simple Headman (high-threshold customer)
- [x] Market screen v0: buy a symbol/charm from 3 options between customers
- [x] ~10 charms (passive effects: "birds +2 value", "first combo ×2", etc.) *(10 charms under `Assets/Resources/Data/Charms`)*

**Exit criterion:** Move on if you can play 30 minutes straight on your own and you catch yourself thinking "what if I tried that deck with that charm". If you don't, go back to Phase 0.

---

## Phase 2 — Vertical Slice / "The Feel" (4-6 weeks)

Goal: all of the MVP content + the presentation layer that establishes the game's **emotion**. What sold Balatro was the click of the score counter; what will sell Telve is the atmosphere of the cup.

### Presentation
- [x] Lock the art direction: dim table, steam, candlelight, coffee-grounds texture (try 2-3 concepts, pick one) *(3 concepts produced — `Assets/Art/Concepts/`; "B - flat gold linework, midnight blue/gold palette, vector folk-art" was chosen — the most legible option for icon production)*
- [x] Table/steam/candle background + empty cup illustration added to the scene with final art *(`Assets/Art/Background/table_background.png` → `Canvas/Background` fullscreen; `cup_art.png` → `Canvas/CupPanel/CupArt` decorative top visual; verified in Play Mode)*
- [x] Cup flip animation (the game's signature moment — spend time on this) *(GameView.PunchScale — placeholder-quality coroutine tween; the final "signature moment" polish comes after the art direction lock)*
- [x] The "emerging" effect of symbols in the coffee grounds *(GameView.RevealSlot)*
- [x] Combo trigger feedback: name card ("Jealousy Noticed"), screen shake, sound *(ComboBannerView + AudioManager.PlayComboHit; combo_hit.wav produced and assigned)*
- [x] Customer reaction system: flinching, delight, fear (simple portrait + 2-3 expressions) *(CustomerReactionView; flinching/fear merged into the "startled" sprite — v1 scope; 3 portraits produced in the Concept B style, saved under `Assets/Art/Portraits/` and assigned to the scene fields)*
- [x] Audio: porcelain cup clink, spoon, ambient murmur, combo musical hits *(AudioManager; 6 clips produced under `Assets/Audio/` and assigned to the scene fields — cup_draw, combo_hit, purchase, positive/negative_result, ambient_loop)*

### Content completion (MVP scope)
- [x] All 20-25 symbols with final art *(24 symbol icons produced in the Concept B style — `Assets/Art/Symbols/`, assigned to the `SymbolData.sprite` fields; `cupSlotIcons`/`marketOfferIcons` added to GameView so they are actually displayed in the cup and market slots — verified in Play Mode.)*
- [x] ~10 charms final *(10 charm icons produced — `Assets/Art/Charms/`, assigned to the `CharmData.icon` fields, displayed in the market UI.)*
- [x] Headman boss with a special mechanic (e.g. "doesn't believe in bad fortunes" — negative combos are penalized)
- [x] Customer variety: 4-5 archetypes (hasty, skeptical, troubled...) with slight rule differences *(Regular + Hasty/Skeptical/Troubled/Generous = 5 archetypes)*

### Fortune-teller's Journal v1
- [x] A record of discovered combos; the golden-frame moment on first discovery (the viral screenshot moment — the share button goes here) *(ComboJournal + JournalView; the visual polish of the golden frame/share button depends on the final art)*

**Exit criterion:** A 10-minute gameplay video can be recorded and a stranger watching it understands what is going on.

---

## Phase 3 — Meta-Progression + Run Depth (4-5 weeks)

- [x] **Wisdom points**: end-of-run earnings + a permanent unlock tree (StS style: new symbols, starting charm options) *(earning: `WisdomReward`+`MetaProgressStore`; spending: `GameController.UnlockCharacter` unlocks `FalciCharacter` (deck+charm packages) — "new symbols/starting charm options" were merged into character selection, a flat list in v1 rather than a tree structure. Verified end to end in Play Mode.)*
- [x] Unlockable symbol decks (2nd and 3rd decks — different play tendencies: "bird-heavy news deck" etc.) *(merged into character selection — each `FalciCharacter` carries its own deck tendency, see below)*
- [x] 2-3 fortune-teller characters (different starting condition/passive — the equivalent of Balatro's deck selection) *(3 `FalciCharacter`s: Default (free), Bird Fortune-teller (15 wisdom, bird-heavy+Bird Feather), Black Cat Fortune-teller (25 wisdom, cat-heavy+Black Cat Charm). `CharacterSelectView` panel, "Fortune-tellers" button. No art/portraits — name/description text only, final art is separate work.)*
- [x] End-of-run summary screen: best combo, total earnings, journal progress *(`GameController.BestComboThisRun`/`TotalGoldEarnedThisRun`/`DiscoveredCombosCount`, shown in GameView together with "Start New Run", verified in Play Mode)*
- [ ] Difficulty curve: customer expectations rise as the day progresses; a losing run should end in an average of 20-40 min *(the escalation part is already implemented: `CustomerProfile.Regular` — Threshold(n)=12+n×4, exactly matching the formula in docs/design/04-economy.md. A second-based pacing estimate was added to `BalanceSimulator` (rough constants based on the step count in 00-core-loop.md: cup/arrangement/result ~75s, market visit ~12s) and 20 runs were executed: in a realistic upper-bound scenario (optimal arrangement + market) ~12.9 min/run — BELOW the ROADMAP's 20-40 min target. This is a SIMULATION ESTIMATE, not validated by human playtesting, but the signal is clear: the current 8-10 customers/day scope is probably short for the intended session length. The final decision (increasing the customer count / deepening the interaction) should be made after a real playtest — the code side has hit its ceiling with this simulation.)*
- [x] Balance pass: symbol/charm/economy tuning with data from 20+ full runs (fast thanks to ScriptableObjects) *(`BalanceSimulator`: simulation of 20 runs × 3 strategies; the Headman threshold multiplier was lowered 1.5×→1.2×, data in the commit message. Not real human playtest data — automated simulation; individual symbol/charm values have not been tuned separately yet, only the Headman threshold.)*
- [x] Save system: mid-run save/resume (mandatory on mobile), meta-progression persistence *(meta-progression persistence: `MetaProgressStore` — wisdom points, fortune-teller's journal, unlocked/selected character, all persisted with PlayerPrefs. Mid-run save/resume: `RunSaveData`/`RunSaveService` + `DaySession.Restore` + `GameController.SaveRunState`/`RestoreRunState` — verified end to end with a real Stop/Play (equivalent to closing and reopening the app): an open cup, an unsubmitted symbol selection, the day-over summary screen, all come back exactly.)*

**Exit criterion:** Losing a run creates the urge to go "one more", and a meta unlock concretely changes the next run.

---

## Phase 4 — Mobile Polish + Monetization (3-4 weeks)

- [ ] Performance: 60 fps on a low-end Android device, battery friendly *(everything possible on the code side has been done: all 42 textures were given an ASTC 6x6 compression override for Android/iOS (previously they were in the default `overridden: 0` state), anisotropic filtering was disabled at every level in Quality Settings (unnecessary for 2D sprites), `vSyncCount=0` + `targetFrameRate=60` were added to `GameController.Awake()`. The code was scanned for performance smells such as Update()/FindObjectOfType and none were found. No fps/battery measurement was done on a REAL physical low-end Android device — this item can only be checked with a device test.)*
- [x] **First real Android build (AAB) + signing key (keystore) setup** *(2026-07-31: the Android Build Support module and the bundled SDK/NDK/JDK were already installed (Unity 6000.1.17f1), the package name `com.yilkgames.telve` was already correct. A new upload keystore was generated (`android-keystore/telve-upload.jks`, PKCS12, alias `telve-upload`) — gitignored inside the project, with its password and file backed up both on the desktop and in the private `Eren-Ozcan/pictures` repo (`pictures/Telv/android-keystore/`). It was wired into Unity Player Settings (`useCustomKeystore`); only the file path/alias name is written into ProjectSettings.asset, the password never enters the repo. The first end-to-end test build (`BuildPipeline.BuildPlayer`, Android target, App Bundle) **SUCCEEDED**: 0 errors, 2 warnings, a signed 55.7 MB `.aab` was produced (`android-keystore/build-output/telve-test.aab`, gitignored). During the build the "Active Input Handling = Both" warning dialog halted it (the Input System package is not installed in the project, so "Both" was meaningless/risky) — `activeInputHandler: 2 → 0` ("Input Manager (Old)") was fixed in `ProjectSettings.asset`, so future builds will not stop on this dialog. Remaining: verifying that this AAB actually runs on a real device and actually uploading it to the Play Console internal test track — neither was done in this session.)*
- [x] **Real portrait (1080×1920) Play Mode test for store screenshots + critical visual bug fix** *(2026-07-31: when the Unity Game View was run at a real portrait resolution for the first time (previously it had always been tested in landscape/free aspect), a systemic data error was found in 25 RectTransforms in the `Game.unity` scene (the cup's 8 symbol slots + the market's 3 offer rows + the background + the cup art + the KVKK consent panel) where the "0." prefix had been dropped from the `anchorMin`/`pivot` values — e.g. the pivot should have been `0.5` instead of `5`, anchorMin.x should have been `0.391` instead of `391`. This bug had previously been noticed and fixed only on `ConsentText`/`AcceptButton` (see the Phase 4 GDPR/KVKK item); the same bug also existed on the cup symbols and market icons and had never been noticed because it was not tested at a real portrait aspect ratio — the symbol icons were spilling far outside/above the screen. The whole scene was scanned with a script in the Unity Editor (every `anchorMin`/`anchorMax`/`pivot` value outside 0-1 was fixed by prefixing "0.") and the scene was saved; verified end to end in Play Mode with coplay-mcp (the cup symbols now display correctly). Along the way, 4 real Play Mode screenshots were captured — see `docs/store-listing.md`.)*
- [ ] IAP infrastructure (Unity IAP): cup/tablecloth cosmetics — we are not selling power, only looks *(alongside the `CosmeticItem` data model + `IPurchaseService`/`MockPurchaseService` (the default, for dev/QA), there is now a real `UnityIAPPurchaseService` too — the com.unity.purchasing 4.12.2 package was installed, `IStoreListener` was fully implemented, compilation verified. There is progress on the Play Console side as well: an app listing was created for Telv (`com.yilkgames.telve`, App ID 4975250454079860772, Unity `applicationIdentifier` matched), and the store text/category/tags/ads/content rating/target audience declarations were entered. THERE IS STILL NO REAL STORE CONNECTION: the product catalog matching the `cosmeticId`s has not been created in the Play Console yet, so `UnityPurchasing.Initialize` cannot be tested end to end and it was not assigned as the default in `GameController.Awake()`. Also, the store listing/Data Safety form cannot be fully submitted until the privacy policy URL is published (it is in draft state) — see `docs/store-listing.md`.)*
- [x] Rewarded ad integration: end-of-run "second chance" + wisdom points ×2 (2 spots, more than that eats the experience) *(alongside `IRewardedAdService`/`MockRewardedAdService` (the default, for dev/QA), there is now a real `LevelPlayRewardedAdService` too — the com.unity.services.levelplay 9.2.0 package was installed (LevelPlay mediation was used because the legacy Unity Ads SDK was sunset on 2026-01-31), compilation verified. The gameplay flow is fully wired: `DaySession.TryGrantSecondChance` + `GameController.RequestSecondChance`/`RequestDoubleWisdom`, once each per run, verified end to end in Play Mode. THERE IS STILL NO REAL LevelPlay account/app key/ad unit ID — it was not assigned as the default until an account is set up.)*
- [x] Tutorial / first 5 minutes flow (first customer = tutorial fortune) *(`TutorialView` — contextual hints on the first never-seen encounter (arrange → read → result), never shown again. Verified in Play Mode.)*
- [x] Localization infrastructure: TR + EN from the start (the English equivalents of fortune-telling terminology are separate work — start early) *(`Localization`/`LocalizedText`/`LanguageToggleView` + the `LocalizationTable` data model; static UI texts are wired to TR/EN. The fortune-telling terminology that Phase 4's own note called "separate work" is also done: the new `ContentLocalizationTable`/`ContentLocalization` (`Assets/Scripts/Meta/ContentLocalization.cs`) maps the EN name/description of 24 symbols + 37 combos + 10 charms + 3 characters by id (the source TR assets were not changed). The dynamic/interpolated status texts inside GameView.Refresh() (gold/score/customer order, market labels, run summary, combo banner, character selection status) are now wired to TR/EN via template keys in the `UiStrings` table. Verified through coplay-mcp that Unity opens with no compile errors and that the language switch (status text, market, run summary, combo banner, character selection) correctly toggles TR↔EN in Play Mode.)*
- [ ] Analytics: run completion, death points, combo discovery rates, D1/D7 retention *(the event infrastructure is ready — `AnalyticsEvents`/`IAnalyticsSink`, with a local file sink as the default; run_started/run_ended/death_point/combo_discovered are being logged, verified in Play Mode. D1/D7 retention requires a real backend + multi-session data, which does not exist yet — it will work automatically once a real sink is connected.)*
- [ ] GDPR/KVKK/ATT consent flows, offline playability *(offline playability is already guaranteed — there is not a single network call in the codebase, all persistence is PlayerPrefs. A consent gate skeleton was set up with `PrivacyConsentView`. The placeholder text was replaced with a realistic KVKK/GDPR disclosure draft (`Game.unity` → `ConsentText`: data controller, data processed through the LevelPlay/ironSource ad network, KVKK art. 11 rights, contact address, a note that the iOS ATT system permission will be requested separately). During Play Mode verification a CRITICAL scene bug was found and fixed: the `anchorMin`/`anchorMax`/`pivot` values of `ConsentText` and `AcceptButton` had been entered as `5` instead of `0.5` (probably due to an old data entry error) — because of this, both the consent text and the accept button fell far outside the screen, so the gate was visible but its content and button never were (the game could effectively never be started). After the fix it was verified end to end with coplay-mcp: the text is readable, "I Accept" can be clicked, the gate closes and the game starts. The item is still not checked, because (1) this draft has not gone through an independent legal review, and (2) the iOS ATT system permission integration (code side) has not been done yet.)*

**Exit criterion:** 10 strangers can finish one day loop on a phone without a tutorial and without help.

---

## Phase 5 — Soft Launch (4-6 weeks)

- [ ] Closed beta (TestFlight / Play Internal): 50-100 players, feedback form
- [ ] Single-market soft launch (suggestion: Turkey — the theme is local, feedback quality is high)
- [ ] Metric targets: D1 ≥ 35%, D7 ≥ 12%, avg. session ≥ 15 min (reasonable thresholds for a roguelike)
- [ ] Balance and economy patch rounds (weekly)
- [ ] Store page: screenshots, 30 sec trailer (cup flip + the golden-frame discovery moment)
- [ ] Content creator seeding: fortune/coffee themed TikTok-Instagram micro-influencers, "what did your fortune say" format

**Exit criterion:** The retention targets are met OR the reason they are not has been diagnosed and fixed.

---

## Phase 6 — Global Launch + Live Service (ongoing)

- [ ] Global release (iOS + Android simultaneously)
- [ ] **Weekly seed + leaderboard** (ready in launch week — this is the community ritual)
- [ ] Seasonal deck packs (they sell variety, not power): season 1 theme ready
- [ ] Monthly balance + content patch rhythm: new symbols, new charms, new customer archetypes
- [ ] Roadmap v2: new bosses (mother-in-law? the coffeehouse apprentice?), daily challenge, PC/Steam port evaluation

---

## Risks and Early Answers

| Risk | Mitigation |
|---|---|
| The combo arrangement decision stays shallow ("the same order is always best") | Catch it in the Phase 0 paper prototype; deepen order dependency (adjacency + position bonuses) |
| The fortune-telling theme is not understood abroad | Lean on the recognizability of "tasseography" in the EN localization; let the tutorial teach the theme in 30 sec |
| The Balatro wave dies down | Theme originality is the real insurance; do not grow the MVP scope for speed |
| Solo developer burnout | Stay faithful to the phases' exit criteria; keep a "v2 list" against scope creep |
| Balance hell | All numbers live in ScriptableObjects; track combo usage rates with analytics |

---

## Rough Timeline

Assuming a solo developer at a more-than-part-time pace:

| Phase | Duration | Cumulative |
|---|---|---|
| 0 — Design foundation | 1-2 weeks | ~2 weeks |
| 1 — Digital prototype | 3-4 weeks | ~6 weeks |
| 2 — Vertical slice | 4-6 weeks | ~3 months |
| 3 — Meta-progression | 4-5 weeks | ~4 months |
| 4 — Mobile polish + money | 3-4 weeks | ~5 months |
| 5 — Soft launch | 4-6 weeks | ~6-6.5 months |
| 6 — Global launch | — | ~month 7 |

> The most critical rule: **do not spend money/time on Phase 2 art before Phase 1 is finished.**
> If the puzzle is not fun, atmosphere will not save it; if it is fun, atmosphere will make it legendary.
