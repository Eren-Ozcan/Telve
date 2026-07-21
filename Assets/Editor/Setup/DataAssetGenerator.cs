using System;
using System.IO;
using Telve.Data;
using UnityEditor;
using UnityEngine;

namespace Telve.EditorTools
{
    /// <summary>
    /// Transcribes docs/design/01-symbols.md, 02-combos.md and 05-charms.md
    /// into real SymbolData/ComboData/CharmData assets under
    /// Assets/Resources/Data (Resources so GameController can
    /// Resources.LoadAll them at runtime in a build, not just via
    /// AssetDatabase in the Editor). Run once via -batchmode
    /// -executeMethod; safe to re-run (overwrites existing assets in
    /// place instead of duplicating them).
    /// </summary>
    public static class DataAssetGenerator
    {
        const string SymbolsFolder = "Assets/Resources/Data/Symbols";
        const string CombosFolder = "Assets/Resources/Data/Combos";
        const string CharmsFolder = "Assets/Resources/Data/Charms";
        const string CharactersFolder = "Assets/Resources/Data/Characters";

        struct SymbolRow
        {
            public string id, displayName, falMeaning;
            public int baseValue, drawWeight;
            public SymbolRarity rarity;
        }

        struct ComboRow
        {
            public string id, displayName;
            public string[] symbols;
            public ComboEffectType effectType;
            public float effectValue, secondaryFlatBonus;
            public bool isNegative;
        }

        struct CharmRow
        {
            public string id, displayName, description;
            public SymbolRarity rarity;
            public int price;
            public CharmEffectTarget effectTarget;
            public float effectValue;
        }

        struct CharacterRow
        {
            public string id, displayName, description, startingCharmId;
            public int wisdomCost;
            public string[] bonusSymbolIds;
        }

        public static void Generate()
        {
            EnsureFolder(SymbolsFolder);
            EnsureFolder(CombosFolder);
            EnsureFolder(CharmsFolder);
            EnsureFolder(CharactersFolder);

            foreach (var row in Symbols) CreateSymbol(row);
            foreach (var row in Combos) CreateCombo(row);
            foreach (var row in Charms) CreateCharm(row);
            foreach (var row in Characters) CreateCharacter(row);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"DATA_GEN_DONE:symbols={Symbols.Length},combos={Combos.Length},charms={Charms.Length},characters={Characters.Length}");
            EditorApplication.Exit(0);
        }

