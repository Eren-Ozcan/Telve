using UnityEngine;

namespace Telve.Data
{
    [System.Serializable]
    public struct LocalizationEntry
    {
        public string key;
        public string tr;
        public string en;
    }

    /// <summary>
    /// ROADMAP.md Faz 4 "Yerelleştirme altyapısı: TR + EN baştan". Tek
    /// tablo — sembol/kombo/tılsım/karakter adları gibi fal terminolojisi
    /// içeriği kasıtlı olarak burada değil (ROADMAP.md: "fal terminolojisinin
    /// İngilizce karşılıkları ayrı iş — erken başla"); bu tablo sadece UI
    /// çerçevesi (buton/panel metinleri) içindir.
    /// </summary>
    [CreateAssetMenu(fileName = "LocalizationTable", menuName = "Telve/LocalizationTable", order = 4)]
    public class LocalizationTable : ScriptableObject
    {
        public LocalizationEntry[] entries;
    }
}
