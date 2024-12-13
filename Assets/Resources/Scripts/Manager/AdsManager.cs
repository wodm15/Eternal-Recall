using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdsManager
{
    InterstitialAd _interstitialAd;
    RewardedAd _rewardedAd;


	// #if UNITY_ANDROID
	private string _adUnitId = "ca-app-pub-3940256099942544/1033173712";

    
    
	// 	#elif UNITY_IPHONE
	// private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
	// 	#else
	// // private string _adUnitId = "unused";
	// 	#endif

    // 광고 초기화 메서드
    public void Init()
    {
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("Google Mobile Ads SDK Initialized.");
            LoadInterstitialAd(); // 초기화 후 인터스티셜 광고 로드
            LoadRewardedAd();
        });
    
    }

    // 인터스티셜 광고 로드 메서드
    public void LoadInterstitialAd()
    {
        // 이전에 로드된 광고 정리
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

		AdRequest adRequest = new AdRequest();

        // 광고 로드 요청
        InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Interstitial ad failed to load with error: " + error);
                    return;
                }

                // 광고 로드 성공
                Debug.Log("Interstitial ad loaded.");
                _interstitialAd = ad;

                // 광고 이벤트 핸들러 등록
                RegisterEventHandlers(_interstitialAd);
            });
    }

    // 인터스티셜 광고 이벤트 핸들러 등록 메서드
    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(string.Format("Interstitial ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
        };

        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };

        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };

        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };

        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            // 광고가 닫히면 새로 로드
            LoadInterstitialAd();
        };

        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content with error: " + error);
        };
    }

    // 인터스티셜 광고 표시 메서드
    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }


    
    private void RegisterEventHandlers(RewardedAd ad)
{
    // Raised when the ad is estimated to have earned money.
    ad.OnAdPaid += (AdValue adValue) =>
    {
        Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
            adValue.Value,
            adValue.CurrencyCode));
    };
    // Raised when an impression is recorded for an ad.
    ad.OnAdImpressionRecorded += () =>
    {
        Debug.Log("Rewarded ad recorded an impression.");
    };
    // Raised when a click is recorded for an ad.
    ad.OnAdClicked += () =>
    {
        Debug.Log("Rewarded ad was clicked.");
    };
    // Raised when an ad opened full screen content.
    ad.OnAdFullScreenContentOpened += () =>
    {
        Debug.Log("Rewarded ad full screen content opened.");
    };
    // Raised when the ad closed full screen content.
    ad.OnAdFullScreenContentClosed += () =>
    {
        Debug.Log("Rewarded ad full screen content closed.");
    };
    // Raised when the ad failed to open full screen content.
    ad.OnAdFullScreenContentFailed += (AdError error) =>
    {
        Debug.LogError("Rewarded ad failed to open full screen content " +
                       "with error : " + error);
    };
}
    public void LoadRewardedAd()
  {
      // Clean up the old ad before loading a new one.
      if (_rewardedAd != null)
      {
            _rewardedAd.Destroy();
            _rewardedAd = null;
      }

      Debug.Log("Loading the rewarded ad.");

      // create our request used to load the ad.
      var adRequest = new AdRequest();

      // send the request to load the ad.
      RewardedAd.Load(_adUnitId, adRequest,
          (RewardedAd ad, LoadAdError error) =>
          {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
              {
                  Debug.LogError("Rewarded ad failed to load an ad " +
                                 "with error : " + error);
                  return;
              }

              Debug.Log("Rewarded ad loaded with response : "
                        + ad.GetResponseInfo());

              _rewardedAd = ad;
          });
  }

    public void ShowRewardedAd()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }
}
