using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsManager: MonoBehaviour
{
    public int currentScore;
    public int stickerCollected;
    public int couchBounced;
    public int haircutsTaken;

    [Header("UI")]
    public GameObject scoreAddText;
    public Transform scoreSpawnPos;

    void Start()
    {
        PlayerPrefs.GetInt("CouchBounced");
        PlayerPrefs.GetInt("TotalStickers");
        PlayerPrefs.GetInt("StickersCollected");
        PlayerPrefs.GetInt("HighScore");
        PlayerPrefs.GetInt("LongestDistanceTravelled");
        PlayerPrefs.GetInt("HaircutsTaken");
    }

    public void AddStickersToTotalOwnedAmount()
    {
        int allStickers = PlayerPrefs.GetInt("TotalStickers") + stickerCollected;
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

    public static void AddToAStat(int amount, string statName)
    {
        int totalAmount = PlayerPrefs.GetInt(statName) + amount;
        PlayerPrefs.SetInt(statName, totalAmount);
        Debug.Log("Added 1 to " + statName + ". Total now = " + PlayerPrefs.GetInt(statName));
    }

    public void AddToScore(int amount, string scoreText)
    {
        currentScore += amount;
        GameObject tempGo = Instantiate(scoreAddText, scoreSpawnPos.position, scoreAddText.transform.rotation);
        tempGo.GetComponentInChildren<TextMeshProUGUI>().text = "+" + amount + " - " + scoreText;
        tempGo.transform.parent = scoreSpawnPos.transform;
        Destroy(tempGo, 3f);
    }
}
