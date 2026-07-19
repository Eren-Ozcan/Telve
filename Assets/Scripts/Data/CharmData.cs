using UnityEngine;

namespace Telve.Data
{
    /// <summary>
    /// One passive tılsım from docs/design/05-charms.md. Purely data —
    /// the actual effect logic dispatches on effectTarget at runtime.
    /// </summary>
    [CreateAssetMenu(fileName = "Charm_", menuName = "Telve/Charm", order = 2)]
    public class CharmData : ScriptableObject
    {
        public string charmId;

        public string displayName;

        public SymbolRarity rarity;

        [Min(0)]
        public int price;

        public CharmEffectTarget effectTarget;

        public float effectValue;

        [TextArea]
        public string description;

        [Tooltip("ROADMAP.md Faz 2 içerik tamamlama: tılsımın pazar/envanter ikonu.")]
        public Sprite icon;
    }
}
