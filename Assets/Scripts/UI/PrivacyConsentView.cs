using Telve.Meta;
using UnityEngine;

namespace Telve.UI
{
    /// <summary>
    /// ROADMAP.md Faz 4 "GDPR/KVKK/ATT izin akışları". Sahnedeki ConsentText
    /// artık yer tutucu değil, gerçekçi bir KVKK/GDPR aydınlatma taslağı
    /// taşıyor (Game.unity, GameObject "ConsentText") — ama nihai metin
    /// bağımsız hukuki incelemeden geçmeli, burada onaylanmış sayılmaz.
    /// iOS ATT (App Tracking Transparency) sistem izni entegrasyonu ayrı
    /// iş, henüz yok. Bu bileşen akışın iskeletini kurar: ilk açılışta bir
    /// kez gösterilir, kabul edilmeden oyun başlamaz, kabul kalıcı kaydedilir.
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
