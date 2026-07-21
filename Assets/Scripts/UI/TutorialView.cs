using Telve.Gameplay;
using Telve.Meta;
using UnityEngine;
using UnityEngine.UI;

namespace Telve.UI
{
    /// <summary>
    /// ROADMAP.md Faz 4 "Tutorial / ilk 5 dakika akışı (ilk müşteri =
    /// öğretici fal)". Oyuncunun hiç görmediği ilk karşılaşma boyunca
    /// (fincan çevir → sırala → oku → sonuç) bağlamsal ipuçları gösterir;
    /// bir kez tamamlanınca MetaProgressStore'a kalıcı işaretlenir ve bir
    /// daha hiç görünmez — sonraki müşteriler/koşular etkilenmez.
    /// </summary>
    public class TutorialView : MonoBehaviour
    {
        [SerializeField] GameController controller;
        [SerializeField] GameObject hintRoot;
        [SerializeField] Text hintText;

        bool _active;
        bool _resultShown;

        void OnEnable()
        {
            _active = !MetaProgressStore.LoadTutorialCompleted();
            if (hintRoot != null) hintRoot.SetActive(_active);
            if (!_active) return;

            controller.OnStateChanged += Refresh;
            controller.OnEncounterResolved += HandleEncounterResolved;
            controller.OnCupDrawn += HandleCupDrawn;
            Refresh();
        }

        void OnDisable()
        {
            controller.OnStateChanged -= Refresh;
            controller.OnEncounterResolved -= HandleEncounterResolved;
            controller.OnCupDrawn -= HandleCupDrawn;
        }

        void Refresh()
        {
            if (!_active || _resultShown) return;

            hintText.text = controller.ReadingOrderCupIndices.Count == 0
                ? "İlk müşterin geldi! Fincandaki sembollere dokunarak bir okuma sırası oluştur."
                : "Sırayı beğendiysen \"Falı Oku\" butonuna bas.";
        }

        void HandleEncounterResolved(EncounterResult result)
        {
            if (!_active || _resultShown) return;

            _resultShown = true;
            hintText.text = "Skorun müşterinin eşiğini aşarsa iyi ödeme alırsın. \"Sıradaki Müşteri\" ile devam et.";
            MetaProgressStore.SaveTutorialCompleted();
        }

        void HandleCupDrawn()
        {
            if (!_resultShown) return;

            _active = false;
            if (hintRoot != null) hintRoot.SetActive(false);
        }
    }
}
