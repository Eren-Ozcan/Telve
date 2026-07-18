# 🔮 Telve — Yol Haritası

> Türk kahvesi falı temalı, deste-kurma roguelike. Balatro'nun "tek deste, sonsuz kombo"
> döngüsü + fal kültürünün anlatı gücü. Hedef platform: mobil (iOS/Android), motor: Unity.

---

## Faz 0 — Tasarım Temeli (1-2 hafta)

Kod yazmadan önce kağıt üzerinde kanıtlanması gerekenler:

- [ ] **Çekirdek döngü dokümanı**: Fincan çevir → 5-7 sembol → okuma sırasına diz → kombo → puan/ödeme. Tek sayfa, tartışmasız net.
- [ ] **Sembol seti v1 (~20-25 sembol)**: Her sembolün adı, temel değeri, nadirlik sınıfı, fal anlamı. (kuş, yol, yılan, dağ, göz, gemi, balık, ağaç, kalp, mektup...)
- [ ] **Kombo matrisi v1**: Yan yana gelen ikili/üçlü kombinasyonlar ve etkileri (örn. Yol+Kuş = "Haber Geliyor" ×1.5 çarpan). MVP için ~30-40 kombo yeterli.
- [ ] **Puanlama formülü**: Balatro'daki "çip × çarpan" muadili. Öneri: `Sembol Değeri × Kombo Çarpanı × Tılsım Etkileri = Müşteri Memnuniyeti → Ödeme`.
- [ ] **Kağıt prototip**: Kartlarla masada oyna. Kombo dizme kararı gerçekten ilginç mi? Değilse burada düzelt, kodda değil.
- [ ] **Ekonomi taslağı**: Müşteri ödemesi → pazar fiyatları → gün sonu hedefi (boss "muhtar" eşiği) dengesi.

**Çıkış kriteri:** Kağıt prototipte 3 farklı kişi "bir el daha" dediyse geç.

---

## Faz 1 — Dijital Prototip / "Bulmaca Kanıtı" (3-4 hafta)

Amaç güzellik değil, **eğlencenin kodda da çalıştığını** kanıtlamak. Placeholder görseller (düz renkli daireler + sembol adı yazısı) yeterli.

- [ ] Unity proje kurulumu (2D URP, portre mod, 1080×1920 referans çözünürlük)
- [ ] Veri mimarisi: Semboller ve kombolar **ScriptableObject** olarak (dengeleme kod değişikliği gerektirmesin)
- [ ] Fincan çevirme → rastgele sembol dağıtımı (seed destekli RNG baştan kur — haftalık seed buna dayanacak)
- [ ] Sürükle-bırak okuma sırası dizme (mobilde parmakla test et, masaüstü fare değil)
- [ ] Kombo tespiti + puan hesaplama + sonuç ekranı
- [ ] Tek müşteri döngüsü: gel → fal bak → öde → git
- [ ] 8-10 müşterilik gün döngüsü + basit muhtar (yüksek eşikli müşteri)
- [ ] Pazar ekranı v0: müşteriler arası 3 seçenekten sembol/tılsım satın alma
- [ ] ~10 tılsım (pasif etki: "kuşlar +2 değer", "ilk kombo ×2" vb.)

**Çıkış kriteri:** Kendi başına 30 dk kesintisiz oynayabiliyorsan ve "şu tılsımla şu desteyi denesem" diye düşünüyorsan geç. Düşünmüyorsan Faz 0'a dön.

---

## Faz 2 — Dikey Dilim / "Hissiyat" (4-6 hafta)

Amaç: MVP içeriğinin tamamı + oyunun **duygusunu** kuran sunum katmanı. Balatro'yu satan şey puan sayacının tıkırtısıydı; Telve'yi satacak şey fincanın atmosferi.

### Sunum
- [ ] Sanat yönü kilidi: loş masa, buhar, mum ışığı, telve dokusu (2-3 konsept dene, birini seç)
- [ ] Fincan çevirme animasyonu (oyunun imza anı — buna zaman harca)
- [ ] Sembollerin telvede "belirme" efekti
- [ ] Kombo tetiklenme geri bildirimi: isim kartı ("Kıskançlık Fark Edildi"), ekran sarsıntısı, ses
- [ ] Müşteri tepki sistemi: irkilme, sevinme, korkma (basit portre + 2-3 ifade)
- [ ] Ses: fincan porselen tıkırtısı, kaşık, ortam uğultusu, kombo müzik vuruşları

### İçerik tamamlama (MVP kapsamı)
- [ ] 20-25 sembolün tamamı final görselle
- [ ] ~10 tılsım final
- [ ] Muhtar boss'u özel mekanikle (örn. "kötü fala inanmaz" — negatif kombolar ceza)
- [ ] Müşteri çeşitliliği: 4-5 arketip (aceleci, kuşkucu, dertli...) hafif kural farklarıyla

### Falcı defteri v1
- [ ] Keşfedilen komboların kaydı; ilk keşifte altın çerçeve anı (viral ekran görüntüsü anı — paylaş butonu buraya)

**Çıkış kriteri:** 10 dakikalık oynanış videosu çekilebilir ve yabancı biri izlediğinde ne olduğunu anlıyor.

---

## Faz 3 — Meta-İlerleme + Koşu Derinliği (4-5 hafta)

- [ ] **Bilgelik puanı**: koşu sonu kazanım + kalıcı açılımlar ağacı (StS tarzı: yeni semboller, başlangıç tılsımı seçenekleri)
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
