using System;

namespace Telve.Meta
{
    /// <summary>
    /// ROADMAP.md Faz 4 "IAP altyapısı (Unity IAP)". Gerçek bir mağaza
    /// entegrasyonu (Unity IAP + App Store/Google Play) bu arayüzü
    /// implemente edip GameController.PurchaseService'e atanarak
    /// tak-çalıştır entegre edilir.
    /// </summary>
    public interface IPurchaseService
    {
        bool IsOwned(string cosmeticId);

        /// <summary>onComplete(true) = satın alma başarılı.</summary>
        void Purchase(string cosmeticId, Action<bool> onComplete);
    }
}
