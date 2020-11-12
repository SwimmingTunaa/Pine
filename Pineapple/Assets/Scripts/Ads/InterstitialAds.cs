using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : MonoBehaviour, IUnityAdsListener
{
    #if UNITY_IOS
    private string gameId = "3845530";
    #elif UNITY_ANDROID
    private string gameId = "3845531";
    #endif
    public static InterstitialAds instance;
    private string myPlacementId = "video";
    private bool testMode = false;

    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize (gameId, testMode);
    }

    public void ShowInterstitialAd() 
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady(myPlacementId)) {
            Advertisement.Show(myPlacementId);
            AdManager.instance.gameOverAdButton.GetComponent<RewardedAdsButton>().enabled = false;
        } 
        else {
            Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
        }
    }


    public void OnUnityAdsDidError(string message)
    {}
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished) 
        {
            AdManager.instance.gameOverAdButton.GetComponent<RewardedAdsButton>().enabled = true;
        } else if (showResult == ShowResult.Skipped) 
        {
            AdManager.instance.gameOverAdButton.GetComponent<RewardedAdsButton>().enabled = true;
        // Do not reward the user for skipping the ad.
        } else if (showResult == ShowResult.Failed) 
        {
            AdManager.instance.gameOverAdButton.GetComponent<RewardedAdsButton>().enabled = true;
            Debug.LogWarning ("The ad did not finish due to an error.");
        }
    }
    public void OnUnityAdsDidStart(string placementId)
    {}
    public  void OnUnityAdsReady(string placementId)
    {}
    public void DisplayVideoAd()
    {
        Advertisement.Show(myPlacementId);
    }

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy() 
    {
        Advertisement.RemoveListener(this);
    }
}
