using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AdManager : MonoBehaviour
{
    public static AdManager instance;
    [Header("Watch Ad")]
    public float playAmountToTrigger = 3;
    public GameObject watchAdButton;
    public GameObject gameOverAdButton;
    [Header("Recieve Reward")]
    public GameObject receiveRewardButton;
    private string startText;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        startText = receiveRewardButton.GetComponentInChildren<TextMeshProUGUI>().text;
    }

    //keep count of how many times players press play;
    public void ShowOptInAdButton()
    {
        if(PlayerPrefs.GetInt("Play Count") >= playAmountToTrigger)
        {
            watchAdButton.SetActive(true);
        }
    }

    public void ShowStickersReceived(int amount)
    {
        receiveRewardButton.SetActive(true);
        TextMeshProUGUI tm = receiveRewardButton.GetComponentInChildren<TextMeshProUGUI>();
        tm.text = startText;
        tm.text = tm.text.Replace("(amount)", amount.ToString("N0"));
    }
}
