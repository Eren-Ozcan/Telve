using System;

namespace Telve.Meta
{
    /// <summary>
    /// ROADMAP.md Faz 4 "Rewarded ad entegrasyonu: koşu sonu 'ikinci şans'
    /// + bilgelik puanı ×2". Gerçek implementasyon: LevelPlayRewardedAdService
    /// (com.unity.services.levelplay) — LevelPlay hesabı/app key kurulunca
    /// GameController.AdService'e atanarak tak-çalıştır devreye alınır.
    /// </summary>
    public interface IRewardedAdService
    {
        bool IsAdReady();

        /// <summary>onComplete(true) = oyuncu reklamı sonuna kadar izledi, ödül hak edildi.</summary>
        void ShowAd(Action<bool> onComplete);
    }
}
