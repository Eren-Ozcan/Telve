using UnityEngine;

namespace Telve.Data
{
    public enum CosmeticCategory
    {
        CupSkin,
        Tablecloth,
    }

    /// <summary>
    /// ROADMAP.md Faz 4 "IAP altyapısı: fincan/masa örtüsü kozmetikleri —
    /// güç satmıyoruz, sadece görünüm". priceDisplay bilinçli olarak
    /// string: gerçek fiyat/para birimi mağaza tarafından (App Store/
    /// Google Play ürün kataloğu) belirlenir, burada sadece placeholder.
    /// </summary>
    [CreateAssetMenu(fileName = "Cosmetic_", menuName = "Telve/CosmeticItem", order = 5)]
    public class CosmeticItem : ScriptableObject
    {
        public string cosmeticId;
        public string displayName;
        public CosmeticCategory category;
        public Sprite previewSprite;

        [Tooltip("Placeholder — gerçek fiyat mağaza ürün kataloğundan gelir.")]
        public string priceDisplay;
    }
}
