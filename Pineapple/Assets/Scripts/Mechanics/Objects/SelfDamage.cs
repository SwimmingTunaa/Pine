using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDamage : HealthGeneric
{
    public Sprite crackedWood;
    public GameObject deathEffect;
    public AudioClip deathSound;
    public static bool isOpened;
    public float delayTime = 0.5f;
    
    void OnEnable()
    {
        if(isOpened)
            gameObject.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            TakeDamage(1); 
            isOpened = true;           
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && health == 0)
        {
            StartCoroutine(delayTurnOff(false, delayTime));
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

    IEnumerator delayTurnOff(bool active, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        deathEffect.SetActive(!active);
        gameObject.SetActive(active);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().PlayOneShot(deathSound);
        Instantiate(deathEffect, transform.position, transform.rotation);
    } 
}
