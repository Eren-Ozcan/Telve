# Store Visual Generation Prompts

Bu dosya, Play Console mağaza görselleri (ikon, feature graphic) için üretim
promptları içerir. Üretilen görseller CLAUDE.md kuralına göre bu public repoya
commit edilmez — `docs/store-assets-originals/` (gitignore'lu) + private
`Eren-Ozcan/pictures` reposuna (`pictures/Telv/`) kaydedilmeli.

## Stil DNA'sı (her promptun ortak temeli)

Tüm görseller `Assets/Art/Concepts/concept_B_flatgold.png` ("Concept B —
düz altın çizgi, gece mavisi/altın palet, vektörel folk-art") stiliyle
birebir tutarlı olmalı. Referans için bakılacak dosyalar:
`Assets/Art/Background/cup_art.png`, `Assets/Art/Symbols/symbol_kus.png`,
`Assets/Art/Portraits/customer_happy.png`.

Ortak stil tanımı (her promptun başına eklenebilir):

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

## 1. Uygulama simgesi (512×512, tek başına net okunmalı)

İkon en küçük boyutta (telefon ana ekranında ~48px) bile okunaklı olmalı —
bu yüzden Concept B zaten "ikon üretimi için en okunaklı seçenek" olarak
seçilmişti (ROADMAP.md Faz 2). İkon TEK ve KALIN bir motif olmalı, sahne
değil.

**Ana seçenek — kuştan bakış fincan:**

```
[stil DNA'sı buraya]

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

**Alternatif — tek sembol:**

```
[stil DNA'sı buraya]

App icon, square 512x512, centered composition. A single bold gold-line
crescent moon with a star nested inside its curve, in the same folk-art
mystical linework style as symbol_kus.png, but simplified to just the
moon-and-star motif with no additional scatter stars (keep it bold and
uncluttered for legibility at small sizes). Deep navy background fills
the full square, edge to edge. No text, no additional ornamentation.
```

## 2. Feature graphic (1024×500, Play Store banner)

Metin (oyun adı "TELVE") görsele AI tarafından gömülmemeli — üretim
modelleri düz yazıyı genelde bozar. Görseli metinsiz üret, "TELVE"
wordmark'ını sonradan (Figma/Photoshop) ayrı bir katman olarak ekle; bu
yüzden sol yarıda (veya üstte) görece sade/koyu bir alan bırakan bir
kompozisyon iste.

```
[stil DNA'sı buraya]

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

## 3. Ekran görüntüleri (Play Console: en az 2, önerilen 4-8)

Ekran görüntüleri için AI-üretilmiş sahte arayüz **kullanılmamalı** —
Play Store politikası gerçek uygulama içeriğini yansıtmasını istiyor.
Bunlar gerçek Play Mode ekran yakalamaları olmalı (fincan çevirme,
okuma sırası dizme, kombo banner'ı, pazar ekranı, koşu özeti gibi anlar).

Unity şu an açık ve coplay-mcp bağlantısı çalışıyor durumda — gerçek
Play Mode ekran görüntülerini şimdi almamı isterseniz söyleyin, aşağıdaki
adımları izlerim:
1. `capture_ui_canvas` ile 4-6 farklı oyun anını yakala (fincan dolu,
   okuma sırası dizilirken, kombo tetiklenince, pazar açıkken, koşu
   özeti ekranı).
2. Bunları `docs/store-assets-originals/screenshots/` altına kaydet
   (gitignore'lu).
3. İsterseniz her birine kısa bir başlık (örn. "Flip the cup, read your
   fortune") eklemek için basit bir dekoratif çerçeve/overlay prompt'u
   da hazırlarım — bu durumda çerçeve AI ile üretilir, içindeki gerçek
   arayüz görüntüsü değişmeden kalır.

İstersen bu dekoratif çerçeve için ayrı bir prompt:

```
[stil DNA'sı buraya]

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
