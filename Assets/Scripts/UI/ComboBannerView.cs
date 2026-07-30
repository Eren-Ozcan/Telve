using System.Collections;
using System.Collections.Generic;
using Telve.Gameplay;
using Telve.Meta;
using UnityEngine;
using UnityEngine.UI;

namespace Telve.UI
{
    /// <summary>
    /// ROADMAP.md Faz 2 "Kombo tetiklenme geri bildirimi": eşleşen her
    /// kombo için sırayla isim kartı ("X Fark Edildi") gösterir + hafif
    /// ekran sarsıntısı uygular. Harici tween paketi yok — düz coroutine.
    /// GameController.OnEncounterResolved'daki ScoreResult.TriggeredCombos
    /// listesini kullanır.
    /// </summary>
    public class ComboBannerView : MonoBehaviour
    {
        [SerializeField] GameController controller;
        [SerializeField] GameObject bannerRoot;
        [SerializeField] Text bannerText;
        [SerializeField] RectTransform shakeTarget;
        [SerializeField] float perComboDisplaySeconds = 1.1f;
        [SerializeField] float shakeMagnitude = 12f;
        [SerializeField] float shakeDuration = 0.2f;

        static readonly Color PositiveColor = new(1f, 0.85f, 0.3f);
        static readonly Color NegativeColor = new(0.85f, 0.35f, 0.35f);

        Coroutine _routine;

        void OnEnable()
        {
            controller.OnEncounterResolved += HandleEncounterResolved;
            bannerRoot.SetActive(false);
        }

        void OnDisable()
        {
            controller.OnEncounterResolved -= HandleEncounterResolved;
        }

        void HandleEncounterResolved(EncounterResult result)
        {
            if (result.Score.TriggeredCombos.Count == 0) return;
            if (_routine != null) StopCoroutine(_routine);
            _routine = StartCoroutine(PlayBanners(result.Score.TriggeredCombos));
        }

        IEnumerator PlayBanners(IReadOnlyList<ComboMatch> combos)
        {
            foreach (var match in combos)
            {
                bannerRoot.SetActive(true);
                bannerText.text = string.Format(Localization.Get("combo_banner.discovered_template"), ContentLocalization.ComboName(match.Combo));
                bannerText.color = match.Combo.isNegative ? NegativeColor : PositiveColor;

                if (shakeTarget != null) yield return StartCoroutine(Shake());

                yield return new WaitForSeconds(perComboDisplaySeconds);
            }

            bannerRoot.SetActive(false);
            _routine = null;
        }

        IEnumerator Shake()
        {
            Vector2 originalPos = shakeTarget.anchoredPosition;
            float elapsed = 0f;
            while (elapsed < shakeDuration)
            {
                elapsed += Time.deltaTime;
                float damper = 1f - elapsed / shakeDuration;
                shakeTarget.anchoredPosition = originalPos + Random.insideUnitCircle * shakeMagnitude * damper;
                yield return null;
            }

            shakeTarget.anchoredPosition = originalPos;
        }
    }
}
