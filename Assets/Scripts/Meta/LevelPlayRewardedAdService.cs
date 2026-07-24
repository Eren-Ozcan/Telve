using System;
using Unity.Services.LevelPlay;
using UnityEngine;

namespace Telve.Meta
{
    /// <summary>
    /// ROADMAP.md Faz 4 "Rewarded ad entegrasyonu". Gerçek com.unity.services.levelplay
    /// (LevelPlay ad mediation) SDK'sına bağlı. Bir LevelPlay hesabı + gerçek
    /// app key/ad unit ID olmadan `LevelPlay.Init` başarıyla tamamlanmaz — bu
    /// yüzden mağaza/reklam hesabı kurulana kadar GameController.Awake()'te
    /// varsayılan olarak ATANMAZ (hâlâ MockRewardedAdService kullanılıyor).
    /// Hesap hazır olduğunda gerçek appKey/adUnitId ile
    /// `GameController.AdService = new LevelPlayRewardedAdService(appKey, adUnitId);`
    /// ile tak-çalıştır devreye alınır.
    /// </summary>
    public class LevelPlayRewardedAdService : IRewardedAdService
    {
        const string PlaceholderAppKey = "YOUR_LEVELPLAY_APP_KEY";
        const string PlaceholderAdUnitId = "YOUR_REWARDED_AD_UNIT_ID";

        readonly LevelPlayRewardedAd _rewardedAd;
        bool _sdkInitialized;
        Action<bool> _pendingCallback;

        public LevelPlayRewardedAdService(string appKey = PlaceholderAppKey, string adUnitId = PlaceholderAdUnitId)
        {
            _rewardedAd = new LevelPlayRewardedAd(adUnitId);
            _rewardedAd.OnAdRewarded += OnAdRewarded;
            _rewardedAd.OnAdClosed += OnAdClosed;
            _rewardedAd.OnAdDisplayFailed += OnAdDisplayFailed;

            LevelPlay.OnInitSuccess += OnInitSuccess;
            LevelPlay.OnInitFailed += OnInitFailed;
            LevelPlay.Init(appKey);
        }

        public bool IsAdReady() => _sdkInitialized && _rewardedAd.IsAdReady();

        public void ShowAd(Action<bool> onComplete)
        {
            if (!IsAdReady())
            {
                onComplete?.Invoke(false);
                return;
            }

            _pendingCallback = onComplete;
            _rewardedAd.ShowAd();
        }

        void OnInitSuccess(LevelPlayConfiguration configuration)
        {
            _sdkInitialized = true;
            _rewardedAd.LoadAd();
        }

        void OnInitFailed(LevelPlayInitError error)
        {
            _sdkInitialized = false;
            Debug.LogError($"LevelPlayRewardedAdService: SDK başlatma başarısız — {error}");
        }

        void OnAdRewarded(LevelPlayAdInfo adInfo, LevelPlayReward reward)
        {
            _pendingCallback?.Invoke(true);
            _pendingCallback = null;
        }

        void OnAdClosed(LevelPlayAdInfo adInfo)
        {
            // OnAdRewarded ödülü zaten verdiyse _pendingCallback burada null'dır.
            // İzleyici ödül almadan (erken) kapattıysa başarısız say.
            if (_pendingCallback != null)
            {
                _pendingCallback.Invoke(false);
                _pendingCallback = null;
            }

            _rewardedAd.LoadAd();
        }

        void OnAdDisplayFailed(LevelPlayAdInfo adInfo, LevelPlayAdError error)
        {
            _pendingCallback?.Invoke(false);
            _pendingCallback = null;
            _rewardedAd.LoadAd();
        }
    }
}
