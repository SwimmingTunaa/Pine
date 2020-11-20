using System;
using UnityEngine;
using UnityEngine.Purchasing;


public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager instance;
    public GameObject confirmedPurchaseButton1000;
    public GameObject confirmedPurchaseButton2500;
    public static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    //Step 1 create your products
    const string stickers1000 = "tsd_stickers_1000";
    const string stickers2500 = "tsd_stickers_2500";


    //************************** Adjust these methods **************************************
    public void InitializePurchasing()
    {
        if (IsInitialized()) { return; }
        /*var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Step 2 choose if your product is a consumable or non consumable
        builder.AddProduct(stickers1000, ProductType.NonConsumable);
        builder.AddProduct(stickers2500, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);*/
    }


    public bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    //Step 3 Create methods

    public void BuyStickers1000()
    {
        BuyProductID(stickers1000);
    }

    public void BuyStickers2500()
    {
        BuyProductID("tsd_stickers_2500");
    }

    public string GetProductPriceFromStore(string id)
    {
        if(m_StoreController != null && m_StoreController.products != null)
            return  m_StoreController.products.WithID(id).metadata.localizedPrice.ToString();
        else 
            return "";
    }


    //Step 4 modify purchasing
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log(args.purchasedProduct.definition.id);
        if (args.purchasedProduct.definition.id == stickers1000)
        {
            StatsManager.Instance.AddStickersToTotalOwnedAmount(1000);
            confirmedPurchaseButton1000.SetActive(true);
            Debug.Log("1000 stickers bought sucessfully");
        }
        else
        if (args.purchasedProduct.definition.id == stickers2500)
        {
            StatsManager.Instance.AddStickersToTotalOwnedAmount(2500);
            confirmedPurchaseButton2500.SetActive(true);
            Debug.Log("2500 stickers bought sucessfully");
        }
        else
        {
            Debug.Log("Purchase Failed");
        }

        return PurchaseProcessingResult.Complete;
    }










    //**************************** Dont worry about these methods ***********************************
    private void Awake()
    {
        TestSingleton();
    }

    void Start()
    {
        if (m_StoreController == null) { InitializePurchasing(); }
    }

    private void TestSingleton()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
               
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            /*var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });*/
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}