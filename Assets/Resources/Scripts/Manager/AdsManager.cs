using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdsManager
{
    InterstitialAd _interstitialAd;

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
}
