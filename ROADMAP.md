# 🔮 Telve — Yol Haritası

> Türk kahvesi falı temalı, deste-kurma roguelike. Balatro'nun "tek deste, sonsuz kombo"
> döngüsü + fal kültürünün anlatı gücü. Hedef platform: mobil (iOS/Android), motor: Unity.

---

## Faz 0 — Tasarım Temeli (1-2 hafta)

Kod yazmadan önce kağıt üzerinde kanıtlanması gerekenler:

- [x] **Çekirdek döngü dokümanı**: Fincan çevir → 5-7 sembol → okuma sırasına diz → kombo → puan/ödeme. Tek sayfa, tartışmasız net. *(kod olarak CupDraw → ReadingOrder → ComboDetector → ScoreCalculator zincirinde uygulanmış durumda)*
- [x] **Sembol seti v1 (~20-25 sembol)**: Her sembolün adı, temel değeri, nadirlik sınıfı, fal anlamı. *(24 sembol `Assets/Resources/Data/Symbols` altında ScriptableObject olarak var)*
- [x] **Kombo matrisi v1**: Yan yana gelen ikili/üçlü kombinasyonlar ve etkileri. *(37 kombo `Assets/Resources/Data/Combos` altında)*
- [x] **Puanlama formülü**: Balatro'daki "çip × çarpan" muadili. *(ScoreCalculator + testleri)*
- [ ] **Kağıt prototip**: Kartlarla masada oyna. Kombo dizme kararı gerçekten ilginç mi? Değilse burada düzelt, kodda değil. *(gerçek insan playtest kanıtı repoda yok — bu satır sadece fiilen oynatıldığında işaretlenmeli)*
- [x] **Ekonomi taslağı**: Müşteri ödemesi → pazar fiyatları → gün sonu hedefi (boss "muhtar" eşiği) dengesi. *(CustomerEconomy, MarketPricing, DaySession)*

**Çıkış kriteri:** Kağıt prototipte 3 farklı kişi "bir el daha" dediyse geç.

---

## Faz 1 — Dijital Prototip / "Bulmaca Kanıtı" (3-4 hafta)

Amaç güzellik değil, **eğlencenin kodda da çalıştığını** kanıtlamak. Placeholder görseller (düz renkli daireler + sembol adı yazısı) yeterli.

- [x] Unity proje kurulumu (2D URP, portre mod, 1080×1920 referans çözünürlük)
- [x] Veri mimarisi: Semboller ve kombolar **ScriptableObject** olarak (dengeleme kod değişikliği gerektirmesin)
- [x] Fincan çevirme → rastgele sembol dağıtımı (seed destekli RNG baştan kur — haftalık seed buna dayanacak)
- [x] Sürükle-bırak okuma sırası dizme (mobilde parmakla test et, masaüstü fare değil) *(ReadingOrderChip drag/drop; masaüstünde fare ile doğrulandı, cihazda parmak testi hâlâ gerekiyor)*
- [x] Kombo tespiti + puan hesaplama + sonuç ekranı
- [x] Tek müşteri döngüsü: gel → fal bak → öde → git
- [x] 8-10 müşterilik gün döngüsü + basit muhtar (yüksek eşikli müşteri)
- [x] Pazar ekranı v0: müşteriler arası 3 seçenekten sembol/tılsım satın alma
- [x] ~10 tılsım (pasif etki: "kuşlar +2 değer", "ilk kombo ×2" vb.) *(10 tılsım `Assets/Resources/Data/Charms` altında)*

**Çıkış kriteri:** Kendi başına 30 dk kesintisiz oynayabiliyorsan ve "şu tılsımla şu desteyi denesem" diye düşünüyorsan geç. Düşünmüyorsan Faz 0'a dön.

---

## Faz 2 — Dikey Dilim / "Hissiyat" (4-6 hafta)

Amaç: MVP içeriğinin tamamı + oyunun **duygusunu** kuran sunum katmanı. Balatro'yu satan şey puan sayacının tıkırtısıydı; Telve'yi satacak şey fincanın atmosferi.

