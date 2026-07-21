using Telve.Meta;
using UnityEngine;
using UnityEngine.UI;

namespace Telve.UI
{
    /// <summary>
    /// ROADMAP.md Faz 4 "Yerelleştirme altyapısı". Statik UI çerçeve metni
    /// (buton/panel başlığı gibi) taşıyan Text bileşenlerine eklenir; dil
    /// değiştiğinde otomatik günceller. Dinamik/interpolasyonlu metinler
    /// (GameView.Refresh() içindeki durum/skor metinleri gibi) kapsam
    /// dışı — bkz. ROADMAP.md notu.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] string key;

        Text _text;

        void Awake() => _text = GetComponent<Text>();

        void OnEnable()
        {
            Localization.OnLanguageChanged += Refresh;
            Refresh();
        }

        void OnDisable() => Localization.OnLanguageChanged -= Refresh;

        void Refresh() => _text.text = Localization.Get(key);
    }
}
