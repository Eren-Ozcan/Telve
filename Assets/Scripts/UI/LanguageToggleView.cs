using Telve.Meta;
using UnityEngine;
using UnityEngine.UI;

namespace Telve.UI
{
    /// <summary>ROADMAP.md Faz 4 yerelleştirme: TR/EN arasında geçiş yapan buton.</summary>
    public class LanguageToggleView : MonoBehaviour
    {
        [SerializeField] Text buttonLabel;

        void OnEnable()
        {
            Localization.OnLanguageChanged += Refresh;
            Refresh();
        }

        void OnDisable() => Localization.OnLanguageChanged -= Refresh;

        public void OnToggleButtonPressed()
        {
            var next = Localization.Current == Language.Turkish ? Language.English : Language.Turkish;
            Localization.SetLanguage(next);
        }

        void Refresh()
        {
            if (buttonLabel != null) buttonLabel.text = Localization.Current == Language.Turkish ? "EN" : "TR";
        }
    }
}