### Sunum
- [x] Sanat yönü kilidi: loş masa, buhar, mum ışığı, telve dokusu (2-3 konsept dene, birini seç) *(3 konsept üretildi — `Assets/Art/Concepts/`; "B - düz altın çizgi, gece mavisi/altın palet, vektörel folk-art" seçildi — ikon üretimi için en okunaklı seçenek)*
- [x] Masa/buhar/mum arka planı + boş fincan illüstrasyonu final görselle sahneye eklendi *(`Assets/Art/Background/table_background.png` → `Canvas/Background` tam ekran; `cup_art.png` → `Canvas/CupPanel/CupArt` dekoratif üst görsel; Play Mode'da doğrulandı)*
- [x] Fincan çevirme animasyonu (oyunun imza anı — buna zaman harca) *(GameView.PunchScale — placeholder kalitede coroutine tween; final "imza an" cilası sanat yönü kilidinden sonra)*
- [x] Sembollerin telvede "belirme" efekti *(GameView.RevealSlot)*
- [x] Kombo tetiklenme geri bildirimi: isim kartı ("Kıskançlık Fark Edildi"), ekran sarsıntısı, ses *(ComboBannerView + AudioManager.PlayComboHit; combo_hit.wav üretilip atandı)*
- [x] Müşteri tepki sistemi: irkilme, sevinme, korkma (basit portre + 2-3 ifade) *(CustomerReactionView; irkilme/korkma "startled" sprite'ında birleşti — v1 kapsamı; 3 portre Concept B stilinde üretilip `Assets/Art/Portraits/` altına kaydedildi ve sahnedeki alanlara atandı)*
- [x] Ses: fincan porselen tıkırtısı, kaşık, ortam uğultusu, kombo müzik vuruşları *(AudioManager; 6 klip `Assets/Audio/` altında üretilip sahnedeki alanlara atandı — cup_draw, combo_hit, purchase, positive/negative_result, ambient_loop)*

### İçerik tamamlama (MVP kapsamı)
- [x] 20-25 sembolün tamamı final görselle *(24 sembol ikonu Concept B stilinde üretildi — `Assets/Art/Symbols/`, `SymbolData.sprite` alanlarına atandı; GameView'a `cupSlotIcons`/`marketOfferIcons` eklenip fincan ve pazar slotlarında fiilen gösteriliyor — Play Mode'da doğrulandı.)*
- [x] ~10 tılsım final *(10 tılsım ikonu üretildi — `Assets/Art/Charms/`, `CharmData.icon` alanlarına atandı, pazar UI'ında görüntüleniyor.)*
- [x] Muhtar boss'u özel mekanikle (örn. "kötü fala inanmaz" — negatif kombolar ceza)
- [x] Müşteri çeşitliliği: 4-5 arketip (aceleci, kuşkucu, dertli...) hafif kural farklarıyla *(Regular + Aceleci/Kuşkucu/Dertli/Cömert = 5 arketip)*

### Falcı defteri v1
- [x] Keşfedilen komboların kaydı; ilk keşifte altın çerçeve anı (viral ekran görüntüsü anı — paylaş butonu buraya) *(ComboJournal + JournalView; altın çerçeve/paylaş butonu görsel cilası final sanata bağlı)*

**Çıkış kriteri:** 10 dakikalık oynanış videosu çekilebilir ve yabancı biri izlediğinde ne olduğunu anlıyor.

---

## Faz 3 — Meta-İlerleme + Koşu Derinliği (4-5 hafta)

- [ ] **Bilgelik puanı**: koşu sonu kazanım + kalıcı açılımlar ağacı (StS tarzı: yeni semboller, başlangıç tılsımı seçenekleri) *(koşu sonu kazanım kısmı yapıldı: `WisdomReward` + `MetaProgressStore`, PlayerPrefs ile kalıcı, testli; kalıcı açılımlar ağacı — puanı harcama tarafı — henüz yok)*
- [ ] Açılabilir sembol desteleri (2. ve 3. deste — farklı oynanış eğilimi: "kuş ağırlıklı haber destesi" vb.)
- [ ] 2-3 falcı karakteri (farklı başlangıç koşulu/pasif — Balatro'daki deste seçimi muadili)
- [ ] Koşu sonu özet ekranı: en iyi kombo, toplam kazanç, defter ilerlemesi
- [ ] Zorluk eğrisi: gün ilerledikçe müşteri beklentisi artışı; kayıp koşusu ortalama 20-40 dk'da bitmeli
- [ ] Denge turu: 20+ tam koşu verisiyle sembol/tılsım/ekonomi ayarı (ScriptableObject'ler sayesinde hızlı)
- [ ] Kayıt sistemi: koşu ortası kayıt/devam (mobilde şart), meta ilerleme kalıcılığı

**Çıkış kriteri:** Bir koşu kaybedince "bir daha" isteği doğuyor ve meta açılım bir sonraki koşuyu somut değiştiriyor.

---

## Faz 4 — Mobil Cila + Monetizasyon (3-4 hafta)

- [ ] Performans: düşük seviye Android cihazda 60 fps, pil dostu
- [ ] IAP altyapısı (Unity IAP): fincan/masa örtüsü kozmetikleri — güç satmıyoruz, sadece görünüm
- [ ] Rewarded ad entegrasyonu: koşu sonu "ikinci şans" + bilgelik puanı ×2 (2 nokta, fazlası deneyimi yer)
- [ ] Tutorial / ilk 5 dakika akışı (ilk müşteri = öğretici fal)
- [ ] Yerelleştirme altyapısı: TR + EN baştan (fal terminolojisinin İngilizce karşılıkları ayrı iş — erken başla)
- [ ] Analitik: koşu tamamlama, ölüm noktaları, kombo keşif oranları, D1/D7 retention
- [ ] GDPR/KVKK/ATT izin akışları, çevrimdışı oynanabilirlik

**Çıkış kriteri:** Tanımadık 10 kişi telefonda tutorial'sız yardım almadan bir gün döngüsünü bitirebiliyor.

---

## Faz 5 — Soft Launch (4-6 hafta)

- [ ] Kapalı beta (TestFlight / Play Internal): 50-100 oyuncu, geri bildirim formu
- [ ] Tek pazar soft launch (öneri: Türkiye — tema yerli, geri bildirim kalitesi yüksek)
- [ ] Metrik hedefleri: D1 ≥ %35, D7 ≥ %12, ort. seans ≥ 15 dk (roguelike için makul eşikler)
- [ ] Denge ve ekonomi yaması turları (haftalık)
- [ ] Store sayfası: ekran görüntüleri, 30 sn fragman (fincan çevirme + altın çerçeve keşif anı)
- [ ] İçerik üreticisi tohumlama: fal/kahve temalı TikTok-Instagram mikro-influencer'ları, "falına ne çıktı" formatı

**Çıkış kriteri:** Retention hedefleri tutuyor VEYA tutmama nedeni teşhis edilip düzeltildi.

---

## Faz 6 — Global Lansman + Canlı Servis (sürekli)

- [ ] Global çıkış (iOS + Android eşzamanlı)
- [ ] **Haftalık tohum (seed) + lider tablosu** (lansman haftasında hazır — topluluk ritüeli bu)
- [ ] Sezonluk deste paketleri (çeşitlilik satar, güç satmaz): sezon 1 teması hazır
- [ ] Aylık denge + içerik yaması ritmi: yeni semboller, yeni tılsımlar, yeni müşteri arketipleri
- [ ] Yol haritası v2: yeni boss'lar (kaynana? kahveci çırağı?), günlük meydan okuma, PC/Steam portu değerlendirmesi

---

## Riskler ve Erken Cevaplar

| Risk | Azaltma |
|---|---|
| Kombo dizme kararı sığ kalır ("her zaman aynı sıra en iyi") | Faz 0 kağıt prototipte yakala; sıra bağımlılığını derinleştir (komşuluk + pozisyon bonusları) |
| Fal teması yurtdışında anlaşılmaz | EN yerelleştirmede "tasseography" bilinirliğine yaslan; tutorial temayı 30 sn'de öğretsin |
| Balatro dalgası söner | Tema özgünlüğü asıl sigorta; hız için MVP kapsamını büyütme |
| Tek geliştirici tükenmişliği | Fazların çıkış kriterlerine sadık kal; kapsam sürünmesine karşı "v2 listesi" tut |
| Denge cehennemi | Tüm sayılar ScriptableObject'te; analitikle kombo kullanım oranlarını izle |

---

## Kaba Zaman Çizelgesi

Tek geliştirici, yarı zamanlı-üstü tempo varsayımıyla:

| Faz | Süre | Kümülatif |
|---|---|---|
| 0 — Tasarım temeli | 1-2 hafta | ~2 hafta |
| 1 — Dijital prototip | 3-4 hafta | ~6 hafta |
| 2 — Dikey dilim | 4-6 hafta | ~3 ay |
| 3 — Meta-ilerleme | 4-5 hafta | ~4 ay |
| 4 — Mobil cila + para | 3-4 hafta | ~5 ay |
| 5 — Soft launch | 4-6 hafta | ~6-6.5 ay |
| 6 — Global lansman | — | ~7. ay |

> En kritik kural: **Faz 1 bitmeden Faz 2 sanatına para/zaman harcama.**
> Bulmaca eğlenceli değilse atmosfer onu kurtarmaz; eğlenceliyse atmosfer onu efsaneleştirir.
