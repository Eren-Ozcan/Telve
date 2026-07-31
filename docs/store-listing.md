# Google Play Store Listing

Play Console'a girilecek metinler. Karakter sınırlarına dikkat: başlık 30,
kısa açıklama 80, uzun açıklama 4000. Play Console'daki "Varsayılan dil"
İngilizce (en-US) seçildi (2026-07-31), bu yüzden ana metin İngilizce.
Türkiye soft launch'ı (ROADMAP.md Faz 5) öncesi Türkçe çeviri ayrı bir
dil girişi olarak eklenmeli — oyun içi TR/EN yerelleştirme zaten var
(`ContentLocalization`), metin çevirisi oradan uyarlanabilir.

## App name (30 characters)

```
Telve
```

## Short description (80 characters)

```
Turkish coffee fortune roguelike — flip the cup, read the symbols, build luck.
```

## Full description

```
Flip the cup. Read the grounds. Build your fortune.

Telve is a roguelike deckbuilder built around tasseography — the old art of
reading fortunes in Turkish coffee grounds. Draw symbols from your cup,
arrange them into a reading order, and watch adjacent symbols trigger
combos for bigger payouts. One cup, endless combinations.

FEATURES

☕ Authentic tasseography theme — 24 symbols drawn from real Turkish coffee
fortune-telling tradition, each with its own meaning

🔮 37 combos to discover — some multiply your score, some are deliberately
risky, and figuring out which symbols to keep apart is half the game

✨ 10 passive charms — shape your reading strategy across a run, no two
decks play quite the same

👥 Five customer archetypes — the hasty, the skeptical, the troubled, the
generous, and the regular customer, each nudging your strategy differently

👑 The Headman — the toughest customer of your day, unimpressed by weak
fortunes

📖 Fortune-teller's Journal — every combo you discover gets recorded, with
a golden frame for your first find

🌟 Wisdom points & unlockable fortune-tellers — three fortune-tellers with
different starting decks and passives, unlocked with points earned from
past runs

🌙 Atmosphere first — candlelight, steam, porcelain clinks, and a table
that feels like it's waiting for you

🌐 English & Turkish — full localization, switch anytime

🔌 No account required — play offline, your progress stays on your device

Simple to learn, hard to put down. Every cup is a new fortune — what will
yours say?
```

## Category

Games > Card (Play Console'daki tam adı "Kağıt")

## Tags

Play Console'da yalnızca önceden tanımlı bir etiket listesinden seçim yapılabiliyor
(serbest metin değil) — "tarot", "kahve", "fal" gibi tam eşleşme yok. Seçilenler:
**Bulmaca, Gündelik, Roguelike oyunlar**.

## Grafik gereksinimleri

- [ ] Uygulama simgesi 512×512 PNG — henüz üretilmedi
- [ ] Öne çıkan görsel (feature graphic) 1024×500 PNG (24-bit, alfasız) —
  henüz üretilmedi
- [ ] Ekran görüntüleri (en az 2, önerilen 4-8) — henüz alınmadı, oyun
  Play Mode'da çalışır durumda (bkz. ROADMAP.md Faz 2), gerçek cihaz/
  Editor ekran görüntüsü alınabilir
- CLAUDE.md kuralına göre üretilecek görseller bu public repoya değil,
  `docs/store-assets-originals/` (gitignore'lu) + private `Eren-Ozcan/pictures`
  reposuna (`pictures/Telv/`) kaydedilecek

## Reklam / Data Safety notları (Play Console) — 2026-07-31 itibarıyla

- Uygulama LevelPlay (ironSource) mediation ile ödüllü reklam gösteriyor
  (`LevelPlayRewardedAdService` — koşu sonu "ikinci şans" ve "bilgelik ×2").
  Gerçek LevelPlay hesabı/app key henüz yok (ROADMAP.md Faz 4).
- **App content → Ads**: "Uygulamam reklam içeriyor" = Evet olarak
  işaretlendi ve kaydedildi. ✅
- **Reklam Kimliği (Ad ID) beyanı**: "Evet, kullanıyor" + amaç =
  "Reklam veya pazarlama" olarak işaretlendi ve kaydedildi. ✅
- **Data safety formu**: Cihaz veya diğer kimlikler (reklam kimliği),
  toplandı + paylaşıldı, amaç = Reklam veya pazarlama, kısa süreli
  işlenmiyor, zorunlu toplama, aktarım şifreli olarak dolduruldu.
  Gizlilik politikası URL'si eklendikten sonra **tam gönderildi**
  (artık taslak değil). ✅ Konum verisi toplanmıyor, hesap sistemi
  yok ("Uygulamam kullanıcıların hesap oluşturmasına izin vermiyor"
  işaretlendi).
- **Gizlilik politikası**: `https://yilkgames.com/privacy-policy/`
  Play Console'a girildi ve kaydedildi. ✅ Bu, stüdyo geneli tek bir
  sayfa (Reefy, Little Grand Hotel, Çengel Bulmaca, Dleverse, Telve,
  Lightwake, Domina, CosmicRumble hepsi kapsamında) — yeni oyun
  eklendiğinde otomatik güncelleniyor, ayrıca Telv'e özel bir sayfa
  gerekmiyor.
- **İçerik derecelendirmesi anketi**: tamamlandı ve gönderildi. ✅
  Tüm otoritelerde en düşük/Genel kategori çıktı (AG, L, E, 3, 0, 3+, 3
  — ESRB/PEGI/USK/ClassInd vb.), cengeBulmaca ile aynı profil.
- **Hedef kitle**: 13-15, 16-17, 18 yaş ve üstü olarak ayarlandı ve
  kaydedildi. ✅ (cengeBulmaca ile tutarlı — çocuk kategorileri hariç
  tutuldu, reklam/veri politikalarında ek "Çocuklara Yönelik"
  kısıtlamasından kaçınmak için)
- **Resmi kurum uygulamaları**: Hayır, kaydedildi. ✅
- **Finans ile ilgili özellikler**: "Sağlanmıyor" olarak kaydedildi. ✅
- **Sağlık uygulamaları**: "Yok" olarak kaydedildi. ✅
- IAP: kozmetik fincan/masa örtüsü öğeleri (`CosmeticItem`,
  `UnityIAPPurchaseService`) — güç satmıyor, sadece görünüm. Play
  Console ürün kataloğu henüz oluşturulmadı.
- Uygulama kategorisi: Kağıt (Card). Etiketler: Bulmaca, Gündelik,
  Roguelike oyunlar (Play Console'un önceden tanımlı etiket listesinden
  seçildi — "tarot"/"kahve"/"fal" gibi serbest metin etiket yok).
- Mağaza girişi metni (ana dil İngilizce): uygulama adı, kısa açıklama,
  tam açıklama girildi ve **taslak olarak kaydedildi**. ✅ (taslak)

## Kalanlar

- ⏳ Mağaza görselleri (ikon 512×512, feature graphic 1024×500, en az
  2 telefon ekran görüntüsü) — Play Console'da bunlar olmadan mağaza
  girişi "Kaydet" ile tam gönderilemiyor, sadece taslak kalıyor. Prompt
  taslakları: `docs/store-visual-prompts.md`.
- ⏳ Dahili test kanalı — yüklenecek bir Android build (AAB) yok, Unity
  projesi henüz Android için derlenmedi (keystore, Android Build Support
  modülü kurulumu gerekiyor).
- ⏳ IAP ürün kataloğu ve gerçek LevelPlay hesabı.
- ⏳ Türkçe mağaza metni (ikincil dil olarak eklenmeli).
