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
        if(triggerAmount > 0 && other.CompareTag("Player"))
        {
            health = other.gameObject.GetComponent<PlayerHealth>();
            DoAction(health.gameObject);
        }
    }

    public override void DoAction(GameObject player)
    {
        active = true;
        base.DoAction(player);
        transform.parent = player.GetComponent<Outfits>().powerUpEffectSpawnPoint;
        transform.position = player.GetComponent<Outfits>().powerUpEffectSpawnPoint.position;
        ShieldPickUpObject s = item.Instance as ShieldPickUpObject;
        health.AddHealth(s.shieldStrength);
        if(GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Shield Shine"))
            GetComponentInChildren<Animator>().Play("Shield Start");
    }

    public override void Update()
    {
        if(health != null && health.health <= 1 && active)
        {
            active = false;
            if(deathSound != null)
            GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(deathSound);
            transform.parent = null;
            GetComponent<ObjectID>().Disable();
        }
    }
}
