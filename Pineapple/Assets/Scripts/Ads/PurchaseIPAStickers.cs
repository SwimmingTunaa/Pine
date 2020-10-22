using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PurchaseIPAStickers : MonoBehaviour
{
    public bool buy1000Stickers;
    public string itemName;
    public float itemPrice;
    public TextMeshProUGUI priceText;
 
    public void BuyItem()
    {
        if(buy1000Stickers)
        {
            IAPManager.instance.BuyStickers1000();
        }
        else   
        {
            IAPManager.instance.BuyStickers2500();
        }
    }

    IEnumerator SetPriceText()
    {
        while(!IAPManager.instance.IsInitialized())
            yield return null;
        print("text changed");
        if(buy1000Stickers)
            priceText.text = IAPManager.instance.GetProductPriceFromStore(IAPManager.instance.stickers2500);
        else
            priceText.text = IAPManager.instance.GetProductPriceFromStore(IAPManager.instance.stickers1000);
    }
}
