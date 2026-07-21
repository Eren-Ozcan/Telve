using System;
using System.Collections.Generic;
using Telve.Data;
using UnityEngine;

namespace Telve.Meta
{
    public enum Language
    {
        Turkish,
        English,
    }

    /// <summary>
    /// ROADMAP.md Faz 4 "Yerelleştirme altyapısı: TR + EN baştan". Tabloyu
    /// Resources'tan yükler, Get(key) mevcut dile göre string döner —
    /// eksik anahtar sessizce anahtarın kendisini döner (çeviri eksikse
    /// bile UI kırılmaz). Dil seçimi PlayerPrefs ile kalıcı.
    /// </summary>
    public static class Localization
    {
        static Dictionary<string, LocalizationEntry> _table;
        static Language _current;
        static bool _initialized;

        public static event Action OnLanguageChanged;

        public static Language Current
        {
            get
            {
                EnsureInitialized();
                return _current;
            }
        }

        public static void SetLanguage(Language language)
        {
            EnsureInitialized();
            if (_current == language) return;

            _current = language;
            MetaProgressStore.SaveLanguage(language);
            OnLanguageChanged?.Invoke();
        }

        public static string Get(string key)
        {
            EnsureInitialized();
            if (string.IsNullOrEmpty(key)) return string.Empty;
            if (!_table.TryGetValue(key, out var entry)) return key;

            return _current == Language.Turkish ? entry.tr : entry.en;
        }

        static void EnsureInitialized()
        {
            if (_initialized) return;
            _initialized = true;

            _table = new Dictionary<string, LocalizationEntry>();
            var loaded = Resources.Load<LocalizationTable>("Data/Localization/UiStrings");
            if (loaded != null)
            {
                foreach (var entry in loaded.entries) _table[entry.key] = entry;
            }

            _current = MetaProgressStore.LoadLanguage();
        }
    }
}
