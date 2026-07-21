using UnityEngine;

namespace Telve.Data
{
    /// <summary>
    /// ROADMAP.md Faz 3 "2-3 falcı karakteri (farklı başlangıç koşulu/pasif
    /// — Balatro'daki deste seçimi muadili)" + "Açılabilir sembol desteleri".
    /// Karakter seçimi = deste seçimi: startingDeckBonusSymbolIds başlangıç
    /// destesine (Common alt kümesi) eklenen ekstra kopyalar (çekiliş
    /// ağırlığını fiilen artırır — aynı sembol listede iki kez varsa iki
    /// katı çekiliş şansı demektir). startingCharmId, oyunun zaten var olan
    /// CharmData sistemini "pasif" olarak yeniden kullanır — ayrı bir efekt
    /// dispatch mekanizması gerekmiyor.
    /// </summary>
    [CreateAssetMenu(fileName = "Character_", menuName = "Telve/FalciCharacter", order = 3)]
    public class FalciCharacter : ScriptableObject
    {
        public string characterId;

        public string displayName;

        [TextArea]
        public string description;

        [Tooltip("Bilgelik puanı maliyeti. 0 = başlangıçta zaten açık.")]
        [Min(0)]
        public int wisdomCost;

        [Tooltip("Başlangıç destesine (Common alt kümesi) eklenen ekstra sembol kopyaları, symbolId ile.")]
        public string[] startingDeckBonusSymbolIds;

        [Tooltip("Boşsa yok. Doluysa CharmData.charmId — koşu başında otomatik aktif tılsım olarak eklenir.")]
        public string startingCharmId;
    }
}
