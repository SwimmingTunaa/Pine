using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldUpgrade : ShopItem
{
    [Header("Shield")]
    public int strengthToAdd;
    public override void Initialise()
    {
        base.Initialise();
        UpgradeShield();
    }

    public override void IncreaseItemLevel()
    {
        UpgradeShield();
        base.IncreaseItemLevel();
    }

    public void UpgradeShield()
    {
        ShieldPickUpObject s = itemInstance as ShieldPickUpObject;
        s.shieldStrength = currentLevel + strengthToAdd; 
        Debug.Log(s.shieldStrength);
    }
}
