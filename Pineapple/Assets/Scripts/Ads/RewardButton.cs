using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class RewardButton : MonoBehaviour
{
    public string timerName;
    public float cooldownTime;
    public TextMeshProUGUI timerText;
    private Button button;
    private TimeSpan timeLeft;
    private long cooldown;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(setCooldownTimer);
    }

    void OnDestroy()
    {
        button.onClick.RemoveListener(setCooldownTimer);
    }
 

    void Update()
    {
        if (PlayerPrefs.HasKey (timerName)) 
            UpdateTimer();
    }

    void UpdateTimer()
    {
        //update Timer text
        cooldown = System.Convert.ToInt64(PlayerPrefs.GetString(timerName));
        timeLeft = DateTime.FromBinary (cooldown).Subtract (System.DateTime.Now);
        timerText.text =timeLeft.Minutes.ToString ("D2")+"m " + timeLeft.Seconds.ToString ("D2")+"s";
        if (timeLeft.TotalSeconds < 0) 
        {  
            button.interactable = true;
            timerText.gameObject.SetActive(false);
        }
        else
            {
                timerText.gameObject.SetActive(true);
                button.interactable = false;
            }
    }

    public void setCooldownTimer()
    {
        var timeWhenCooldownFinishes = System.DateTime.Now.AddMinutes(cooldownTime);
        string dataString = timeWhenCooldownFinishes.ToBinary().ToString();
        PlayerPrefs.SetString ( timerName, dataString );
        PlayerPrefs.Save ();
        button.interactable = false;
        timerText.gameObject.SetActive(true);
    }
}
