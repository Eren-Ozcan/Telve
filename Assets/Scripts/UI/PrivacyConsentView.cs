using Telve.Meta;
using UnityEngine;

namespace Telve.UI
{
    /// <summary>
    /// ROADMAP.md Faz 4 "GDPR/KVKK/ATT izin akışları". YER TUTUCU METİN —
    /// gerçek gizlilik politikası/aydınlatma metni ve ATT (App Tracking
    /// Transparency, iOS) sistem izni entegrasyonu hukuki inceleme
    /// gerektirir, burada değil. Bu bileşen sadece akışın iskeletini
    /// kurar: ilk açılışta bir kez gösterilir, kabul edilmeden oyun
    /// başlamaz, kabul kalıcı kaydedilir.
    /// </summary>
    public class PrivacyConsentView : MonoBehaviour
    {
        [SerializeField] GameObject gate;

        void Awake()
        {
            bool alreadyConsented = MetaProgressStore.LoadPrivacyConsentGiven();
            if (gate != null) gate.SetActive(!alreadyConsented);
        }

        public void OnAcceptButtonPressed()
        {
            MetaProgressStore.SavePrivacyConsentGiven();
            if (gate != null) gate.SetActive(false);
        }
    }
}
