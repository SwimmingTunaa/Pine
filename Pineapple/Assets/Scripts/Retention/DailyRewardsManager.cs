using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRewardsManager : MonoBehaviour
{
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

    public void GameServicesLogin()
    {
        if(!GameServices.Instance.IsLoggedIn())
            GameServices.Instance.LogIn(LoginComplete);
            else
                GameServices.Instance.ShowLeaderboadsUI();
    }

    private void LoginComplete (bool success)
    {
        if(success==true)
        {
           GameServices.Instance.ShowLeaderboadsUI();
        }
        else
        {
            //Login failed
        }
    }

    void Update()
    {

    }
}
