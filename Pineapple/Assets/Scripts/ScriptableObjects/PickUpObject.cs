using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PickUp", menuName = "Item/Pick Up Item")]
[SerializeField]
public class PickUpObject : ScriptableObject
{
    [Header("About This Item")]
    public string itemName;
    public int itemCost;
    public int seedlingCost;
    public int costIncrement;
    public float effectDuration;
    public int itemMaxLevel = 1;
    public AudioClip pickUpSound;
    public GameObject deathFX;
    public PickUpObject Instance;

    public void Init()
    {
        Instance = Instantiate(this);
    }
}

