using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsManager: MonoBehaviour
{
    public static StatsManager Instance;
    public int currentScore;
    public int stickerCollectedThisRound;

    [Header("UI")]
    public GameObject scoreAddText;
    public Transform scoreSpawnPos;
    void Awake()
    {
        if (Instance != null) 
                Destroy(gameObject);
            else
                Instance = this;
    }

    public void AddStickersToTotalOwnedAmount()
    {
        int allStickers = PlayerPrefs.GetInt("TotalStickers") + stickerCollectedThisRound;
        PlayerPrefs.SetInt("TotalStickers", allStickers);
    }

    public void AddStickersToTotalOwnedAmount(int amount)
    {
        int allStickers = PlayerPrefs.GetInt("TotalStickers") + amount;
        PlayerPrefs.SetInt("TotalStickers", allStickers);
    }

    public void AddSeedlingsToTotalOwnedAmount(int amount)
    {
        int allStickers = PlayerPrefs.GetInt("Seedlings") + amount;
        PlayerPrefs.SetInt("Seedlings", allStickers);
    }


    public void MinusStickers(int amount)
    {
        int allStickers = PlayerPrefs.GetInt("TotalStickers") - amount;
        PlayerPrefs.SetInt("TotalStickers", allStickers);
    }

    public void MinusSeedlings(int amount)
    {
        int allSeedlings = PlayerPrefs.GetInt("Seedlings") - amount;
        PlayerPrefs.SetInt("Seedlings", allSeedlings);
    }
    public void UpdateMostStickersEverCollected()
    {
        if(stickerCollectedThisRound > PlayerPrefs.GetInt("StickersCollected"))
            PlayerPrefs.SetInt("StickersCollected", stickerCollectedThisRound);
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
    }
    public void AddOneToAStat(string statName)
    {
        int totalAmount = PlayerPrefs.GetInt(statName) + 1;
        PlayerPrefs.SetInt(statName, totalAmount);
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
            PlayerPrefs.DeleteAll();
    }
}
