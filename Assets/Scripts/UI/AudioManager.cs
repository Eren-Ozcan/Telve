using System.Collections.Generic;
using Telve.Gameplay;
using UnityEngine;

namespace Telve.UI
{
    /// <summary>
    /// ROADMAP.md Faz 2 "Ses": GameController olaylarına abone olup ilgili
    /// SFX/müzik klibini çalar. Klipler Coplay generate_sfx/generate_music
    /// ile üretilip Assets/Audio altına kaydedilir, buradaki alanlara
    /// Inspector'dan atanır — hiçbiri kod içinde hardcode edilmez.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] GameController controller;
        [SerializeField] AudioSource sfxSource;
        [SerializeField] AudioSource musicSource;

        [SerializeField] AudioClip cupDrawClip;
        [SerializeField] AudioClip comboHitClip;
        [SerializeField] AudioClip purchaseClip;
        [SerializeField] AudioClip positiveResultClip;
        [SerializeField] AudioClip negativeResultClip;
        [SerializeField] AudioClip ambientLoopClip;

        void OnEnable()
        {
            controller.OnCupDrawn += PlayCupDraw;
            controller.OnEncounterResolved += PlayEncounterResult;
            controller.OnNewCombosDiscovered += PlayComboHit;
            controller.OnOfferPurchased += PlayPurchase;

            if (ambientLoopClip != null && musicSource != null && !musicSource.isPlaying)
            {
                musicSource.clip = ambientLoopClip;
                musicSource.loop = true;
                musicSource.Play();
            }
        }

        void OnDisable()
        {
            controller.OnCupDrawn -= PlayCupDraw;
            controller.OnEncounterResolved -= PlayEncounterResult;
            controller.OnNewCombosDiscovered -= PlayComboHit;
            controller.OnOfferPurchased -= PlayPurchase;
        }

        void PlayCupDraw() => PlayOneShot(cupDrawClip);

        void PlayEncounterResult(EncounterResult result) =>
            PlayOneShot(result.Payment.ThresholdMet ? positiveResultClip : negativeResultClip);

        void PlayComboHit(IReadOnlyList<string> _) => PlayOneShot(comboHitClip);

        void PlayPurchase(MarketOffer _) => PlayOneShot(purchaseClip);

        void PlayOneShot(AudioClip clip)
        {
            if (clip == null || sfxSource == null) return;
            sfxSource.PlayOneShot(clip);
        }
    }
}
