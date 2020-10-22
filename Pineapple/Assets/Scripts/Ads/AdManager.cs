using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    public static AdManager instance;
    [Header("Watch Ad")]
    public float playAmountToTrigger = 1;
    public GameObject watchAdButton;
    public GameObject gameOverAdButton;
    public GameObject rewardPanel;
    [Header("Recieve Reward")]
    public GameObject receiveRewardButton;
    private string startText;

    [Header("IAP")]
    public GameObject parentContainer;
    public TextMeshProUGUI descriptionText;
    public GameObject confirmationContainer;
    public Button confirmationButton;
    public Button cancelButton;
    private string _currentText;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        startText = receiveRewardButton.GetComponentInChildren<TextMeshProUGUI>().text;
    }

    void Start()
    {
        _currentText = descriptionText.text;
    }

    //keep count of how many times players press play;
    public void ShowOptInAdButton()
    {
        gameOverAdButton.SetActive(true);
        if(PlayerPrefs.GetInt("Play Count") >= playAmountToTrigger)
        {
            watchAdButton.SetActive(true);
            PlayerPrefs.SetInt("Play Count",0);
        }
    }

    public void ShowStickersReceived(int amount)
    {
        receiveRewardButton.SetActive(true);
        TextMeshProUGUI tm = receiveRewardButton.GetComponentInChildren<TextMeshProUGUI>();
        tm.text = startText;
        tm.text = tm.text.Replace("(amount)", amount.ToString("N0"));
    }

    public void OpenConfirmation(PurchaseIPAStickers IAPItem)
    {
       descriptionText.text = _currentText;
        parentContainer.gameObject.SetActive(true);
        //add the method to the onclick event
        confirmationButton.onClick.AddListener(() => IAPItem.BuyItem());
        cancelButton.onClick.AddListener(()=> CloseConfirmation(IAPItem.itemName));
        descriptionText.text = descriptionText.text.Replace("(ItemName)",IAPItem.itemName);
        //converts the item cost to string and then add commas to the thousandth
        descriptionText.text = descriptionText.text.Replace("(ItemCost)","$" +IAPItem.itemPrice.ToString());
    }

    public void CloseConfirmation(string itemName)
    {
        //reset the button to defualt
        //this will remove the sound effect too TODO: Fix this
        confirmationButton.onClick.RemoveAllListeners();
        descriptionText.text = descriptionText.text.Replace(itemName, ("(ItemName)"));
       // _confirmButtonText.text = _confirmButtonText.text.Replace(itemCost.ToString("N0"), ("(ItemCost)"));
        parentContainer.gameObject.SetActive(false);
    }
}
