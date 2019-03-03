using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<ItemObject> inventoryItems = new List<ItemObject>();
    public List<Image> inventorySpaces = new List<Image>();

    void Awake()
    {
        ResetInvetory();
    }

    public void AddItem(ItemObject itemToAdd)
    {
        //TODO: could do a check to see if the same item is not already inventory using itemObject.ID
        for(int i = 0; i < inventorySpaces.Count; i++)
        {
            if(!inventorySpaces[i].enabled)
            {
                inventoryItems.Add(itemToAdd);
                inventorySpaces[i].sprite = itemToAdd.itemSprite;
                inventorySpaces[i].enabled = true;
                return;
            }
        }
    }

    public void RemoveItem(ItemObject itemToRemove)
    {
        for(int i = 0; i < inventorySpaces.Count; i++)
        {
            print(i);
            if(inventorySpaces[i].enabled && inventoryItems.Contains(itemToRemove))
            {
                inventoryItems.Remove(itemToRemove);
                inventorySpaces[i].enabled = false;
                return;
            }
        } 
    }

    public void ResetInvetory()
    {
        foreach(Image i in inventorySpaces)
        {
            i.sprite = null;
            i.enabled = false;
        }
        inventoryItems.Clear();
    }
}
