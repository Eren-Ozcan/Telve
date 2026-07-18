# Tılsım Listesi v1

10 pasif tılsım (Balatro'daki Joker kartlarının muadili). Her tılsım
otomatik ve pasif etki yapar, oyuncu elle tetiklemez.

| Tılsım | Nadirlik | Fiyat | Etki |
|---|---|---|---|
| Kuş Tüyü | Common | 12 | Kuş sembollerinin değeri +2 |
| Şanslı Nazar | Common | 12 | Göz sembolü içeren negatif kombolar etkisiz olur |
| Sadık Dost | Common | 12 | Aynı sembol iki kez yan yana gelirse +5 sabit bonus |
| Ekonomik Fal | Common | 12 | Pazar fiyatları %15 düşer |
| İlk Kombo Çarpanı | Uncommon | 22 | Dizilimde tetiklenen ilk kombonun çarpanı ×2 olur |
| Kahve Telvesi Yoğun | Uncommon | 22 | Fincanda 1 sembol daha fazla çıkar (6-8 arası) |
| Kara Kedi Tılsımı | Uncommon | 22 | Negatif kombo cezaları %50 azalır (örn. ×0.6 → ×0.8) |
| Şans Tekerleği | Rare | 38 | Rare ve Epic sembol çekiliş ağırlığı %50 artar |
| Muhtarın Gözdesi | Rare | 38 | Muhtar müşterisinde ödeme ×1.5 |
| Kader Anahtarı | Epic | 65 | Taç sembolü her elde garanti çıkar |

## Etki Hedefi Sınıflandırması (03-scoring.md Adım 5 için)

Puanlama formülündeki 5. adımda ("Tılsım Etkileri Uygulanır"), her
tılsımın hangi aşamaya müdahale ettiği net olmalı:

| Etki Hedefi | Tılsımlar |
|---|---|
| Sembol değeri (taban puana girer) | Kuş Tüyü |
| Sabit bonus (taban puana eklenir) | Sadık Dost |
| Kombo çarpanı (çarpan aşamasında) | İlk Kombo Çarpanı, Kara Kedi Tılsımı |
| Negatif kombo bastırma | Şanslı Nazar |
| Çekiliş / RNG (fincan çevirme aşamasında, puanlamadan önce) | Kahve Telvesi Yoğun, Şans Tekerleği, Kader Anahtarı |
| Ekonomi (pazar/ödeme aşamasında, puanlamadan sonra) | Ekonomik Fal, Muhtarın Gözdesi |

## Tasarım Notları

- Her nadirlik sınıfından en az bir tılsım var; fiyatlar 04-economy.md
  pazar fiyat tablosuyla birebir eşleşiyor.
- "Kara Kedi Tılsımı" ve "Şanslı Nazar" doğrudan 02-combos.md'deki
  kasıtlı negatif kombolara karşı bir denge/karşı-strateji sunuyor —
  oyuncu negatif komboları tamamen dizilim disipliniyle mi yönetecek,
  yoksa tılsım yatırımıyla mı telafi edecek, bu seçim deste kurma
  kimliğinin bir parçası olsun diye tasarlandı.
- Faz 1'de her tılsım bir `CharmData` ScriptableObject'i olacak:
  `id, displayName, rarity, price, effectTarget (enum), effectValue`.
- MVP hedefi ~10 tılsım karşılandı (ROADMAP.md Faz 1). Faz 2'de tılsım
  sayısı arttırılmadan önce mevcut 10'un kağıt/dijital prototipte
  gerçekten "şu tılsımla şu desteyi denesem" hissi yaratıp
  yaratmadığı doğrulanmalı (ROADMAP.md Faz 1 çıkış kriteri).
