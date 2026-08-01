# Store Visual Generation Prompts

This file contains the generation prompts for the Play Console store visuals
(icon, feature graphic). Per the CLAUDE.md rule, the generated visuals are not
committed to this public repo — they must be saved to
`docs/store-assets-originals/` (gitignored) + the private `Eren-Ozcan/pictures`
repo (`pictures/Telv/`).

## Style DNA (the shared basis of every prompt)

All visuals must be exactly consistent with the style of
`Assets/Art/Concepts/concept_B_flatgold.png` ("Concept B — flat gold linework,
midnight blue/gold palette, vector folk-art"). Files to look at for reference:
`Assets/Art/Background/cup_art.png`, `Assets/Art/Symbols/symbol_kus.png`,
`Assets/Art/Portraits/customer_happy.png`.

Shared style definition (can be prepended to every prompt):

```
Deep midnight navy background (#0b1130 to #10173f range), fine elegant gold
linework in an art-nouveau mystical folk-art style — NOT flat cartoon,
NOT 3D render, NOT photorealistic. Celestial motifs: thin four-point
sparkle stars, small eight-point stars, scattered dot-stars like a
constellation, crescent moons. Ottoman/Turkish tulip filigree scrollwork
on circular or cup elements. Warm cream/ivory and amber-gold highlights
against the cool navy for contrast. Mystical, cozy, nocturnal
tasseography (Turkish coffee fortune-telling) atmosphere. Clean vector
linework, no visible brush texture, no photographic elements.
```

## 1. App icon (512×512, must read clearly on its own)

The icon has to stay legible even at the smallest size (~48px on a phone home
screen) — this is exactly why Concept B was chosen as "the most legible option
for icon production" (ROADMAP.md Phase 2). The icon must be a SINGLE and BOLD
motif, not a scene.

**Main option — top-down cup:**

```
[style DNA here]

App icon, square 512x512, centered composition filling the frame edge to
edge. A single Turkish coffee cup and saucer viewed from directly above,
rendered in thick bold gold linework on a deep navy background. Inside
the cup rim, the coffee grounds (telve) form a simple silhouette of a
crescent moon and a single star — dark brown-black grounds against the
warm ivory cup interior. The saucer has a simple ring of gold Ottoman
tulip filigree. Bold, thick outlines so the icon reads clearly at very
small sizes (down to 48px). Square format, flat background color fills
every corner (no transparency, no rounded corners — Play Store adds its
own icon mask).
```

**Alternative — a single symbol:**

```
[style DNA here]

App icon, square 512x512, centered composition. A single bold gold-line
crescent moon with a star nested inside its curve, in the same folk-art
mystical linework style as symbol_kus.png, but simplified to just the
moon-and-star motif with no additional scatter stars (keep it bold and
uncluttered for legibility at small sizes). Deep navy background fills
the full square, edge to edge. No text, no additional ornamentation.
```

## 2. Feature graphic (1024×500, Play Store banner)

Text (the game name "TELVE") should not be baked into the image by the AI —
generation models usually mangle plain text. Generate the image without text and
add the "TELVE" wordmark afterwards (in Figma/Photoshop) as a separate layer;
for that reason, ask for a composition that leaves a relatively plain/dark area
on the left half (or at the top).

```
[style DNA here]

Wide banner, 1024x500, landscape orientation. A cozy fortune-telling
table scene at night: a Turkish coffee cup and saucer with gold
filigree sits center-right, rendered in the same top-down or slight
three-quarter angle as cup_art.png. Two lit candles with warm amber
glow flank the scene, thin wisps of steam rising in soft gold-tinted
curves. Scattered tarot-card-like rectangles with gold-outlined star
and moon icons sit in the lower-right corner. The left third of the
composition is a simpler, darker navy area with only faint scattered
stars and a large crescent moon — reserved as empty space for a game
logo/title to be added afterward. No text baked into the image. Same
fine gold linework, deep navy palette as the rest of the game's art.
```

## 3. Screenshots (Play Console: at least 2, 4-8 recommended)

AI-generated fake interfaces **must not be used** for screenshots — Play Store
policy requires them to reflect the real app content. These must be real Play
Mode captures (moments such as the cup flip, arranging the reading order, the
combo banner, the market screen, the run summary).

Unity is currently open and the coplay-mcp connection is working — say the word
if you want me to take the real Play Mode screenshots now, and I will follow the
steps below:
1. Capture 4-6 different game moments with `capture_ui_canvas` (full cup, while
   the reading order is being arranged, when a combo triggers, with the market
   open, the run summary screen).
2. Save them under `docs/store-assets-originals/screenshots/` (gitignored).
3. If you want, I can also prepare a simple decorative frame/overlay prompt for
   adding a short caption to each one (e.g. "Flip the cup, read your fortune") —
   in that case the frame is AI-generated while the real interface capture
   inside it stays unchanged.

If you want, here is a separate prompt for that decorative frame:

```
[style DNA here]

Phone screenshot decorative frame/background, portrait orientation
1080x1920, designed to have a game UI screenshot composited on top
of the center 90% of the frame. Only the outer 5% margin has visible
ornamentation: a thin gold border with small corner flourishes (star
and crescent moon accents in the four corners), matching the same
folk-art linework style. The rest of the frame is a soft dark navy
gradient that will mostly be covered by the actual screenshot. Leave
the bottom ~10% as a slightly darker solid navy bar with room for a
short white/gold caption text to be added afterward.
```
