using System;
using System.Collections.Generic;

namespace Telve.Meta
{
    /// <summary>
    /// GERÇEK MAĞAZA BAĞLANTISI DEĞİL — ödeme almaz. Satın alma isteği her
    /// zaman anında başarılı sonuçlanır ve sahiplik yerelde (PlayerPrefs)
    /// işaretlenir. ROADMAP.md Faz 4'te Unity IAP + gerçek mağaza ürün
    /// kataloğu bağlanana kadar varsayılan — geliştirme/QA için.
    /// </summary>
    public class MockPurchaseService : IPurchaseService
    {
        readonly HashSet<string> _owned;

        public MockPurchaseService()
        {
            _owned = new HashSet<string>(MetaProgressStore.LoadOwnedCosmeticIds());
        }

        public bool IsOwned(string cosmeticId) => _owned.Contains(cosmeticId);

        public void Purchase(string cosmeticId, Action<bool> onComplete)
        {
            if (_owned.Add(cosmeticId))
            {
                MetaProgressStore.SaveOwnedCosmeticIds(_owned);
            }

            onComplete?.Invoke(true);
        }
    }
}
