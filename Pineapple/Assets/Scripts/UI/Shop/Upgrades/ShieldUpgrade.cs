using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldUpgrade : ShopItem
{
    [Header("Shield")]
    public float strengthToAdd;
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
        foreach(ShieldPickUp p in itemInGame)
        {
            p.shieldStrength = currentLevel + strengthToAdd; 
        }
    }
}
