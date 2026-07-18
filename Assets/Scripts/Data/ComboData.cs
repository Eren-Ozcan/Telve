using UnityEngine;

namespace Telve.Data
{
    /// <summary>
    /// One kombo from docs/design/02-combos.md. requiredSymbolIds is an
    /// ordered, adjacent run of SymbolData.symbolId values (length 2 for
    /// ikili, length 3 for üçlü kombolar) — matched against consecutive
    /// positions in the player's reading order.
    /// </summary>
    [CreateAssetMenu(fileName = "Combo_", menuName = "Telve/Combo", order = 1)]
    public class ComboData : ScriptableObject
    {
        public string comboId;

        public string displayName;

        [Tooltip("Ordered symbolId run this combo matches against adjacent reading-order positions.")]
        public string[] requiredSymbolIds;

        public ComboEffectType effectType;

        [Tooltip("Multiplier: e.g. 1.5 for ×1.5. Flat: added to the base score before multipliers, e.g. 3 for +3.")]
        public float effectValue;

        [Tooltip("Rare compound-effect combos (ör. 'Zorlu Yolculuk' ×1.2, +2) carry both a multiplier and a flat bonus. 0 for the common single-effect case.")]
        public float secondaryFlatBonus;

        [Tooltip("Negatif kombolar (ör. Kara Gün ×0.6) kasıtlı ceza taşır — bkz. docs/design/02-combos.md tasarım notları.")]
        public bool isNegative;
    }
}
