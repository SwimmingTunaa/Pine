using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsManager: MonoBehaviour
{
    public static StatsManager Instance;
    public int currentScore;
    public int stickerCollected;

    [Header("UI")]
    public GameObject scoreAddText;
    public Transform scoreSpawnPos;
    public TextMeshProUGUI furthestTravelled;
    public TextMeshProUGUI mostStickersCollected;
    public TextMeshProUGUI hairCuts;
    public TextMeshProUGUI killByFans;
    public TextMeshProUGUI CouchBounce;
    public TextMeshProUGUI mouseStomped;
    public TextMeshProUGUI spidersSquished;
    void Awake()
    {
        if (Instance != null) 
                Destroy(gameObject);
            else
                Instance = this;
        Initialise();
    }

    public void Initialise()
    {
        furthestTravelled.text = PlayerPrefs.GetInt("LongestDistanceTravelled").ToString() + "m";
        mostStickersCollected.text = PlayerPrefs.GetInt("StickersCollected").ToString();
        hairCuts.text = PlayerPrefs.GetInt("HaircutsTaken").ToString();
        killByFans.text = PlayerPrefs.GetInt("KilledByFans").ToString();
        CouchBounce.text = PlayerPrefs.GetInt("CouchBounce").ToString();
        mouseStomped.text = PlayerPrefs.GetInt("Mouse Killed").ToString();
        spidersSquished.text = PlayerPrefs.GetInt("Spider Killed").ToString();
    }

    public void AddStickersToTotalOwnedAmount()
    {
        int allStickers = PlayerPrefs.GetInt("TotalStickers") + stickerCollected;
        PlayerPrefs.SetInt("TotalStickers", allStickers);
    }

    public void AddStickersToTotalOwnedAmount(int amount)
    {
        int allStickers = PlayerPrefs.GetInt("TotalStickers") + amount;
        PlayerPrefs.SetInt("TotalStickers", allStickers);
    }

    public void MinusStickers(int amount)
    {
        int allStickers = PlayerPrefs.GetInt("TotalStickers") - amount;
        PlayerPrefs.SetInt("TotalStickers", allStickers);
    }
    public void AddStickersToTotalEverCollected()
    {
        if(stickerCollected > PlayerPrefs.GetInt("StickersCollected"))
            PlayerPrefs.SetInt("StickersCollected", stickerCollected);
    }
    
    public void UpdateFurthestDistanceTravelled()
    {
        if(Statics.DistanceTraveled  > PlayerPrefs.GetInt("LongestDistanceTravelled"))
            PlayerPrefs.SetInt("LongestDistanceTravelled", (int)Statics.DistanceTraveled );
    }

    public void AddToAStat(int amount, string statName)
    {
        int totalAmount = PlayerPrefs.GetInt(statName) + amount;
        PlayerPrefs.SetInt(statName, totalAmount);
        //Debug.Log("Added 1 to " + statName + ". Total now = " + PlayerPrefs.GetInt(statName));
    }

    public void AddToScore(int amount, string scoreText)
    {
        currentScore += amount;
        GameObject tempGo = Instantiate(scoreAddText, scoreSpawnPos.position, scoreAddText.transform.rotation);
        tempGo.GetComponentInChildren<TextMeshProUGUI>().text = "+" + amount + " - " + scoreText;
        tempGo.transform.SetParent(scoreSpawnPos.transform);
        Destroy(tempGo, 3f);
    }

     public void ResetStats()
   {
       /*PlayerPrefs.DeleteKey("HighScore");
       PlayerPrefs.DeleteKey("StickersCollected");
       //PlayerPrefs.DeleteKey("FirstTimeStart");
       foreach(string s in trackedItems)
            PlayerPrefs.DeleteKey(s);*/
        PlayerPrefs.DeleteAll();
   }
}
