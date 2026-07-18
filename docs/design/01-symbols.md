# Sembol Seti v1

24 sembol, 4 nadirlik sınıfı. Nadirlik hem çekiliş ağırlığını hem baz
değer aralığını belirler (nadir sembol = düşük çekiliş şansı ama
yüksek taban puan).

Çekiliş ağırlığı (Faz 1'de ScriptableObject alanı `drawWeight`):
Common 100, Uncommon 45, Rare 15, Epic 4.

## Common — taban değer 2-4 (12 sembol)

| Sembol | Değer | Fal Anlamı |
|---|---|---|
| Yol | 3 | Yolculuk, değişim, yeni yön |
| Kuş | 3 | Haber, müjde |
| Kalp | 3 | Aşk, duygusal bağ |
| Göz | 2 | Nazar, korunma |
| Ağaç | 3 | Aile, kök, büyüme |
| Balık | 3 | Bereket, para |
| Bulut | 2 | Belirsizlik, endişe |
| Çiçek | 3 | Güzel haber, teklif |
| Ay | 3 | Değişim, gizli duygu |
| Merdiven | 4 | Yükseliş, terfi |
| Kapı | 3 | Yeni başlangıç, fırsat |
| Mektup | 2 | Beklenen haber |

## Uncommon — taban değer 4-5 (7 sembol)

| Sembol | Değer | Fal Anlamı |
|---|---|---|
| Yılan | 5 | Düşman, ihanet, dedikodu |
| Gemi | 5 | Uzak yolculuk |
| Anahtar | 4 | Yeni fırsat, çözüm |
| Kedi | 4 | Kıskançlık, hile |
| Köprü | 5 | Geçiş dönemi, karar anı |
| Çan | 4 | Düğün/tören haberi |
| Ok | 5 | Yön, kesin karar |

## Rare — taban değer 7-8 (4 sembol)

| Sembol | Değer | Fal Anlamı |
|---|---|---|
| Dağ | 8 | Büyük engel, zorluk |
| Yıldız | 7 | Dilek, umut |
| Güneş | 8 | Başarı, mutluluk |
| Kuyu | 7 | Derin sır |

## Epic — taban değer 12 (1 sembol)

| Sembol | Değer | Fal Anlamı |
|---|---|---|
| Taç | 12 | Büyük güç, kaderin dönüm noktası |

## Notlar

- Toplam 24 sembol: MVP hedefi olan 20-25 aralığında.
- Değerler kasıtlı düşük tutuldu — asıl skor patlaması kombo
  çarpanlarından gelmeli (bkz. 03-scoring.md), yoksa "en yüksek değerli
  sembolleri sıraya koy" tek stratejisi baskın olur ve dizilim kararı
  anlamsızlaşır.
- Faz 1'de her sembol bir `SymbolData` ScriptableObject'i olacak:
  `id, displayName, baseValue, rarity, drawWeight, spriteRef,
  falMeaningText`.
- Faz 2'de sembol başına ayrı final görsel + "belirme" efekti planlanıyor
  (ROADMAP.md Faz 2).
