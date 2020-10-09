using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

[RequireComponent (typeof (Button))]
public class RewardedAdsButton : MonoBehaviour, IUnityAdsListener 
{
    public int stickerRewardAmount = 100;

    #if UNITY_IOS
    private string gameId = "3845530";
    #elif UNITY_ANDROID
    private string gameId = "3845531";
    #endif

    Button myButton;
    public string myPlacementId = "rewardedVideo";
    public GameObject parentContainer;

    void Start () 
    {   
        myButton = GetComponent <Button> ();

        // Set interactivity to be dependent on the Placement’s status:
        myButton.interactable = Advertisement.IsReady (myPlacementId); 

        // Map the ShowRewardedVideo function to the button’s click listener:
        if (myButton) myButton.onClick.AddListener (ShowRewardedVideo);

        // Initialize the Ads listener and service:
        Advertisement.AddListener (this);
        Advertisement.Initialize (gameId, true);
    }

    // Implement a function for showing a rewarded video ad:
    void ShowRewardedVideo () 
    {
        Advertisement.Show (myPlacementId);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady (string placementId)
    {
        // If the ready Placement is rewarded, activate the button: 
        if (placementId == myPlacementId) {        
            myButton.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish (string placementId, ShowResult showResult) 
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished) 
        {
            // Reward the user for watching the ad to completion.
            PlayerPrefs.SetInt("TotalStickers", PlayerPrefs.GetInt("TotalStickers") + stickerRewardAmount);
            if(parentContainer) parentContainer.SetActive(false);
            AdManager.instance.ShowStickersReceived(stickerRewardAmount);
            AdManager.instance.gameOverAdButton.SetActive(false);
            Debug.Log(PlayerPrefs.GetInt("TotalStickers") + " + " + stickerRewardAmount + " = " + (PlayerPrefs.GetInt("TotalStickers") + stickerRewardAmount));
        } else if (showResult == ShowResult.Skipped) 
        {
            // Do not reward the user for skipping the ad.
        } else if (showResult == ShowResult.Failed) 
        {
            Debug.LogWarning ("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsDidError (string message) {
        // Log the error.
    }

    public void OnUnityAdsDidStart (string placementId) {
        // Optional actions to take when the end-users triggers an ad.
    } 
}