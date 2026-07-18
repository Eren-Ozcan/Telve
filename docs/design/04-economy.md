# Ekonomi Taslağı

> Bu bir **taslaktır**, kesin sayılar değildir. ROADMAP.md Faz 3'te
> 20+ tam koşu verisiyle yeniden dengelenecek. Buradaki amaç,
> döngünün uçtan uca **oynanabilir** olması — her sayı ScriptableObject'te
> tutulacağı için değiştirmek kod değişikliği gerektirmeyecek.

## Başlangıç Durumu

- Başlangıç altını: **20**
- Başlangıç destesi: Faz 1'de tanımlanacak temel sembol alt kümesi
  (ör. 24 sembolün en yaygın 10 Common'ı) + 0 tılsım.

## Müşteri Eşikleri (Gün İçi Zorluk Eğrisi)

8 sıradan müşteri + 1 muhtar (gün sonu boss).

```
Eşik(n)  = 12 + n × 4        (n = müşteri sırası, 1-8)
Ödeme(n) = 6 + n × 2 taban   + (Memnuniyet - Eşik) × 0.3  [eşik aşılırsa]
```

| Müşteri | Eşik | Taban Ödeme | Not |
|---|---|---|---|
| 1 | 16 | 8 | Öğretici zorluk |
| 2 | 20 | 10 | |
| 3 | 24 | 12 | |
| 4 | 28 | 14 | |
| 5 | 32 | 16 | |
| 6 | 36 | 18 | |
| 7 | 40 | 20 | |
| 8 | 44 | 22 | |
| **Muhtar** | **66** (Eşik(8) × 1.5) | **35** | Gün sonu boss |

- Eşik **aşılırsa**: `Taban Ödeme + (Memnuniyet - Eşik) × 0.3` altın
  kazanılır, müşteri olumlu tepki verir, defter/itibar puanı işler.
- Eşik **aşılamazsa**: taban ödemenin **%40**'ı verilir (fal yine de
  bir şeyler söyler, ama müşteri tatmin olmaz), olumsuz tepki animasyonu
  oynar.
- **Muhtar aşılamazsa**: gün kaybedilir, koşu sona erer (roguelike
  ölüm koşulu).

## Pazar Fiyatları

Müşteriler arası pazara uğranarak deste güçlendirilir.

| Nadirlik | Sembol Fiyatı | Tılsım Fiyatı |
|---|---|---|
| Common | 8 | 12 |
| Uncommon | 15 | 22 |
| Rare | 28 | 38 |
| Epic | 50 | 65 |

- Her pazar ziyaretinde 3 seçenek sunulur (ROADMAP.md Faz 1: "3
  seçenekten sembol/tılsım satın alma").
- Seçenekler o güne kadar açılmış nadirlik havuzundan rastgele çekilir
  (Epic havuzu ilk günlerde kilitli olabilir — Faz 3'te meta-ilerleme
  ile açılması değerlendirilecek).

## Gün Sonu Hedefi Dengesi

Bir günün toplam potansiyel kazancı (8 müşteri taban ödemesi + muhtar,
bonus hariç):

```
8 + 10 + 12 + 14 + 16 + 18 + 20 + 22 + 35 = 155 altın (taban, gün sonu toplamı)
```

- Bu, oyuncunun gün içinde ortalama **1-2 Common** veya **1 Uncommon**
  seviye alım yapabileceği bir bütçeye denk gelir — her gün somut ama
  yavaş bir deste büyümesi hissi hedefleniyor.
- Hedef aralık ROADMAP.md'deki ilkeyle uyumlu: ne çok cömert (deste
  hemen doyar, kararlar anlamsızlaşır) ne çok cimri (oyuncu asla
  yükseltme yapamaz).

## Açık Sorular (Faz 3 Denge Turunda Netleşecek)

- Kullanılmayan altın gün sonunda taşınıyor mu, yoksa sıfırlanıyor mu?
  (Öneri: taşınır — biriktirme stratejisi de geçerli bir oyun tarzı
  olmalı.)
- Muhtar özel mekaniği ("kötü fala inanmaz" — negatif kombolar ceza,
  ROADMAP.md Faz 2) ekonomiye nasıl yansır? Şimdilik sadece eşik
  çarpanı (×1.5) ile temsil ediliyor, özel mekanik Faz 2'de eklenecek.
- Rewarded ad "ikinci şans" (ROADMAP.md Faz 4) ekonomiyi nasıl
  etkiler? Muhtemelen kaybedilen günde bir kerelik eşik affı — bu
  taslağın kapsamı dışında, Faz 4'te tasarlanacak.
