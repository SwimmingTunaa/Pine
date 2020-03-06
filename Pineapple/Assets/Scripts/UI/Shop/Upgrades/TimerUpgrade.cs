using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerUpgrade : ShopItem
{
    [Header("Timer")]
    public float timeToAdd;

    public override void Initialise()
    {
        base.Initialise();
        foreach(PickUpsBase p in itemInGame)
        {
            p.effectDuration = p._initialDuration + (timeToAdd * currentLevel);
            UpgradeTimer();
        }
    }

    public override void IncreaseItemLevel()
    {
        base.IncreaseItemLevel();
        UpgradeTimer();
    }

    public void UpgradeTimer()
    {
        foreach(PickUpsBase p in itemInGame)
        {
            p.effectDuration = p._initialDuration + (timeToAdd * currentLevel);
        }
    }
}
