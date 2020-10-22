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
    public PickUpObject item;
    [HideInInspector] public PickUpObject itemInstance;

    [Header("Text")]
    public bool showSeedlingPrice;
    public SetBar progressBar;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI purchaseText;
    public TextMeshProUGUI priceText;
    public int currentLevel;

    private int _currentLevel{get{return currentLevel;} set{currentLevel = value;}}
    private int _startCost;
    
    
    void Awake()
    {
        item.Init();
        if(item)
            itemInstance = item.Instance;
        //PlayerPrefs.GetInt(item.itemName);
        //set the level of the item to its tracked level
        //_confirmButtonText = confirmationBody.GetComponentInChildren<TextMeshProUGUI>();
        //get the base price of the item
        _startCost = itemInstance.itemCost;
        if(progressBar)
            progressBar.MaxLevelPoints = itemInstance.itemMaxLevel;
        //Initialise();
    }

    public virtual void Initialise()
    {
        //set the level of the item to its tracked level
        if(PlayerPrefs.GetInt(itemInstance.itemName) <= 0 && item.itemMaxLevel > 1)
        {
            PlayerPrefs.SetInt(itemInstance.itemName, 1);
        }
        _currentLevel = PlayerPrefs.GetInt(itemInstance.itemName);
        itemInstance.itemCost = _startCost + (itemInstance.itemMaxLevel > 1 ? (_currentLevel == 1 ? 0 : (_currentLevel - 1)* itemInstance.costIncrement) : 0);  
        UpdateText();
        CompletedItem();
    }

    void OnEnable()
    {
        Initialise();
    }

    public virtual void IncreaseItemLevel()
    {  
        //pay first before level is increased then increase the price
        StatsManager.Instance.MinusStickers(itemInstance.itemCost);
        if(itemInstance.seedlingCost > 0)
            StatsManager.Instance.MinusSeedlings(itemInstance.seedlingCost);
        //increase level
        if(item.itemMaxLevel > 0)
        _currentLevel += 1;
        itemInstance.itemCost = _startCost + (itemInstance.itemMaxLevel > 1 ? (_currentLevel == 1 ? 0 : (_currentLevel -1)* itemInstance.costIncrement) : 0);
        PlayerPrefs.SetInt(itemInstance.itemName, _currentLevel);
        Debug.Log(itemInstance.itemName + " current level: " + _currentLevel);
        //update text
        UpdateText();
        CompletedItem();
        ShopManager.Instance.CloseConfirmation(itemInstance.itemName);
    }

    void CompletedItem()
    {
        if(_currentLevel >= itemInstance.itemMaxLevel)
        {
            purchaseText.text = "COMPLETED";
            GetComponentInChildren<Button>().interactable = false;
            priceText.text = "ITEM";          
        }
    }

    void UpdateText()
    {
        if(levelText)
            levelText.text = _currentLevel + "/" + itemInstance.itemMaxLevel;
        if(priceText)
        {
            if(!showSeedlingPrice)
                priceText.text = itemInstance.itemCost.ToString("N0");
            else if(showSeedlingPrice)
                priceText.text = itemInstance.seedlingCost.ToString("N0");
        }
        progressBar.Level = _currentLevel;
    }
}
