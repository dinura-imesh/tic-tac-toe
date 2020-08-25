using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour , IUnityAdsListener
{

    public static AdManager instance;

    public static bool isAdsReady { get; private set; }

    public const string APP_ID = "3232178";
    public const string REWARDED_AD_ID = "rewardedVideo";
    public const string INTERSTITIAL_AD_ID = "interstitial";
    public const string VIDEO_AD_ID = "video";

    private void Awake()
    {
        instance = this;
        Advertisement.AddListener(this);
        Advertisement.Initialize(APP_ID , false);
        if (Advertisement.isInitialized)
            Debug.Log("Ads initialized!");
    }

    public static void ShowRewardedAd()
    {
        Advertisement.Show(REWARDED_AD_ID);
    }
    public static void ShowVideoAd()
    {
        if(Advertisement.IsReady(VIDEO_AD_ID))
            Advertisement.Show(VIDEO_AD_ID);
    }

    public void OnUnityAdsReady(string placementId)
    {
        isAdsReady = true;
    }

    public void OnUnityAdsDidError(string message)
    {
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if(placementId == REWARDED_AD_ID)
        {
            switch (showResult)
            {
                case ShowResult.Finished:
                    Debug.Log("Complete ad");
                    //reward
                    break;
                case ShowResult.Failed:
                    Debug.Log("Unkown Error while playing ad!");
                    //Toast.MakeText(EnumToast.Debug, "Unknown Error!");
                    break;
            }
        }
    }
}
