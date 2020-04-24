using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

[RequireComponent(typeof(AudioSource))]
public abstract class ShopItem : MonoBehaviour
{
    [Header("Base Item Info")]
    public ShopManager shopManager;
    public string itemName;
    public int itemCost;
    public int costIncrement;
    public List<PickUpsBase> itemInGame = new List<PickUpsBase>();

    [Header("Text")]
    public Slider progressBar;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI purchaseText;
    public TextMeshProUGUI priceText;
    public int currentLevel;

    private int _currentLevel{get{return currentLevel;} set{currentLevel = value;}}
    private int _startCost;
    
    
    void Awake()
    {
        PlayerPrefs.GetInt(itemName);
        //set the level of the item to its tracked level
        //_confirmButtonText = confirmationBody.GetComponentInChildren<TextMeshProUGUI>();
        //get the base price of the item
        _startCost = itemCost;
        Initialise();
    }

    public virtual void Initialise()
    {
         //set the level of the item to its tracked level
         if(PlayerPrefs.GetInt(itemName) <= 0)
         {
             PlayerPrefs.SetInt(itemName, 1);
         }
        _currentLevel = PlayerPrefs.GetInt(itemName);
        itemCost = _startCost * _currentLevel;  
        UpdateText();
        CompletedItem();
    }

    #region //old Code
    
    /*public void UpgradeTimer(PickUpsBase p)
    {
        IncreaseItemLevel(p);
        p.effectDuration = p._initialDuration + (timeToAdd * (_currentLevel)); 
        //change the tracked level of item up 1
        CloseConfirmation();
    }*/
   /* public void UpgradeShield(ShieldPickUp p)
    {
        //add one cause it starts at 0
        p.shieldStrength = _currentLevel == 1 ? 2 : _currentLevel + 1;
        IncreaseItemLevel(p);
        CloseConfirmation(); 
    }*/

    /*void OnClickEvent(PickUpsBase p)
    {
        if(timeToAdd <= 0)
            UpgradeShield((ShieldPickUp)p);
        else
            UpgradeTimer(p);
    }
    */
   /* public void OpenConfirmation()
    {
        if(PlayerPrefs.GetInt("TotalStickers") - itemCost >= 0)
        {
            foreach(PickUpsBase p in itemInGame)
            {
                if(p.itemCurrentLvl < p.itemMaxLevel)
                {
                    confirmationBody.SetActive(true);
                    //add the method to the onclick event
                    confirmButton.onClick.AddListener(() => OnClickEvent(p));
                    cancelButton.onClick.AddListener(()=> CloseConfirmation());
                    _confirmButtonText.text = _confirmButtonText.text.Replace("(ItemName)",itemName);
                    //converts the item cost to string and then add commas to the thousandth
                    _confirmButtonText.text = _costText;
                }
            }
        } else 
        {
            unableToPurchaseUI.SetActive(true);
        }
    }*/

   /* public void CloseConfirmation()
    {
        confirmationBody.SetActive(false);
        //reset the button to defualt
        confirmButton.onClick.RemoveAllListeners();
        _confirmButtonText.text = _confirmButtonText.text.Replace(itemName, ("(ItemName)"));
       // _confirmButtonText.text = _confirmButtonText.text.Replace(itemCost.ToString("N0"), ("(ItemCost)"));
       _confirmButtonText.text = _currentText;
    }*/
    #endregion

    void OnEnable()
    {
        //Debug.Log(itemName + " enabled");
        Initialise();
    }

    public virtual void IncreaseItemLevel()
    {  
        //pay first before level is increased then increase the price
        StatsManager.MinusStickers(itemCost);
        itemCost = _startCost + (_currentLevel * costIncrement);
        //increase level
        _currentLevel += 1;
        PlayerPrefs.SetInt(itemName, _currentLevel);
        Debug.Log(itemName + " current level: " + _currentLevel);
        //update text
        UpdateText();
        CompletedItem();
        shopManager.CloseConfirmation(itemName);
    }

    void CompletedItem()
    {
        if(_currentLevel >= itemInGame[0].itemMaxLevel)
        {
            purchaseText.text = "COMPLETED";
            GetComponentInChildren<Button>().interactable = false;
            priceText.text = "ITEM";          
        }
    }

    void UpdateText()
    {
        levelText.text = _currentLevel + "/" + itemInGame[0].itemMaxLevel;
        priceText.text = itemCost.ToString("N0");
        progressBar.maxValue = itemInGame[0].itemMaxLevel;
        progressBar.value = _currentLevel;
    }
}
