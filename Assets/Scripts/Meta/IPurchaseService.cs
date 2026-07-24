using System;

namespace Telve.Meta
{
    /// <summary>
    /// ROADMAP.md Faz 4 "IAP altyapısı (Unity IAP)". Gerçek implementasyon:
    /// UnityIAPPurchaseService (com.unity.purchasing) — mağaza ürün kataloğu
    /// kurulunca GameController.PurchaseService'e atanarak tak-çalıştır
    /// devreye alınır.
    /// </summary>
    public interface IPurchaseService
    {
        bool IsOwned(string cosmeticId);

        /// <summary>onComplete(true) = satın alma başarılı.</summary>
        void Purchase(string cosmeticId, Action<bool> onComplete);
    }
}
