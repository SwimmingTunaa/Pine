using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Outfits: MonoBehaviour
{
    public GameObject deathEffect;
    public Transform pickUpSpawnPoint;
    [Header("Hair")]
    public GameObject slicedHairMask;

    [Header("Outfits")]
    public Transform glovePunchSlot;
    public GameObject puncherOutFit; 
    public GameObject boostOutFit;

    public void toggleOutfit(GameObject outfit, bool toggle)
    {
        outfit.SetActive(toggle);        
    }
}
