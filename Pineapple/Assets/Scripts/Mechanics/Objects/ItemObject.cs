using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Default Item")]
public  class ItemObject : ScriptableObject
{
    [Header("About This Item")]
    public int itemID;
    public string itemName;
    public Sprite itemSprite;
    public AudioClip pickUpSound;
    public GameObject deathFX;
    

    public void Initialize(GameObject thisGameObject)
    {
        thisGameObject.GetComponentInChildren<SpriteRenderer>().sprite = itemSprite;
    }
}

