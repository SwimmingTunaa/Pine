using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeUseItemUpgrade : ShopItem
{
    public override void Initialise()
    {
        base.Initialise();
    }

    public override void IncreaseItemLevel()
    {
        base.IncreaseItemLevel();
        OneTimeUseObject o = (OneTimeUseObject)itemInstance;
        if(o.itemObject)
        {
            GameObject gameobjectInstance = Instantiate(o.itemObject);
            gameobjectInstance.SetActive(false);
            gameobjectInstance.transform.position = CharacterManager.activeCharacter.transform.position;
            ShopManager.Instance.otItems.Add(gameobjectInstance);
        }
    }
}
