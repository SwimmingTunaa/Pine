using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
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
    [Header("One time use items")]
    public List<GameObject> otItems = new List<GameObject>();

    public void Awake()
    {
        if (Instance != null) 
                Destroy(gameObject);
            else
                Instance = this;
        _confirmButtonText = confirmationBody.GetComponentInChildren<TextMeshProUGUI>();
    }
    public void OnEnable()
    {
 
        _currentText = _confirmButtonText.text;
    }

    public void OpenConfirmation(ShopItem shopItem)
    {
        if(shopItem.itemInstance.itemCost > 0 && (PlayerPrefs.GetInt("TotalStickers") - shopItem.itemInstance.itemCost >= 0) ||
            shopItem.itemInstance.seedlingCost > 0 && (PlayerPrefs.GetInt("Seedlings") - shopItem.itemInstance.seedlingCost >= 0))
        {
            confirmationBody.SetActive(true);
            //add the method to the onclick event
            confirmButton.onClick.AddListener(() => shopItem.IncreaseItemLevel());
            cancelButton.onClick.AddListener(()=> CloseConfirmation(shopItem.item.itemName));
            _confirmButtonText.text = _confirmButtonText.text.Replace("(ItemName)",shopItem.item.itemName);
            //converts the item cost to string and then add commas to the thousandth
            if(shopItem.itemInstance.itemCost > 0)
                _confirmButtonText.text = _confirmButtonText.text.Replace("(ItemCost)",shopItem.itemInstance.itemCost.ToString() + "<color=#B88CFD> Stickers?</color>");
            else if (shopItem.itemInstance.seedlingCost > 0)
                _confirmButtonText.text = _confirmButtonText.text.Replace("(ItemCost)",shopItem.itemInstance.seedlingCost.ToString() + "<color=#19df88> Seedlings?</color>");
        }
        else
            insufficientFundButton.SetActive(true);
    }

       public void OpenConfirmationCharacter(ShopItem shopItem)
    {
        if(PlayerPrefs.GetInt("TotalStickers") - shopItem.itemInstance.itemCost >= 0)
        {
            confirmationBody.SetActive(true);
            //add the method to the onclick event
            confirmButton.onClick.AddListener(() => shopItem.IncreaseItemLevel());
            cancelButton.onClick.AddListener(()=> CloseConfirmation(shopItem.item.itemName));
            _confirmButtonText.text = _confirmButtonText.text.Replace("(ItemName)",shopItem.item.itemName);
            //converts the item cost to string and then add commas to the thousandth
            _confirmButtonText.text = _confirmButtonText.text.Replace("(ItemCost)",shopItem.itemInstance.itemCost.ToString());
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

    public void ResetPlayerPrefsOnPlay()
    {
        //done in play button click
        //turn on any One time items that have been spawned
        foreach(GameObject g in otItems)
        {
            g.SetActive(true);
        }
        //reset the items to be avaible in shops again
        PlayerPrefs.SetInt("Canned Fruits", 0);     
        PlayerPrefs.SetInt("Super Fruit", 0);       
    }
}
