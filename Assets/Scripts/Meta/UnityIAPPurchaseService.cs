using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Telve.Meta
{
    /// <summary>
    /// ROADMAP.md Faz 4 "IAP altyapısı (Unity IAP)". Gerçek com.unity.purchasing
    /// SDK'sına bağlı. ÜRÜN KATALOĞU (App Store Connect / Google Play Console
    /// tarafında `Telve.Data.CosmeticItem.cosmeticId`lerle eşleşen ürünlerin
    /// oluşturulması) ve imzalı bir build olmadan `UnityPurchasing.Initialize`
    /// tamamlanmaz — bu sınıf bu yüzden mağaza hesabı kurulana kadar
    /// GameController.Awake()'te varsayılan olarak ATANMAZ (hâlâ
    /// MockPurchaseService kullanılıyor). Hesap/katalog hazır olduğunda
    /// `GameController.PurchaseService = new UnityIAPPurchaseService();`
    /// ile tak-çalıştır devreye alınır.
    /// </summary>
    public class UnityIAPPurchaseService : IPurchaseService, IDetailedStoreListener
    {
        IStoreController _controller;
        bool _initialized;
        readonly Dictionary<string, Action<bool>> _pendingCallbacks = new();

        public UnityIAPPurchaseService()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            foreach (var cosmetic in Resources.LoadAll<Telve.Data.CosmeticItem>("Data/Cosmetics"))
            {
                builder.AddProduct(cosmetic.cosmeticId, ProductType.NonConsumable);
            }

            UnityPurchasing.Initialize(this, builder);
        }

        public bool IsOwned(string cosmeticId)
        {
            if (!_initialized) return false;
            var product = _controller.products.WithID(cosmeticId);
            return product != null && product.hasReceipt;
        }

        public void Purchase(string cosmeticId, Action<bool> onComplete)
        {
            if (!_initialized)
            {
                onComplete?.Invoke(false);
                return;
            }

            var product = _controller.products.WithID(cosmeticId);
            if (product == null || !product.availableToPurchase)
            {
                onComplete?.Invoke(false);
                return;
            }

            _pendingCallbacks[cosmeticId] = onComplete;
            _controller.InitiatePurchase(product);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _controller = controller;
            _initialized = true;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            _initialized = false;
            Debug.LogError($"UnityIAPPurchaseService: başlatma başarısız — {error}");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            _initialized = false;
            Debug.LogError($"UnityIAPPurchaseService: başlatma başarısız — {error}: {message}");
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            var id = purchaseEvent.purchasedProduct.definition.id;
            if (_pendingCallbacks.TryGetValue(id, out var callback))
            {
                _pendingCallbacks.Remove(id);
                callback?.Invoke(true);
            }

            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason reason) =>
            FailPurchase(product, reason.ToString());

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription) =>
            FailPurchase(product, $"{failureDescription.reason}: {failureDescription.message}");

        void FailPurchase(Product product, string reasonText)
        {
            if (_pendingCallbacks.TryGetValue(product.definition.id, out var callback))
            {
                _pendingCallbacks.Remove(product.definition.id);
                callback?.Invoke(false);
            }

            Debug.LogWarning($"UnityIAPPurchaseService: satın alma başarısız — {product.definition.id}: {reasonText}");
        }
    }
}
