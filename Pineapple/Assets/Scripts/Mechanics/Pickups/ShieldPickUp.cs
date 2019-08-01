using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUp : PickUpsBase
{
    [Header("Shield")]
    public float shieldStrength;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && triggerAmount > 0)
        {
            DoAction(other.gameObject);
        }
    }

    public override void DoAction(GameObject player)
    {
        base.DoAction(player);
        transform.parent = player.transform;
        transform.position = player.transform.position;
        player.GetComponent<PlayerHealth>().AddHealth(shieldStrength);
    }
}
