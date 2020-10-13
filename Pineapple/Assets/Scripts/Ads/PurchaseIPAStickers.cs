using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseIPAStickers : MonoBehaviour
{
    public bool buy1000Stickers;
    public string itemName;
    public float itemPrice;
    public void BuyItem()
    {
        if(buy1000Stickers)
            IAPManager.instance.BuyStickers1000();
        else   
            IAPManager.instance.BuyStickers400();

    }
}
