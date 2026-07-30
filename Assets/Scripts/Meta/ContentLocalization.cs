using System.Collections.Generic;
using Telve.Data;
using Telve.Gameplay;
using UnityEngine;

namespace Telve.Meta
{
    /// <summary>
    /// ROADMAP.md Faz 4 "fal terminolojisinin İngilizce karşılıkları":
    /// sembol/kombo/tılsım/karakter adı ve açıklamalarının EN karşılığı.
    /// Localization sınıfından kasıtlı ayrı — o sadece statik UI çerçeve
    /// metnini taşır (bkz. LocalizationTable.cs yorumu). Eksik çeviri
    /// sessizce TR kaynağa (SymbolData.displayName vb.) düşer.
    /// </summary>
    public static class ContentLocalization
    {
        static Dictionary<string, ContentLocalizationEntry> _symbols;
        static Dictionary<string, ContentLocalizationEntry> _combos;
        static Dictionary<string, ContentLocalizationEntry> _charms;
        static Dictionary<string, ContentLocalizationEntry> _characters;
        static bool _initialized;

        public static string SymbolName(SymbolData symbol)
        {
            EnsureInitialized();
            return Resolve(_symbols, symbol.symbolId, symbol.displayName, false);
        }

        public static string SymbolMeaning(SymbolData symbol)
        {
            EnsureInitialized();
            return Resolve(_symbols, symbol.symbolId, symbol.falMeaning, true);
        }

        public static string ComboName(ComboData combo)
        {
            EnsureInitialized();
            return Resolve(_combos, combo.comboId, combo.displayName, false);
        }

        public static string CharmName(CharmData charm)
        {
            EnsureInitialized();
            return Resolve(_charms, charm.charmId, charm.displayName, false);
        }

        public static string CharmDescription(CharmData charm)
        {
            EnsureInitialized();
            return Resolve(_charms, charm.charmId, charm.description, true);
        }

        public static string CharacterName(FalciCharacter character)
        {
            EnsureInitialized();
            return Resolve(_characters, character.characterId, character.displayName, false);
        }

        public static string CharacterDescription(FalciCharacter character)
        {
            EnsureInitialized();
            return Resolve(_characters, character.characterId, character.description, true);
        }

        public static string ArchetypeName(CustomerArchetype archetype)
        {
            string key = archetype switch
            {
                CustomerArchetype.Aceleci => "archetype.aceleci",
                CustomerArchetype.Kuskucu => "archetype.kuskucu",
                CustomerArchetype.Dertli => "archetype.dertli",
                CustomerArchetype.Comert => "archetype.comert",
                _ => null,
            };
            return key == null ? archetype.ToString() : Localization.Get(key);
        }

        static string Resolve(Dictionary<string, ContentLocalizationEntry> map, string id, string trFallback, bool detail)
        {
            if (Localization.Current == Language.Turkish) return trFallback;
            if (map == null || !map.TryGetValue(id, out var entry)) return trFallback;

            string value = detail ? entry.detailEn : entry.nameEn;
            return string.IsNullOrEmpty(value) ? trFallback : value;
        }

        static void EnsureInitialized()
        {
            if (_initialized) return;
            _initialized = true;

            _symbols = Load("Data/Localization/SymbolStrings");
            _combos = Load("Data/Localization/ComboStrings");
            _charms = Load("Data/Localization/CharmStrings");
            _characters = Load("Data/Localization/CharacterStrings");
        }

        static Dictionary<string, ContentLocalizationEntry> Load(string resourcePath)
        {
            var map = new Dictionary<string, ContentLocalizationEntry>();
            var table = Resources.Load<ContentLocalizationTable>(resourcePath);
            if (table != null)
            {
                foreach (var entry in table.entries) map[entry.id] = entry;
            }

            return map;
        }
    }
}
