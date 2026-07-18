using UnityEngine;

namespace Telve.Data
{
    /// <summary>
    /// One fal sembolü. Field set mirrors docs/design/01-symbols.md 1:1 so
    /// the 24-symbol MVP table can be transcribed straight into assets.
    /// </summary>
    [CreateAssetMenu(fileName = "Symbol_", menuName = "Telve/Symbol", order = 0)]
    public class SymbolData : ScriptableObject
    {
        [Tooltip("Stable identifier used by ComboData.requiredSymbolIds — not the display name.")]
        public string symbolId;

        public string displayName;

        [Min(0)]
        public int baseValue;

        public SymbolRarity rarity;

        [Tooltip("Relative draw weight within the fincan çevirme çekilişi. Common=100, Uncommon=45, Rare=15, Epic=4 per the design doc.")]
        [Min(0)]
        public int drawWeight;

        public Sprite sprite;

        [TextArea]
        [Tooltip("Geleneksel fal anlamı — falcı defteri ve müşteri diyaloğunda gösterilir.")]
        public string falMeaning;
    }
}
