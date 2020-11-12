using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using TMPro;
public class PurchaseIPAStickers : MonoBehaviour
{
    public bool buy1000Stickers;
    public string itemName;
    public float itemPrice;
    public TextMeshProUGUI priceText;

    void Start()
    {
        StartCoroutine(SetPriceText());
    }
 
    public void BuyItem()
    {
        if(buy1000Stickers)
        {
            IAPManager.instance.BuyStickers1000();
        }
        else if(!buy1000Stickers)   
        {
            IAPManager.instance.BuyStickers2500();
        }
    }

    IEnumerator SetPriceText()
    {
        while(!IAPManager.instance.IsInitialized())
            yield return null;
        
        if(buy1000Stickers)
        {
            Product product = IAPManager.m_StoreController.products.WithID("tsd_stickers_1000");
            priceText.text = product.metadata.localizedPriceString + " " + product.metadata.isoCurrencyCode.ToString();
        }
        else
        {
            Product product = IAPManager.m_StoreController.products.WithID("tsd_stickers_2500");
            priceText.text = product.metadata.localizedPriceString + " " + product.metadata.isoCurrencyCode.ToString();
        }
    }
}
