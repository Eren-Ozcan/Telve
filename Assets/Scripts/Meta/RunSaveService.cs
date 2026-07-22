using UnityEngine;

namespace Telve.Meta
{
    /// <summary>ROADMAP.md Faz 3 "Kayıt sistemi": RunSaveData'nın PlayerPrefs'e ham JSON olarak saklanması.</summary>
    public static class RunSaveService
    {
        const string Key = "Telve.Meta.RunSave";

        public static bool HasSavedRun() => PlayerPrefs.HasKey(Key);

        public static void Save(RunSaveData data)
        {
            PlayerPrefs.SetString(Key, JsonUtility.ToJson(data));
            PlayerPrefs.Save();
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
