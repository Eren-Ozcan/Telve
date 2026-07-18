# Çekirdek Döngü

> Faz 0 çıkış maddesi: "Tek sayfa, tartışmasız net."

## Bir Müşteri Turu (Tek El)

```
1. FİNCAN ÇEVİR
   Müşteri kahvesini içer, fincanı tabağa ters kapatır, bekler, çevirir.
   → Telvede 5-7 sembol rastgele belirir (destedeki sembol havuzundan,
     nadirlik ağırlıklı çekiliş, seed'li RNG).

2. OKUMA SIRASINA DİZ
   Oyuncu belirlenen sembolleri sürükleyip bir okuma sırasına dizer
   (soldan sağa, fincanın ağzından dibine doğru okunur — fal geleneği).
   Bu, Balatro'daki "eldeki kartları seçme" kararının muadili: hangi
   sembolü nereye koyarsan hangi komboyu tetiklersin.

3. KOMBO TESPİTİ
   Dizilimdeki komşu sembol çiftleri/üçlüleri önceden tanımlı kombo
   matrisine bakılarak taranır (bkz. 02-combos.md). Her kombo bir isim
   kartı ve bir etki (sabit bonus veya çarpan) tetikler.

4. PUAN HESAPLA
   Taban puan (sembol değerleri toplamı) → kombo etkileri → aktif
   tılsım etkileri sırasıyla uygulanır → Müşteri Memnuniyeti puanı
   çıkar (bkz. 03-scoring.md).

5. ÖDEME / TEPKİ
   Memnuniyet puanı o müşterinin eşiğiyle karşılaştırılır. Eşik
   aşılırsa tam+bonus ödeme ve olumlu tepki; aşılmazsa düşük ödeme ve
   olumsuz tepki (bkz. 04-economy.md). Falcı defterine yeni keşfedilen
   kombolar altın çerçeveyle kaydedilir.

6. SIRADAKİ MÜŞTERİ
   Müşteriler arası pazara uğranabilir: kazanılan altınla yeni sembol
   veya tılsım satın alınıp deste güçlendirilir (bkz. 04-economy.md).
```

## Gün Döngüsü

```
Müşteri 1 → [pazar] → Müşteri 2 → [pazar] → ... → Müşteri 8
→ [pazar] → Muhtar (gün sonu boss, yüksek eşikli özel müşteri)
→ Gün Sonu Özeti → Ertesi Gün
```

- Bir günde 8-10 sıradan müşteri + 1 muhtar vardır.
- Zorluk gün içinde artar: müşteri eşikleri sıraya göre yükselir.
- Muhtar geçilemezse gün kaybedilir (roguelike ölüm koşulu — koşu biter).

## Neden Bu Döngü Balatro Analojisiyle Çalışır

| Balatro | Telve |
|---|---|
| Kart eli çek | Fincan çevir → sembol çık |
| Poker eli seç (hangi kartlar oynanır) | Okuma sırasına dizme (hangi sembol nereye) |
| Poker eli türü (flush, straight...) | Kombo (Yol+Kuş = "Haber Geliyor") |
| Çip × Çarpan | Sembol Değeri × Kombo Çarpanı × Tılsım |
| Blind eşiği | Müşteri Memnuniyeti eşiği |
| Joker kartları | Tılsımlar |
| Mağaza | Pazar |
| Boss blind | Muhtar |

Fark: Balatro'da el *seçimi* önemliyken Telve'de el *sırası* (dizilim)
önemli — komşuluk bazlı kombo sistemi, sıralama kararını oyunun
merkezine koyar. Bu, Faz 0 kağıt prototipinde en kritik test edilecek
varsayımdır (bkz. ROADMAP.md Faz 0 çıkış kriteri).

## Açık Sorular (Kağıt Prototipte Cevaplanacak)

- Sürükleme sırasında zaman baskısı olmalı mı, yoksa serbest düşünme
  süresi mi? (Öneri: Faz 1'de serbest, ileride zorluk modu olarak
  süre eklenebilir.)
- Sembol sayısı sabit mi (her zaman 6) yoksa değişken mi (5-7)? (Öneri:
  değişken — tılsımlarla etkileşim alanı açar, bkz. "Kahve Telvesi
  Yoğun" tılsımı.)
- Dizilimde kullanılmayan (telvede kalan ama sıraya konmayan) sembol
  olabilir mi? (Öneri: hayır, MVP'de çekilen tüm semboller dizilmek
  zorunda — basitlik için.)
