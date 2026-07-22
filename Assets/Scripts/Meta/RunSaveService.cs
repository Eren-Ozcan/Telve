using UnityEngine;

namespace Telve.Meta
{
    /// <summary>ROADMAP.md Faz 3 "Kayıt sistemi": RunSaveData'nın PlayerPrefs'e ham JSON olarak saklanması.</summary>
    public static class RunSaveService
    {
        const string Key = "Telve.Meta.RunSave";

        public static bool HasSavedRun() => PlayerPrefs.HasKey(Key);

        /// <summary>
        /// flushToDisk=false: sadece PlayerPrefs'in bellek-içi değerini
        /// günceller (ucuz) — sık tetiklenen ama düşük riskli mutasyonlar
        /// için (ör. ToggleCupSlot: uygulama tam bu anda öldürülürse
        /// sadece en son dokunuş kaybolur, gerçek ilerleme değil).
        /// flushToDisk=true: PlayerPrefs.Save() ile diske de yazar — gerçek
        /// cihazlarda (Android XML dosyası) Editor'daki (registry) kadar
        /// ucuz değildir, bu yüzden sadece anlamlı kontrol noktalarında
        /// (fincan çekildi, fal okundu, pazar alışverişi) kullanılır.
        /// </summary>
        public static void Save(RunSaveData data, bool flushToDisk = true)
        {
            PlayerPrefs.SetString(Key, JsonUtility.ToJson(data));
            if (flushToDisk) PlayerPrefs.Save();
        }

        public static RunSaveData Load()
        {
            if (!PlayerPrefs.HasKey(Key)) return null;

            string json = PlayerPrefs.GetString(Key);
            return string.IsNullOrEmpty(json) ? null : JsonUtility.FromJson<RunSaveData>(json);
        }

        public static void Clear() => PlayerPrefs.DeleteKey(Key);
    }
}
