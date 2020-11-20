using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRewardsManager : MonoBehaviour
{

    
    public GameObject dailyRewardAvailbleEffect;
    void Start()
    {
        GleyDailyRewards.Calendar.AddClickListener(CalandarButtonClicked);
       //Invoke("ShowCalendar", .5f);
    }

    private void CalandarButtonClicked(int dayNumber, int rewardValue, Sprite rewardSprite)
    {
        PlayerPrefs.SetInt("TotalStickers", PlayerPrefs.GetInt("TotalStickers") + rewardValue);
    }

    public void ShowCalendar()
    {
        GleyDailyRewards.Calendar.Show();
    }

    void Update()
    {

    }
}
