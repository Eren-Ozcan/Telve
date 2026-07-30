using UnityEngine;

namespace Telve.Data
{
    [System.Serializable]
    public struct ContentLocalizationEntry
    {
        public string id;
        public string nameEn;

        [TextArea]
        public string detailEn;
    }

    /// <summary>
    /// ROADMAP.md Faz 4 "fal terminolojisinin İngilizce karşılıkları — ayrı
    /// iş" burada karşılanır: sembol/kombo/tılsım/karakter TR verisi zaten
    /// SymbolData/ComboData/CharmData/FalciCharacter üzerinde, bu tablo id
    /// üzerinden sadece EN karşılığını taşır (kaynak asset'leri elle
    /// düzenlemeye gerek kalmaz). Ayrı tablo örnekleri: SymbolStrings,
    /// ComboStrings, CharmStrings, CharacterStrings — bkz. ContentLocalization.
    /// </summary>
    [CreateAssetMenu(fileName = "ContentLocalizationTable", menuName = "Telve/ContentLocalizationTable", order = 6)]
    public class ContentLocalizationTable : ScriptableObject
    {
        public ContentLocalizationEntry[] entries;
    }
}
