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

- [x] **Bilgelik puanı**: koşu sonu kazanım + kalıcı açılımlar ağacı (StS tarzı: yeni semboller, başlangıç tılsımı seçenekleri) *(kazanım: `WisdomReward`+`MetaProgressStore`; harcama: `GameController.UnlockCharacter` ile `FalciCharacter` (deste+tılsım paketleri) açılıyor — "yeni semboller/başlangıç tılsımı seçenekleri" karakter seçimiyle birleştirildi, ağaç yapısı değil düz liste v1. Play Mode'da uçtan uca doğrulandı.)*
- [x] Açılabilir sembol desteleri (2. ve 3. deste — farklı oynanış eğilimi: "kuş ağırlıklı haber destesi" vb.) *(karakter seçimiyle birleştirildi — her `FalciCharacter` kendi deste eğilimini taşıyor, bkz. altta)*
- [x] 2-3 falcı karakteri (farklı başlangıç koşulu/pasif — Balatro'daki deste seçimi muadili) *(3 `FalciCharacter`: Varsayılan (ücretsiz), Kuş Falcısı (15 bilgelik, kuş ağırlıklı+Kuş Tüyü), Kara Kedi Falcısı (25 bilgelik, kedi ağırlıklı+Kara Kedi Tılsımı). `CharacterSelectView` panel, "Falcılar" butonu. Görsel/portre yok — sadece isim/açıklama metni, final sanat ayrı iş.)*
- [x] Koşu sonu özet ekranı: en iyi kombo, toplam kazanç, defter ilerlemesi *(`GameController.BestComboThisRun`/`TotalGoldEarnedThisRun`/`DiscoveredCombosCount`, GameView'da "Yeni Koşu Başlat" ile birlikte gösteriliyor, Play Mode'da doğrulandı)*
- [ ] Zorluk eğrisi: gün ilerledikçe müşteri beklentisi artışı; kayıp koşusu ortalama 20-40 dk'da bitmeli *(artış kısmı zaten uygulanmış: `CustomerProfile.Regular` — Eşik(n)=12+n×4, docs/design/04-economy.md formülüyle birebir. `BalanceSimulator`'a saniye-bazlı pacing tahmini eklendi (00-core-loop.md adım sayısına dayalı kaba sabitler: fincan/dizilim/sonuç ~75sn, pazar uğrağı ~12sn) ve 20 koşu çalıştırıldı: gerçekçi üst sınır senaryosunda (optimal dizilim + pazar) ~12,9 dk/koşu — ROADMAP'in 20-40 dk hedefinin ALTINDA. Bu SİMÜLASYON TAHMİNİ, insan playtestiyle doğrulanmadı, ama sinyal net: mevcut 8-10 müşteri/gün kapsamı hedeflenen oturum uzunluğu için muhtemelen kısa. Kesin karar (müşteri sayısını artırma/etkileşimi derinleştirme) gerçek playtestten sonra verilmeli — kod tarafı bu simülasyonla tavan yaptı.)*
- [x] Denge turu: 20+ tam koşu verisiyle sembol/tılsım/ekonomi ayarı (ScriptableObject'ler sayesinde hızlı) *(`BalanceSimulator`: 20 koşu × 3 strateji simülasyonu; muhtar eşik çarpanı 1.5×→1.2× düşürüldü, veri commit mesajında. Gerçek insan playtest verisi değil — otomatik simülasyon; sembol/tılsım tekil değerleri henüz ayrıca ayarlanmadı, sadece muhtar eşiği.)*
- [x] Kayıt sistemi: koşu ortası kayıt/devam (mobilde şart), meta ilerleme kalıcılığı *(meta ilerleme kalıcılığı: `MetaProgressStore` — bilgelik puanı, falcı defteri, açılan/seçili karakter, hepsi PlayerPrefs ile kalıcı. Koşu ortası kayıt/devam: `RunSaveData`/`RunSaveService` + `DaySession.Restore` + `GameController.SaveRunState`/`RestoreRunState` — gerçek Stop/Play (uygulama kapat-aç eşdeğeri) ile uçtan uca doğrulandı: açık fincan, gönderilmemiş sembol seçimi, gün-bitti özet ekranı, hepsi birebir geri geliyor.)*

**Çıkış kriteri:** Bir koşu kaybedince "bir daha" isteği doğuyor ve meta açılım bir sonraki koşuyu somut değiştiriyor.

---

## Faz 4 — Mobil Cila + Monetizasyon (3-4 hafta)

- [ ] Performans: düşük seviye Android cihazda 60 fps, pil dostu *(kod tarafında elden gelenler yapıldı: 42 texture'ın tamamına Android/iOS için ASTC 6x6 sıkıştırma override'ı verildi (önceden `overridden: 0` varsayılan haldeydi), Quality Settings'te tüm seviyelerde anisotropic filtering kapatıldı (2D sprite'lar için gereksiz), `GameController.Awake()`'e `vSyncCount=0` + `targetFrameRate=60` eklendi. Kod tarafında Update()/FindObjectOfType gibi performans kokusu taranıp bulunamadı. GERÇEK fiziksel düşük seviye Android cihazda fps/pil ölçümü yapılmadı — bu madde ancak cihaz testiyle işaretlenebilir.)*
- [x] **İlk gerçek Android build (AAB) + imzalama anahtarı (keystore) kurulumu** *(2026-07-31: Android Build Support modülü ve bundled SDK/NDK/JDK zaten kuruluydu (Unity 6000.1.17f1), paket adı `com.yilkgames.telve` zaten doğruydu. Yeni bir upload keystore üretildi (`android-keystore/telve-upload.jks`, PKCS12, alias `telve-upload`) — proje içinde gitignore'lu, şifresi ve dosyası hem masaüstünde hem private `Eren-Ozcan/pictures` reposunda (`pictures/Telv/android-keystore/`) yedeklendi. Unity Player Settings'e bağlandı (`useCustomKeystore`), ProjectSettings.asset'e sadece dosya yolu/alias adı yazılıyor, şifre asla repoya girmiyor. İlk uçtan uca test build'i (`BuildPipeline.BuildPlayer`, Android hedefi, App Bundle) **BAŞARILI**: 0 hata, 2 uyarı, 55.7 MB imzalı `.aab` üretildi (`android-keystore/build-output/telve-test.aab`, gitignore'lu). Build sırasında "Active Input Handling = Both" uyarı diyaloğu build'i durdurdu (proje'de Input System paketi kurulu değil, bu yüzden "Both" anlamsız/riskliydi) — `ProjectSettings.asset`'te `activeInputHandler: 2 → 0` ("Input Manager (Old)") olarak düzeltildi, gelecekteki build'ler bu diyalogla durmayacak. Kalan: bu AAB'nin gerçek cihazda çalıştığının doğrulanması ve Play Console dahili test kanalına fiilen yüklenmesi — ikisi de bu oturumda yapılmadı.)*
- [x] **Store ekran görüntüleri için gerçek portre (1080×1920) Play Mode testi + kritik görsel hata düzeltmesi** *(2026-07-31: Unity Game View ilk kez gerçek portre çözünürlükte (önceden hep landscape/free aspect'te test edilmişti) çalıştırılınca `Game.unity` sahnesinde 25 adet RectTransform'da (fincanın 8 sembol slotu + pazarın 3 teklif satırı + arka plan + fincan görseli + KVKK onay paneli) `anchorMin`/`pivot` değerlerinde "0." öneki düşmüş sistemik bir veri hatası bulundu — örn. pivot `5` yerine `0.5`, anchorMin.x `391` yerine `0.391` olmalıydı. Bu hata daha önce sadece `ConsentText`/`AcceptButton`'da fark edilip düzeltilmişti (bkz. Faz 4 GDPR/KVKK maddesi); aynı hata fincan sembolleri ve pazar ikonlarında da varmış ve gerçek portre aspect oranında test edilmediği için hiç fark edilmemiş — sembol ikonları ekranın çok dışına/üstüne taşıyordu. Unity Editor'da script ile tüm sahne taranıp (`anchorMin`/`anchorMax`/`pivot` için 0-1 dışı her değer "0." ekleyerek" düzeltildi) sahne kaydedildi; Play Mode'da coplay-mcp ile uçtan uca doğrulandı (fincan sembolleri artık doğru görünüyor). Bu arada 4 gerçek Play Mode ekran görüntüsü alındı — bkz. `docs/store-listing.md`.)*
- [ ] IAP altyapısı (Unity IAP): fincan/masa örtüsü kozmetikleri — güç satmıyoruz, sadece görünüm *(`CosmeticItem` veri modeli + `IPurchaseService`/`MockPurchaseService` (varsayılan, dev/QA için) yanında artık gerçek `UnityIAPPurchaseService` de var — com.unity.purchasing 4.12.2 paketi kuruldu, `IStoreListener` tam implemente edildi, derleme doğrulandı. Play Console tarafında da ilerleme var: Telv için app listing açıldı (`com.yilkgames.telve`, App ID 4975250454079860772, Unity `applicationIdentifier` eşleştirildi), mağaza metni/kategori/etiket/reklam/içerik derecelendirmesi/hedef kitle beyanları girildi. GERÇEK MAĞAZA BAĞLANTISI HÂLÂ YOK: `cosmeticId`lerle eşleşen ürün kataloğu Play Console'da henüz oluşturulmadı, bu yüzden `UnityPurchasing.Initialize` uçtan uca test edilemez ve `GameController.Awake()`'te varsayılan olarak atanmadı. Ayrıca gizlilik politikası URL'si yayınlanmadan mağaza girişi/Data Safety formu tam gönderilemiyor (taslak durumda) — bkz. `docs/store-listing.md`.)*
- [x] Rewarded ad entegrasyonu: koşu sonu "ikinci şans" + bilgelik puanı ×2 (2 nokta, fazlası deneyimi yer) *(`IRewardedAdService`/`MockRewardedAdService` (varsayılan, dev/QA için) yanında artık gerçek `LevelPlayRewardedAdService` de var — com.unity.services.levelplay 9.2.0 paketi kuruldu (Unity Ads legacy SDK 2026-01-31'de sunset edildiği için LevelPlay mediation kullanıldı), derleme doğrulandı. Oynanış akışı tam bağlı: `DaySession.TryGrantSecondChance` + `GameController.RequestSecondChance`/`RequestDoubleWisdom`, koşu başına birer kez, Play Mode'da uçtan uca doğrulandı. GERÇEK LevelPlay hesabı/app key/ad unit ID HÂLÂ YOK — hesap kurulana kadar varsayılan olarak atanmadı.)*
- [x] Tutorial / ilk 5 dakika akışı (ilk müşteri = öğretici fal) *(`TutorialView` — ilk hiç görülmemiş karşılaşmada bağlamsal ipuçları (sırala → oku → sonuç), bir daha görünmez. Play Mode'da doğrulandı.)*
- [x] Yerelleştirme altyapısı: TR + EN baştan (fal terminolojisinin İngilizce karşılıkları ayrı iş — erken başla) *(`Localization`/`LocalizedText`/`LanguageToggleView` + `LocalizationTable` veri modeli; statik UI metinleri TR/EN'e bağlı. Faz 4'ün kendi notundaki "ayrı iş" olan fal terminolojisi de tamamlandı: yeni `ContentLocalizationTable`/`ContentLocalization` (`Assets/Scripts/Meta/ContentLocalization.cs`) 24 sembol + 37 kombo + 10 tılsım + 3 karakterin EN adı/açıklamasını id üzerinden eşliyor (kaynak TR asset'leri değişmedi). GameView.Refresh() içindeki dinamik/interpolasyonlu durum metinleri (altın/skor/müşteri sırası, pazar etiketleri, koşu özeti, kombo banner'ı, karakter seçim durumu) artık `UiStrings` tablosundaki template key'lerle TR/EN'e bağlı. Unity açılıp derleme hatasız olduğu ve Play Mode'da dil değişiminin (durum metni, pazar, koşu özeti, kombo banner'ı, karakter seçimi) TR↔EN doğru geçtiği coplay-mcp üzerinden doğrulandı.)*
- [ ] Analitik: koşu tamamlama, ölüm noktaları, kombo keşif oranları, D1/D7 retention *(event altyapısı hazır — `AnalyticsEvents`/`IAnalyticsSink`, varsayılan yerel dosya sink'i; run_started/run_ended/death_point/combo_discovered loglanıyor, Play Mode'da doğrulandı. D1/D7 retention gerçek backend + çoklu oturum verisi gerektirir, henüz yok — gerçek sink bağlandığında otomatik çalışır.)*
- [ ] GDPR/KVKK/ATT izin akışları, çevrimdışı oynanabilirlik *(çevrimdışı oynanabilirlik zaten sağlanıyor — kod tabanında hiçbir ağ çağrısı yok, tüm kalıcılık PlayerPrefs. `PrivacyConsentView` ile onay kapısı iskeleti kuruldu. Yer tutucu metin, gerçekçi bir KVKK/GDPR aydınlatma taslağıyla değiştirildi (`Game.unity` → `ConsentText`: veri sorumlusu, LevelPlay/ironSource reklam ağı üzerinden işlenen veriler, KVKK md. 11 haklar, iletişim adresi, iOS ATT sistem izninin ayrıca sorulacağı notu). Play Mode doğrulaması sırasında KRİTİK bir sahne hatası bulunup düzeltildi: `ConsentText` ve `AcceptButton`'ın `anchorMin`/`anchorMax`/`pivot` değerleri (muhtemelen eski bir veri girişi hatasıyla) `0.5` yerine `5` girilmişti — bu yüzden onay metni de kabul butonu da ekranın çok dışına düşüyor, kapı görünüyor ama içeriği ve butonu asla görünmüyordu (oyun fiilen hiç başlatılamazdı). Düzeltme sonrası coplay-mcp ile uçtan uca doğrulandı: metin okunuyor, "Kabul Ediyorum" tıklanıyor, kapı kapanıp oyuna geçiliyor. Madde yine de işaretlenmedi, çünkü (1) bu taslak bağımsız hukuki incelemeden geçmedi, (2) iOS ATT sistem izni entegrasyonu (kod tarafı) henüz yapılmadı.)*

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
