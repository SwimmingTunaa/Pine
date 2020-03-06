using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Confirmation Button")]
    public GameObject confirmationBody;
    public Button confirmButton;
    public Button cancelButton;
    public GameObject unableToPurchaseUI;
    private string _costText;
    private string _currentText;
    private TextMeshProUGUI _confirmButtonText;

    [Header("Insufficient Funds Button")]
    public GameObject insufficientFundButton;

    public void Awake()
    {
        _confirmButtonText = confirmationBody.GetComponentInChildren<TextMeshProUGUI>();
    }
    public void Start()
    {
 
        _currentText = _confirmButtonText.text;
    }

    public void OpenConfirmation(ShopItem shopItem)
    {
        if(PlayerPrefs.GetInt("TotalStickers") - shopItem.itemCost >= 0)
        {
            confirmationBody.SetActive(true);
            //add the method to the onclick event
            confirmButton.onClick.AddListener(() => shopItem.IncreaseItemLevel());
            cancelButton.onClick.AddListener(()=> CloseConfirmation(shopItem.itemName));
            _confirmButtonText.text = _confirmButtonText.text.Replace("(ItemName)",shopItem.itemName);
            //converts the item cost to string and then add commas to the thousandth
            _confirmButtonText.text = _confirmButtonText.text.Replace("(ItemCost)",shopItem.itemCost.ToString());
        }
        else
            insufficientFundButton.SetActive(true);
    }

    public void CloseConfirmation(string itemName)
    {
        confirmationBody.SetActive(false);
        //reset the button to defualt
        confirmButton.onClick.RemoveAllListeners();
        _confirmButtonText.text = _confirmButtonText.text.Replace(itemName, ("(ItemName)"));
       // _confirmButtonText.text = _confirmButtonText.text.Replace(itemCost.ToString("N0"), ("(ItemCost)"));
       _confirmButtonText.text = _currentText;
    }
}
