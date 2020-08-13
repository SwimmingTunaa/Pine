using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamager : Damager
{
    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
         if(other.GetComponent<ObjectID>() != null && other.GetComponent<ObjectID>().objectType == ObjType.Obstacle)
        {
            if(!other.GetComponent<ObjectID>().selfDestroy)
                 other.GetComponent<ObjectID>().Disable();
                //Instantiate(killEffect, target.transform.position, killEffect.transform.rotation);
        }
    }
}
