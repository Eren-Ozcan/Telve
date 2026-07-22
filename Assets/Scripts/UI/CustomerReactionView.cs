using Telve.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Telve.UI
{
    /// <summary>
    /// ROADMAP.md Faz 2 "Müşteri tepki sistemi": eşik aşılıp aşılmadığına
    /// göre 3 ifadeden birini gösterir (irkilme/korkma tek "startled"
    /// sprite'ta birleştirildi — v1 kapsamı). Sprite'lar Coplay
    /// generate_or_edit_images ile üretilir, Inspector'dan atanır.
    /// </summary>
    public class CustomerReactionView : MonoBehaviour
    {
        [SerializeField] GameController controller;
        [SerializeField] Image portraitImage;
        [SerializeField] Sprite neutralSprite;
        [SerializeField] Sprite happySprite;
        [SerializeField] Sprite startledSprite;

        void OnEnable()
        {
            controller.OnEncounterResolved += HandleEncounterResolved;
            controller.OnCupDrawn += ResetPortrait;

            // Koşu-ortası kayıttan devamda son karşılaşmanın sonucunu yansıt
            // (aksi hâlde her zaman nötr'e sıfırlanırdı — bkz. GameController.
            // LastEncounterThresholdMet).
            if (controller.LastEncounterThresholdMet is { } thresholdMet)
            {
                if (portraitImage != null) portraitImage.sprite = thresholdMet ? happySprite : startledSprite;
            }
            else
            {
                ResetPortrait();
            }
        }

        void OnDisable()
        {
            controller.OnEncounterResolved -= HandleEncounterResolved;
            controller.OnCupDrawn -= ResetPortrait;
        }

        void ResetPortrait()
        {
            if (portraitImage != null) portraitImage.sprite = neutralSprite;
        }

        void HandleEncounterResolved(EncounterResult result)
        {
            if (portraitImage == null) return;
            portraitImage.sprite = result.Payment.ThresholdMet ? happySprite : startledSprite;
        }
    }
}
