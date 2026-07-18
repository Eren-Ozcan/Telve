# Kombo Matrisi v1

37 kombo (31 ikili + 6 üçlü). Komşuluk = okuma sırasında yan yana
duran semboller (pozisyon i ve i+1). Üçlü kombolar 3 ardışık pozisyonu
kapsar.

**Çakışma kuralı:** Aynı sembol grubu hem ikili hem üçlü kombo
tetikliyorsa (örn. Yol-Kuş-Mektup ardışıksa hem "Haber Geliyor" hem
"Kesin Haber" tetiklenebilir gibi görünür), sadece **en yüksek çarpanlı
kombo** sayılır — alt kombolar bastırılır. Çift sayım yok. Detay için
bkz. 03-scoring.md.

**Etki türleri:** `×N` (çarpan, sırayla çarpılır) veya `+N` (sabit,
çarpanlardan önce taban puana eklenir).

## Haber Grubu

| Kombo | İsim | Etki |
|---|---|---|
| Yol + Kuş | Haber Geliyor | ×1.5 |
| Kuş + Mektup | Beklenen Mektup | ×1.5 |
| Mektup + Yol | Yolda Haber | +3 |
| Kuş + Çan | Düğün Haberi | ×1.8 |

## Aşk Grubu

| Kombo | İsim | Etki |
|---|---|---|
| Kalp + Yıldız | Kader Aşkı | ×2.0 |
| Kalp + Ay | Gizli Aşk | ×1.6 |
| Kalp + Kalp | Aşk Üçgeni | ×1.7 |
| Kalp + Kapı | Yeni Aşk | +4 |
| Kalp + Yılan | İhanete Uğrayan Aşk | ×1.4 |

## Uyarı Grubu (bazıları kasıtlı negatif — riskli dizilim dersi)

| Kombo | İsim | Etki |
|---|---|---|
| Yılan + Kedi | İki Yüzlü Düşman | ×1.9 |
| Dağ + Bulut | Zorlu Engel | ×0.8 |
| Yılan + Göz | Nazar Kırılıyor | ×1.3 |
| Kedi + Mektup | Dedikodu | ×0.9 |

## Bereket / Başarı Grubu

| Kombo | İsim | Etki |
|---|---|---|
| Balık + Güneş | Bol Kazanç | ×2.2 |
| Balık + Anahtar | Yeni Fırsat Kapısı | +5 |
| Güneş + Taç | Zaferin Zirvesi | ×3.0 |
| Merdiven + Güneş | Yükselen Yıldız | ×1.8 |

## Yolculuk Grubu

| Kombo | İsim | Etki |
|---|---|---|
| Yol + Gemi | Uzak Yolculuk | ×1.6 |
| Gemi + Balık | Bereketli Deniz | +6 |
| Köprü + Kapı | Geçiş Dönemi | ×1.5 |
| Yol + Dağ | Zorlu Yolculuk | ×1.2, +2 |
| Anahtar + Kapı | Kaderin Kapısı | ×2.0 |

## Sır Grubu

| Kombo | İsim | Etki |
|---|---|---|
| Kuyu + Ay | Derin Sır | ×1.7 |
| Kuyu + Göz | Görülen Sır | ×1.4 |
| Ay + Bulut | Belirsiz Gelecek | ×0.85 |

## Aile / Büyüme Grubu

| Kombo | İsim | Etki |
|---|---|---|
| Ağaç + Kalp | Aile Mutluluğu | ×1.6 |
| Ağaç + Çiçek | Yeni Nesil | +5 |
| Çiçek + Kalp | Romantik Sürpriz | ×1.5 |

## Diğer İkili Kombolar

| Kombo | İsim | Etki |
|---|---|---|
| Göz + Yıldız | Korunan Dilek | ×1.4 |
| Kapı + Yol | Yeni Yola Çıkış | +3 |
| Çan + Çiçek | Mutlu Tören | ×1.5 |

## Üçlü Kombolar (yüksek risk / yüksek ödül)

| Kombo (ardışık 3) | İsim | Etki |
|---|---|---|
| Yol + Kuş + Mektup | Kesin Haber | ×2.5 |
| Kalp + Yıldız + Taç | Büyük Aşk Kaderi | ×3.5 |
| Balık + Güneş + Taç | Efsanevi Bereket | ×4.0 |
| Yılan + Dağ + Bulut | Kara Gün | ×0.6 |
| Anahtar + Kapı + Köprü | Yeni Hayat Yolu | ×2.8 |
| Göz + Yıldız + Ay | Gökyüzü Falı | ×2.2 |

Toplam: 31 ikili + 6 üçlü = **37 kombo** (MVP hedefi 30-40 aralığında).

## Tasarım Notları

- Negatif kombolar (Dağ+Bulut, Kedi+Mektup, Ay+Bulut, Kara Gün)
  kasıtlı: oyuncuyu "her sembolü en güçlü kombonun yanına koy" saf
  optimizasyonundan çıkarıp *hangi sembolleri birbirinden uzak tutmalıyım*
  kararına zorlar. Kağıt prototipte bu kararın gerçekten hissedilip
  hissedilmediği test edilmeli (ROADMAP.md Faz 0 çıkış kriteri).
- "Kara Gün" (×0.6) MVP'deki en sert ceza — üç negatif sembolün
  bilerek yan yana getirilmesi gerektiği için nadiren tetiklenir, esas
  işlevi deste kurarken negatif sembolleri "dağıtma" stratejisini
  öğretmek.
- Faz 1'de her kombo bir `ComboData` ScriptableObject'i olacak:
  `id, requiredSymbolIds (sıralı liste), displayName, effectType
  (Multiplier/Flat), effectValue, isNegative`.
- Kombo tespiti performans notu: pozisyon bazlı O(n) tarama yeterli
  (n ≤ 8 sembol), erken optimizasyona gerek yok.
