using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerUpgrade : ShopItem
{
    [Header("Timer")]
    public float timeToAdd;

    private float _initialDuration;

    public override void Initialise()
    {
        base.Initialise();
        _initialDuration = itemInstance.effectDuration;
        if(currentLevel > 1)
            itemInstance.effectDuration = _initialDuration + (timeToAdd * currentLevel);
    }

    public override void IncreaseItemLevel()
    {
        base.IncreaseItemLevel();
        UpgradeTimer();
    }

    public void UpgradeTimer()
    {
        itemInstance.effectDuration = _initialDuration + (timeToAdd * currentLevel);
    }
}
