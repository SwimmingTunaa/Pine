using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUp : PickUpsBase
{
    [Header("Shield")]
    public float shieldStrength;

    private PlayerHealth health;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && triggerAmount > 0)
        {
            health = other.gameObject.GetComponent<PlayerHealth>();
            DoAction(other.gameObject);
        }
    }

    public override void DoAction(GameObject player)
    {
        base.DoAction(player);
        transform.parent = player.transform;
        transform.position = player.transform.position;
        health.AddHealth(shieldStrength);
    }

    void Update()
    {
        if(health != null && health.health <= 1)
        {
            transform.parent = null;
            GetComponent<ObjectID>().Disable();
        }
    }
}
