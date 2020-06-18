using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharacterBuy : ShopItem
{
    [Header("Character Info")]
    public int itemCost;
    public CharacterManager characterManager;
    public GameObject characterPrefab;
    public bool equipped;
    public int characterOwned;
    public GameObject equipButton;
    public GameObject purchaseButton;

    private string itemAlreadyPurchased;

    public override void Initialise()
    {
        itemAlreadyPurchased = item.name + " purchased";
        if(characterOwned == 0)
            characterOwned = PlayerPrefs.GetInt(itemAlreadyPurchased);
        if(characterOwned > 0)
        {
            purchaseButton.SetActive(false);
            equipButton.SetActive(true);
        }    
    }

    public override void IncreaseItemLevel()
    {
        StatsManager.Instance.MinusStickers(item.itemCost);
        ShopManager.Instance.CloseConfirmation(item.name);
        characterManager.moveToSpawnPos(characterPrefab, characterManager.spawnPos);
        PlayerPrefs.SetInt(itemAlreadyPurchased, 1);
        characterOwned = PlayerPrefs.GetInt(itemAlreadyPurchased);
        purchaseButton.SetActive(false);
        equipButton.SetActive(true);
    }
}
