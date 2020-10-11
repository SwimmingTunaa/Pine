using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamager : Damager
{
    public ObjectPools killEffect;
    int layersToDestroy;
    public override void Awake()
    {
        base.Awake();
        //converts it to bit value
        layersToDestroy = layerMask.value;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        //check bit value agaisnt the target object
        if(layersToDestroy == (layerMask | (1 << other.gameObject.layer)))
        {
            if(other.GetComponent<HealthGeneric>())
            {
                other.GetComponent<HealthGeneric>().TakeDamage(DamageAmount);

            }
            else
            {
                if(other.GetComponent<ObjectID>() && !other.GetComponent<ObjectID>().selfDestroy)
                    other.GetComponent<ObjectID>().Disable();
                else if (!other.GetComponent<ObjectID>())
                    other.gameObject.SetActive(false);
                GameObject effect = killEffect.GetNextItem();
                effect.SetActive(true);
                effect.transform.position = other.gameObject.transform.position;
            }
        }
    }
}
