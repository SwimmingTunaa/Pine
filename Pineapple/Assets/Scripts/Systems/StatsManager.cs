using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsManager: MonoBehaviour
{
    public static StatsManager Instance;
    public IntVariable currentScore;
    public IntVariable regionVistedVar;
    public IntVariable seedlingsCollectedVar;
    public int stickerCollectedThisRound;
    public GameObject highscoreIcon;

    [Header("UI")]
    public GameObject scoreAddText;
    public Transform scoreSpawnPos;
    void Awake()
    {
        if (Instance != null) 
                Destroy(gameObject);
            else
                Instance = this;
        highscoreIcon.SetActive(false);
        regionVistedVar.RuntimeValue = 0;
    }

    public void AddStickersToTotalOwnedAmount()
    {
        int allStickers = PlayerPrefs.GetInt("TotalStickers") + stickerCollectedThisRound;
        PlayerPrefs.SetInt("TotalStickers", allStickers);
    }

    public void AddStickersToTotalOwnedAmount(int amount)
    {
        int allStickers = PlayerPrefs.GetInt("TotalStickers") + amount;
        Debug.Log(allStickers + " total stickers");
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
        currentScore.RuntimeValue += amount;
        GameObject tempGo = Instantiate(scoreAddText, scoreSpawnPos.position, scoreAddText.transform.rotation);
        tempGo.GetComponentInChildren<TextMeshProUGUI>().text = "+" + amount + " - " + scoreText;
        tempGo.transform.SetParent(scoreSpawnPos.transform);
        Destroy(tempGo, 3f);
    }

    public void getTotalScore()
    {
        currentScore.RuntimeValue  = ((int)GameManager.Instance.distanceVariable.RuntimeValue + (GameManager.Instance.stickersCollectedVar.RuntimeValue * 10) +
                                    (seedlingsCollectedVar.RuntimeValue * 100) + (regionVistedVar.RuntimeValue * 300));

        if(currentScore.RuntimeValue > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", currentScore.RuntimeValue);
            //show highscore icon
            highscoreIcon.SetActive(true);
            //set inactive in play button
        }
    }

    public void ResetStats()
    {
            PlayerPrefs.DeleteAll();
    }
}
