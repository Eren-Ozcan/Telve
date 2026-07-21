using System;

namespace Telve.Meta
{
    /// <summary>
    /// GERÇEK REKLAM SDK'SI DEĞİL. Reklam her zaman "hazır" ve izlenince
    /// her zaman başarılı sonuçlanır — geliştirme/QA için. ROADMAP.md
    /// Faz 4'te gerçek bir reklam ağı bağlanana kadar varsayılan.
    /// </summary>
    public class MockRewardedAdService : IRewardedAdService
    {
        public bool IsAdReady() => true;

        public void ShowAd(Action<bool> onComplete) => onComplete?.Invoke(true);
    }
}
