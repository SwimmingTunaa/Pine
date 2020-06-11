using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDamage : MonoBehaviour
{
    public int triggerAmount = 2;
    public static int health = 2;
    public Sprite crackedWood;
    public GameObject deathEffect;
    public AudioClip deathSound;
    public static bool floorIsOpened;
    public float delayTime = 0.5f;
    
    void OnEnable()
    {
        if(floorIsOpened)
            gameObject.SetActive(false);
        UpdateState();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && triggerAmount > 0)
        {
            TakeDamage(1); 
            triggerAmount--;   
            UpdateState();
        }
    }

    void UpdateState()
    {
        if(health == 0)
            {
                floorIsOpened = true; 
                StartCoroutine(delayTurnOff(false, delayTime));   
            }
            else    
                if(health == 1)
                {
                    foreach (Transform child in transform)
                    {
                        child.GetComponent<SpriteRenderer>().sprite = crackedWood;
                    }
                }
    }
    
    static void TakeDamage(int damage)
    {
        if(health > 0)
            health -= damage;
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
