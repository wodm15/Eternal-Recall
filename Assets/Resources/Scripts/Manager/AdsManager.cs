using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdsManager
{
    InterstitialAd _interstitialAd;
    RewardedAd _rewardedAd;

    // 광고 유닛 ID 설정 (보상형 광고, 인터스티셜 광고 각각의 ID 사용)
    private string _adUnitId = "ca-app-pub-1071815426479027/5104078510"; // Rewarded Ad
    private string _interstitialAdUnitId = "ca-app-pub-1071815426479027/6170410886";  // 인터스티얼 광고 ID

    // 광고 초기화 메서드
    public void Init()
    {
        #if UNITY_EDITOR
        {
            _adUnitId = "ca-app-pub-3940256099942544/5224354917"; // Google의 기본 테스트 ID (Rewarded Ad)
            _interstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";  // Google의 기본 테스트 ID (Interstitial Ad)
            LoadInterstitialAd(); // 인터스티얼 광고 로드
            LoadRewardedAd(); // 보상형 광고 로드
            Debug.Log("Unity Editor Ads: Using test ad units.");
        }

        #elif UNITY_ANDROID
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("Google Mobile Ads SDK Initialized.");
            LoadInterstitialAd(); // 인터스티얼 광고 로드
            LoadRewardedAd(); // 보상형 광고 로드
        });
        #else
        Debug.Log("Google Mobile Ads SDK is only initialized on Android.");
        #endif
    }

    // 인터스티얼 광고 로드 메서드
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
        InterstitialAd.Load(_interstitialAdUnitId, adRequest,
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

    // 인터스티얼 광고 이벤트 핸들러 등록 메서드
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

    // 인터스티얼 광고 표시 메서드
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

    // 보상형 광고 이벤트 핸들러 등록 메서드
    private void RegisterEventHandlers(RewardedAd ad)
    {
        // 광고 수익 기록
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
        };
        // 광고 인상 기록
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // 광고 클릭 기록
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // 광고가 전체 화면을 연 경우
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // 광고가 닫혔을 때
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // 광고 실패
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content with error: " + error);
        };
    }

    // 보상형 광고 로드 메서드
    public void LoadRewardedAd()
    {
        // 이전 광고 정리
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        AdRequest adRequest = new AdRequest();

        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load with error: " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded.");
                _rewardedAd = ad;
            });
    }

    // 보상형 광고 표시 메서드
    public void ShowRewardedAd()
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                Debug.Log(string.Format("Rewarded ad rewarded the user. Type: {0}, amount: {1}.", reward.Type, reward.Amount));
            });
        }
    }
}
