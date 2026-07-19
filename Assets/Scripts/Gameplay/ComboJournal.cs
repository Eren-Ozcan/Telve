using System.Collections.Generic;

namespace Telve.Gameplay
{
    /// <summary>
    /// ROADMAP.md Faz 2 "Falcı defteri v1": keşfedilen kombo id'lerinin
    /// kaydı. Kalıcılık (Faz 3 kayıt sistemi) bu sınıfın dışında — burada
    /// sadece bir oturum içindeki keşif durumu ve "ilk keşif mi" sinyali
    /// tutulur; alreadyDiscovered parametresi Faz 3'te save'den yüklenen
    /// id kümesini enjekte etmek için var.
    /// </summary>
    public class ComboJournal
    {
        readonly HashSet<string> _discoveredComboIds;

        public IReadOnlyCollection<string> DiscoveredComboIds => _discoveredComboIds;

        public ComboJournal(IEnumerable<string> alreadyDiscovered = null)
        {
            _discoveredComboIds = alreadyDiscovered != null
                ? new HashSet<string>(alreadyDiscovered)
                : new HashSet<string>();
        }

        public bool IsDiscovered(string comboId) => _discoveredComboIds.Contains(comboId);

        /// <summary>Tetiklenen komboları kaydeder; ilk kez görülenlerin id'lerini döner (altın çerçeve anı için).</summary>
        public List<string> RecordEncounter(IEnumerable<ComboMatch> triggeredCombos)
        {
            var newlyDiscovered = new List<string>();
            foreach (var match in triggeredCombos)
            {
                if (_discoveredComboIds.Add(match.Combo.comboId))
                {
                    newlyDiscovered.Add(match.Combo.comboId);
                }
            }

            return newlyDiscovered;
        }
    }
}
