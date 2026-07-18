# Puanlama Formülü

Balatro'daki "Çip × Çarpan" muadili:

```
Müşteri Memnuniyeti =
    ( Σ Sembol Değerleri  +  Σ Sabit Kombo Bonusları  +  Σ Sabit Tılsım Bonusları )
    × ( Π Kombo Çarpanları )
    × ( Π Tılsım Çarpanları )
```

## Adım Adım Hesaplama Sırası

1. **Taban Puan**: Dizilimdeki tüm sembollerin `baseValue` toplamı.
2. **Kombo Tespiti**: 02-combos.md'deki matrise göre dizilimdeki tüm
   komşu çiftler/üçlüler taranır. Çakışma kuralı uygulanır (en yüksek
   çarpanlı kombo kazanır, alt kombolar bastırılır — bkz. 02-combos.md).
3. **Sabit Bonuslar Toplanır**: Tetiklenen kombolardaki tüm `+N` etkiler
   taban puana eklenir.
4. **Kombo Çarpanları Sırayla Uygulanır**: Tetiklenen kombolardaki tüm
   `×N` etkiler, dizilimdeki tetiklenme sırasına göre çarpılır (sıra
   tılsım etkileriyle önemli hale gelir — bkz. "İlk Kombo Çarpanı"
   tılsımı, 05-charms.md).
5. **Tılsım Etkileri Uygulanır**: Aktif tılsımlar sırasıyla kendi
   etkilerini uygular (sembol değerine, kombo çarpanına veya nihai
   sonuca göre değişir — her tılsımın etki hedefi 05-charms.md'de
   tanımlı).
6. **Sonuç**: Müşteri Memnuniyeti puanı çıkar → 04-economy.md'deki
   ödeme formülüne girdi olur.

## Örnek Hesaplama 1 — Tılsımsız

Dizilim: **Yol(3) → Kuş(3) → Mektup(2)**

- Taban puan: 3 + 3 + 2 = **8**
- Kombo taraması: Yol-Kuş, Kuş-Mektup, Yol-Kuş-Mektup (üçlü) hepsi eşleşiyor.
  Çakışma kuralı: üçlü kombo ("Kesin Haber", ×2.5) en yüksek çarpanlı
  olduğu için ikili kombolar bastırılır.
- Sabit bonus: yok (bastırılan ikili kombolardaki +3 "Yolda Haber" de
  aynı üçlü sembol grubuna ait olduğu için sayılmaz — çakışma kuralı
  sabitler için de geçerli).
- Çarpan uygulama: 8 × 2.5 = **20**
- **Müşteri Memnuniyeti = 20**

## Örnek Hesaplama 2 — Tılsımlı

Aynı dizilim + **"İlk Kombo Çarpanı" tılsımı** (tetiklenen ilk kombonun
çarpanını ×2 yapar):

- Taban puan: 8
- Tetiklenen tek kombo "Kesin Haber" (×2.5) zaten dizilimdeki ilk (ve
  tek) kombo → tılsım onu ×2 katlar: 2.5 × 2 = 5.0 efektif çarpan.
- 8 × 5.0 = **40**
- **Müşteri Memnuniyeti = 40**

## Örnek Hesaplama 3 — Negatif Kombo Riski

Dizilim: **Yılan(5) → Dağ(8) → Bulut(2)**

- Taban puan: 5 + 8 + 2 = **15**
- Kombo: üçlü "Kara Gün" tetiklenir (×0.6) — ikili Dağ+Bulut (×0.8) bastırılır.
- 15 × 0.6 = **9**
- **Müşteri Memnuniyeti = 9** (aynı sembolleri farklı sırada dizmek —
  örn. Yılanı en başa değil ortaya koyup üçlüyü bozmak — bu cezayı
  önleyebilirdi. Bu, dizilim kararının neden önemli olduğunun somut
  kanıtı.)

## Tasarım İlkeleri

- **Sabit bonuslar önce, çarpanlar sonra**: erken oyunda küçük
  sabitler anlamlı hissettirir, geç oyunda çarpan yığınları patlama
  yaratır (Balatro'nun "chips then mult" felsefesiyle aynı sıralama
  mantığı).
- **Çarpanlar çarpımsal (sıra bağımsız), sabit tılsımlar toplamsal**:
  hesaplama basit ve öngörülebilir kalır; oyuncu zihinden
  yaklaşık hesap yapabilmeli (Balatro'nun okunabilirlik dersi).
- Tüm katsayılar (`1.5`, `×2`, `+3` vb.) Faz 1'de ScriptableObject
  alanları olarak tutulacak, kod değişikliği gerektirmeden
  dengelenebilecek (ROADMAP.md tasarım kuralı).
- Bu formül bir **taslaktır** — kesin denge Faz 3'te 20+ tam koşu
  verisiyle ayarlanacak (ROADMAP.md Faz 3).
