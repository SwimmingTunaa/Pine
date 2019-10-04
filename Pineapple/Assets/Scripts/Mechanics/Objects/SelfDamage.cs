using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDamage : HealthGeneric
{
    public Sprite crackedWood;
    public GameObject deathEffect;
    public AudioClip deathSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            GetComponent<HealthGeneric>().TakeDamage(1);            
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && health == 0)
        {
            StartCoroutine(delayTurnOff(false));
        }
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if(health == 1)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<SpriteRenderer>().sprite = crackedWood;
            }
        }
    }

    IEnumerator delayTurnOff(bool active)
    {
        yield return new WaitForSeconds(0.5f);
        deathEffect.SetActive(!active);
        gameObject.SetActive(active);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().PlayOneShot(deathSound);
        Instantiate(deathEffect, transform.position, transform.rotation);
        
    } 
}