        /// <summary>
        /// Generate()'in aksine EditorApplication.Exit çağırmaz — açık,
        /// canlı bir Editor oturumunda (MCP script çalıştırma gibi)
        /// güvenle çağrılabilir.
        /// </summary>
        public static void GenerateCharactersOnly()
        {
            EnsureFolder(CharactersFolder);
            foreach (var row in Characters) CreateCharacter(row);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"DATA_GEN_CHARACTERS_DONE:characters={Characters.Length}");
        }

        static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            var parent = Path.GetDirectoryName(path)?.Replace('\\', '/');
            var leaf = Path.GetFileName(path);
            if (!AssetDatabase.IsValidFolder(parent)) EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, leaf);
        }

        static T LoadOrCreate<T>(string path) where T : ScriptableObject
        {
            var existing = AssetDatabase.LoadAssetAtPath<T>(path);
            if (existing != null) return existing;
            var asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }

        static void CreateSymbol(SymbolRow row)
        {
            var path = $"{SymbolsFolder}/Symbol_{row.id}.asset";
            var asset = LoadOrCreate<SymbolData>(path);
            asset.symbolId = row.id;
            asset.displayName = row.displayName;
            asset.baseValue = row.baseValue;
            asset.rarity = row.rarity;
            asset.drawWeight = row.drawWeight;
            asset.falMeaning = row.falMeaning;
            EditorUtility.SetDirty(asset);
        }

        static void CreateCombo(ComboRow row)
        {
            var path = $"{CombosFolder}/Combo_{row.id}.asset";
            var asset = LoadOrCreate<ComboData>(path);
            asset.comboId = row.id;
            asset.displayName = row.displayName;
            asset.requiredSymbolIds = row.symbols;
            asset.effectType = row.effectType;
            asset.effectValue = row.effectValue;
            asset.secondaryFlatBonus = row.secondaryFlatBonus;
            asset.isNegative = row.isNegative;
            EditorUtility.SetDirty(asset);
        }

        static void CreateCharm(CharmRow row)
        {
            var path = $"{CharmsFolder}/Charm_{row.id}.asset";
            var asset = LoadOrCreate<CharmData>(path);
            asset.charmId = row.id;
            asset.displayName = row.displayName;
            asset.rarity = row.rarity;
            asset.price = row.price;
            asset.effectTarget = row.effectTarget;
            asset.effectValue = row.effectValue;
            asset.description = row.description;
            EditorUtility.SetDirty(asset);
        }

        static void CreateCharacter(CharacterRow row)
        {
            var path = $"{CharactersFolder}/Character_{row.id}.asset";
            var asset = LoadOrCreate<FalciCharacter>(path);
            asset.characterId = row.id;
            asset.displayName = row.displayName;
            asset.description = row.description;
            asset.wisdomCost = row.wisdomCost;
            asset.startingDeckBonusSymbolIds = row.bonusSymbolIds;
            asset.startingCharmId = row.startingCharmId;
            EditorUtility.SetDirty(asset);
        }

        // --- docs/design/01-symbols.md ---
        static readonly SymbolRow[] Symbols =
        {
            new() { id = "yol", displayName = "Yol", baseValue = 3, rarity = SymbolRarity.Common, drawWeight = 100, falMeaning = "Yolculuk, değişim, yeni yön" },
            new() { id = "kus", displayName = "Kuş", baseValue = 3, rarity = SymbolRarity.Common, drawWeight = 100, falMeaning = "Haber, müjde" },
            new() { id = "kalp", displayName = "Kalp", baseValue = 3, rarity = SymbolRarity.Common, drawWeight = 100, falMeaning = "Aşk, duygusal bağ" },
            new() { id = "goz", displayName = "Göz", baseValue = 2, rarity = SymbolRarity.Common, drawWeight = 100, falMeaning = "Nazar, korunma" },
            new() { id = "agac", displayName = "Ağaç", baseValue = 3, rarity = SymbolRarity.Common, drawWeight = 100, falMeaning = "Aile, kök, büyüme" },
            new() { id = "balik", displayName = "Balık", baseValue = 3, rarity = SymbolRarity.Common, drawWeight = 100, falMeaning = "Bereket, para" },
            new() { id = "bulut", displayName = "Bulut", baseValue = 2, rarity = SymbolRarity.Common, drawWeight = 100, falMeaning = "Belirsizlik, endişe" },
            new() { id = "cicek", displayName = "Çiçek", baseValue = 3, rarity = SymbolRarity.Common, drawWeight = 100, falMeaning = "Güzel haber, teklif" },
            new() { id = "ay", displayName = "Ay", baseValue = 3, rarity = SymbolRarity.Common, drawWeight = 100, falMeaning = "Değişim, gizli duygu" },
            new() { id = "merdiven", displayName = "Merdiven", baseValue = 4, rarity = SymbolRarity.Common, drawWeight = 100, falMeaning = "Yükseliş, terfi" },
            new() { id = "kapi", displayName = "Kapı", baseValue = 3, rarity = SymbolRarity.Common, drawWeight = 100, falMeaning = "Yeni başlangıç, fırsat" },
            new() { id = "mektup", displayName = "Mektup", baseValue = 2, rarity = SymbolRarity.Common, drawWeight = 100, falMeaning = "Beklenen haber" },
            new() { id = "yilan", displayName = "Yılan", baseValue = 5, rarity = SymbolRarity.Uncommon, drawWeight = 45, falMeaning = "Düşman, ihanet, dedikodu" },
            new() { id = "gemi", displayName = "Gemi", baseValue = 5, rarity = SymbolRarity.Uncommon, drawWeight = 45, falMeaning = "Uzak yolculuk" },
            new() { id = "anahtar", displayName = "Anahtar", baseValue = 4, rarity = SymbolRarity.Uncommon, drawWeight = 45, falMeaning = "Yeni fırsat, çözüm" },
            new() { id = "kedi", displayName = "Kedi", baseValue = 4, rarity = SymbolRarity.Uncommon, drawWeight = 45, falMeaning = "Kıskançlık, hile" },
            new() { id = "kopru", displayName = "Köprü", baseValue = 5, rarity = SymbolRarity.Uncommon, drawWeight = 45, falMeaning = "Geçiş dönemi, karar anı" },
            new() { id = "can", displayName = "Çan", baseValue = 4, rarity = SymbolRarity.Uncommon, drawWeight = 45, falMeaning = "Düğün/tören haberi" },
            new() { id = "ok", displayName = "Ok", baseValue = 5, rarity = SymbolRarity.Uncommon, drawWeight = 45, falMeaning = "Yön, kesin karar" },
            new() { id = "dag", displayName = "Dağ", baseValue = 8, rarity = SymbolRarity.Rare, drawWeight = 15, falMeaning = "Büyük engel, zorluk" },
            new() { id = "yildiz", displayName = "Yıldız", baseValue = 7, rarity = SymbolRarity.Rare, drawWeight = 15, falMeaning = "Dilek, umut" },
            new() { id = "gunes", displayName = "Güneş", baseValue = 8, rarity = SymbolRarity.Rare, drawWeight = 15, falMeaning = "Başarı, mutluluk" },
            new() { id = "kuyu", displayName = "Kuyu", baseValue = 7, rarity = SymbolRarity.Rare, drawWeight = 15, falMeaning = "Derin sır" },
            new() { id = "tac", displayName = "Taç", baseValue = 12, rarity = SymbolRarity.Epic, drawWeight = 4, falMeaning = "Büyük güç, kaderin dönüm noktası" },
        };

        // --- docs/design/02-combos.md ---
        static readonly ComboRow[] Combos =
        {
            new() { id = "haber_geliyor", displayName = "Haber Geliyor", symbols = new[] { "yol", "kus" }, effectType = ComboEffectType.Multiplier, effectValue = 1.5f },
            new() { id = "beklenen_mektup", displayName = "Beklenen Mektup", symbols = new[] { "kus", "mektup" }, effectType = ComboEffectType.Multiplier, effectValue = 1.5f },
            new() { id = "yolda_haber", displayName = "Yolda Haber", symbols = new[] { "mektup", "yol" }, effectType = ComboEffectType.Flat, effectValue = 3f },
            new() { id = "dugun_haberi", displayName = "Düğün Haberi", symbols = new[] { "kus", "can" }, effectType = ComboEffectType.Multiplier, effectValue = 1.8f },

            new() { id = "kader_aski", displayName = "Kader Aşkı", symbols = new[] { "kalp", "yildiz" }, effectType = ComboEffectType.Multiplier, effectValue = 2.0f },
            new() { id = "gizli_ask", displayName = "Gizli Aşk", symbols = new[] { "kalp", "ay" }, effectType = ComboEffectType.Multiplier, effectValue = 1.6f },
            new() { id = "ask_ucgeni", displayName = "Aşk Üçgeni", symbols = new[] { "kalp", "kalp" }, effectType = ComboEffectType.Multiplier, effectValue = 1.7f },
            new() { id = "yeni_ask", displayName = "Yeni Aşk", symbols = new[] { "kalp", "kapi" }, effectType = ComboEffectType.Flat, effectValue = 4f },
            new() { id = "ihanete_ugrayan_ask", displayName = "İhanete Uğrayan Aşk", symbols = new[] { "kalp", "yilan" }, effectType = ComboEffectType.Multiplier, effectValue = 1.4f },

            new() { id = "iki_yuzlu_dusman", displayName = "İki Yüzlü Düşman", symbols = new[] { "yilan", "kedi" }, effectType = ComboEffectType.Multiplier, effectValue = 1.9f },
            new() { id = "zorlu_engel", displayName = "Zorlu Engel", symbols = new[] { "dag", "bulut" }, effectType = ComboEffectType.Multiplier, effectValue = 0.8f, isNegative = true },
            new() { id = "nazar_kiriliyor", displayName = "Nazar Kırılıyor", symbols = new[] { "yilan", "goz" }, effectType = ComboEffectType.Multiplier, effectValue = 1.3f },
            new() { id = "dedikodu", displayName = "Dedikodu", symbols = new[] { "kedi", "mektup" }, effectType = ComboEffectType.Multiplier, effectValue = 0.9f, isNegative = true },

            new() { id = "bol_kazanc", displayName = "Bol Kazanç", symbols = new[] { "balik", "gunes" }, effectType = ComboEffectType.Multiplier, effectValue = 2.2f },
            new() { id = "yeni_firsat_kapisi", displayName = "Yeni Fırsat Kapısı", symbols = new[] { "balik", "anahtar" }, effectType = ComboEffectType.Flat, effectValue = 5f },
            new() { id = "zaferin_zirvesi", displayName = "Zaferin Zirvesi", symbols = new[] { "gunes", "tac" }, effectType = ComboEffectType.Multiplier, effectValue = 3.0f },
            new() { id = "yukselen_yildiz", displayName = "Yükselen Yıldız", symbols = new[] { "merdiven", "gunes" }, effectType = ComboEffectType.Multiplier, effectValue = 1.8f },

            new() { id = "uzak_yolculuk", displayName = "Uzak Yolculuk", symbols = new[] { "yol", "gemi" }, effectType = ComboEffectType.Multiplier, effectValue = 1.6f },
            new() { id = "bereketli_deniz", displayName = "Bereketli Deniz", symbols = new[] { "gemi", "balik" }, effectType = ComboEffectType.Flat, effectValue = 6f },
            new() { id = "gecis_donemi", displayName = "Geçiş Dönemi", symbols = new[] { "kopru", "kapi" }, effectType = ComboEffectType.Multiplier, effectValue = 1.5f },
            new() { id = "zorlu_yolculuk", displayName = "Zorlu Yolculuk", symbols = new[] { "yol", "dag" }, effectType = ComboEffectType.Multiplier, effectValue = 1.2f, secondaryFlatBonus = 2f },
            new() { id = "kaderin_kapisi", displayName = "Kaderin Kapısı", symbols = new[] { "anahtar", "kapi" }, effectType = ComboEffectType.Multiplier, effectValue = 2.0f },

            new() { id = "derin_sir", displayName = "Derin Sır", symbols = new[] { "kuyu", "ay" }, effectType = ComboEffectType.Multiplier, effectValue = 1.7f },
            new() { id = "gorulen_sir", displayName = "Görülen Sır", symbols = new[] { "kuyu", "goz" }, effectType = ComboEffectType.Multiplier, effectValue = 1.4f },
            new() { id = "belirsiz_gelecek", displayName = "Belirsiz Gelecek", symbols = new[] { "ay", "bulut" }, effectType = ComboEffectType.Multiplier, effectValue = 0.85f, isNegative = true },

            new() { id = "aile_mutlulugu", displayName = "Aile Mutluluğu", symbols = new[] { "agac", "kalp" }, effectType = ComboEffectType.Multiplier, effectValue = 1.6f },
            new() { id = "yeni_nesil", displayName = "Yeni Nesil", symbols = new[] { "agac", "cicek" }, effectType = ComboEffectType.Flat, effectValue = 5f },
            new() { id = "romantik_surpriz", displayName = "Romantik Sürpriz", symbols = new[] { "cicek", "kalp" }, effectType = ComboEffectType.Multiplier, effectValue = 1.5f },

            new() { id = "korunan_dilek", displayName = "Korunan Dilek", symbols = new[] { "goz", "yildiz" }, effectType = ComboEffectType.Multiplier, effectValue = 1.4f },
            new() { id = "yeni_yola_cikis", displayName = "Yeni Yola Çıkış", symbols = new[] { "kapi", "yol" }, effectType = ComboEffectType.Flat, effectValue = 3f },
            new() { id = "mutlu_toren", displayName = "Mutlu Tören", symbols = new[] { "can", "cicek" }, effectType = ComboEffectType.Multiplier, effectValue = 1.5f },

            new() { id = "kesin_haber", displayName = "Kesin Haber", symbols = new[] { "yol", "kus", "mektup" }, effectType = ComboEffectType.Multiplier, effectValue = 2.5f },
            new() { id = "buyuk_ask_kaderi", displayName = "Büyük Aşk Kaderi", symbols = new[] { "kalp", "yildiz", "tac" }, effectType = ComboEffectType.Multiplier, effectValue = 3.5f },
            new() { id = "efsanevi_bereket", displayName = "Efsanevi Bereket", symbols = new[] { "balik", "gunes", "tac" }, effectType = ComboEffectType.Multiplier, effectValue = 4.0f },
            new() { id = "kara_gun", displayName = "Kara Gün", symbols = new[] { "yilan", "dag", "bulut" }, effectType = ComboEffectType.Multiplier, effectValue = 0.6f, isNegative = true },
            new() { id = "yeni_hayat_yolu", displayName = "Yeni Hayat Yolu", symbols = new[] { "anahtar", "kapi", "kopru" }, effectType = ComboEffectType.Multiplier, effectValue = 2.8f },
            new() { id = "gokyuzu_fali", displayName = "Gökyüzü Falı", symbols = new[] { "goz", "yildiz", "ay" }, effectType = ComboEffectType.Multiplier, effectValue = 2.2f },
        };

        // --- docs/design/05-charms.md ---
        static readonly CharmRow[] Charms =
        {
            new() { id = "kus_tuyu", displayName = "Kuş Tüyü", rarity = SymbolRarity.Common, price = 12, effectTarget = CharmEffectTarget.SymbolValue, effectValue = 2f, description = "Kuş sembollerinin değeri +2" },
            new() { id = "sansli_nazar", displayName = "Şanslı Nazar", rarity = SymbolRarity.Common, price = 12, effectTarget = CharmEffectTarget.NegativeComboSuppression, effectValue = 1.0f, description = "Göz sembolü içeren negatif kombolar etkisiz olur" },
            new() { id = "sadik_dost", displayName = "Sadık Dost", rarity = SymbolRarity.Common, price = 12, effectTarget = CharmEffectTarget.FlatBonus, effectValue = 5f, description = "Aynı sembol iki kez yan yana gelirse +5 sabit bonus" },
            new() { id = "ekonomik_fal", displayName = "Ekonomik Fal", rarity = SymbolRarity.Common, price = 12, effectTarget = CharmEffectTarget.Economy, effectValue = -0.15f, description = "Pazar fiyatları %15 düşer" },
            new() { id = "ilk_kombo_carpani", displayName = "İlk Kombo Çarpanı", rarity = SymbolRarity.Uncommon, price = 22, effectTarget = CharmEffectTarget.ComboMultiplier, effectValue = 2.0f, description = "Dizilimde tetiklenen ilk kombonun çarpanı ×2 olur" },
            new() { id = "kahve_telvesi_yogun", displayName = "Kahve Telvesi Yoğun", rarity = SymbolRarity.Uncommon, price = 22, effectTarget = CharmEffectTarget.DrawRng, effectValue = 1f, description = "Fincanda 1 sembol daha fazla çıkar (6-8 arası)" },
            new() { id = "kara_kedi_tilismi", displayName = "Kara Kedi Tılsımı", rarity = SymbolRarity.Uncommon, price = 22, effectTarget = CharmEffectTarget.NegativeComboSuppression, effectValue = 0.5f, description = "Negatif kombo cezaları %50 azalır" },
            new() { id = "sans_tekerlegi", displayName = "Şans Tekerleği", rarity = SymbolRarity.Rare, price = 38, effectTarget = CharmEffectTarget.DrawRng, effectValue = 0.5f, description = "Rare ve Epic sembol çekiliş ağırlığı %50 artar" },
            new() { id = "muhtarin_gozdesi", displayName = "Muhtarın Gözdesi", rarity = SymbolRarity.Rare, price = 38, effectTarget = CharmEffectTarget.Economy, effectValue = 1.5f, description = "Muhtar müşterisinde ödeme ×1.5" },
            new() { id = "kader_anahtari", displayName = "Kader Anahtarı", rarity = SymbolRarity.Epic, price = 65, effectTarget = CharmEffectTarget.DrawRng, effectValue = 1f, description = "Taç sembolü her elde garanti çıkar" },
        };

        // --- ROADMAP.md Faz 3: "2-3 falcı karakteri" + "açılabilir sembol desteleri" ---
        static readonly CharacterRow[] Characters =
        {
            new()
            {
                id = "varsayilan", displayName = "Falcı Ayşe", wisdomCost = 0,
                description = "Klasik falcı. Dengeli başlangıç destesi, özel bir eğilimi yok.",
                bonusSymbolIds = System.Array.Empty<string>(), startingCharmId = "",
            },
            new()
            {
                id = "kus_falcisi", displayName = "Haberci Zehra", wisdomCost = 15,
                description = "Kuş sembolüne düşkün — destesinde fazladan Kuş var ve Kuş Tüyü tılsımıyla başlar.",
                bonusSymbolIds = new[] { "kus", "kus" }, startingCharmId = "kus_tuyu",
            },
            new()
            {
                id = "kara_kedi_falcisi", displayName = "Gizemli Sema", wisdomCost = 25,
                description = "Riskli fallara inanır — destesinde fazladan Kedi var ve Kara Kedi Tılsımıyla başlar.",
                bonusSymbolIds = new[] { "kedi", "kedi" }, startingCharmId = "kara_kedi_tilismi",
            },
        };
    }
}
