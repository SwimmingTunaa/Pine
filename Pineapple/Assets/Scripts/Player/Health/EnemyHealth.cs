using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthGeneric
{
    public GameObject attachedObject;
    public GameObject deathEffect;
    public AudioClip deathSound;
    public bool resetHealthOnEnable = true;
    
    void Start()
    {
        if (!attachedObject) attachedObject = gameObject;
    }
    
    public virtual void OnEnable()
    {
        if(resetHealthOnEnable) 
        {
            health = startHealth;
            dead = false;
        } 
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if(health <= 0 && !dead)
            Dead();
    }
    
    public override void Dead()
    {
        dead = true;

        //turn on death effect and unparent it so that it doesnt get disabled
        if(deathEffect)
        {
            deathEffect.SetActive(true);
            deathEffect.transform.parent = null;
        }

        if(deathSound)
             GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(deathSound);

        attachedObject.SetActive(false);
    }
}
