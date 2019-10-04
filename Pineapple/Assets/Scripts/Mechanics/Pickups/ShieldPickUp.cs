using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUp : PickUpsBase
{
    [Header("Shield")]
    public float shieldStrength;
    public AudioClip deathSound;

    private PlayerHealth health;
    private bool active;

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
        active = true;
        base.DoAction(player);
        transform.parent = player.transform;
        transform.position = player.transform.position;
        health.AddHealth(shieldStrength);
        GetComponentInChildren<Animator>().Play("Shield Start");
    }

    void Update()
    {
        if(health != null && health.health <= 1 && active)
        {
            active = false;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().PlayOneShot(deathSound);
            transform.parent = null;
            GetComponent<ObjectID>().Disable();
        }
    }
}
