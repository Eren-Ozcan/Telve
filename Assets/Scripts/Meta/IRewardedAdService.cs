using System;

namespace Telve.Meta
{
    /// <summary>
    /// ROADMAP.md Faz 4 "Rewarded ad entegrasyonu: koşu sonu 'ikinci şans'
    /// + bilgelik puanı ×2". Gerçek bir reklam SDK'sı (Unity Ads/AdMob vb.)
    /// bu arayüzü implemente edip GameController.AdService'e atanarak
    /// tak-çalıştır entegre edilir.
    /// </summary>
    public interface IRewardedAdService
    {
        bool IsAdReady();

        /// <summary>onComplete(true) = oyuncu reklamı sonuna kadar izledi, ödül hak edildi.</summary>
        void ShowAd(Action<bool> onComplete);
    }
}
