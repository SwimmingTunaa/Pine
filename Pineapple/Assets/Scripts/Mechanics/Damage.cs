using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int triggerAmount = 1;
    public float damageAmount;
    public bool destroyHair;
    public bool damageShieldOnly = false;
    public bool destroyAfterDamage;
    public AudioClip hitSoundEffect;
    public GameObject hitEffect;

    private Collision2D collision;

    void OnTriggerEnter2D(Collider2D other)
    {
        DoDamage(other.gameObject);
    }

    void OnEnable()
    {
        triggerAmount = 1;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
            collision = other;
        DoDamage(other.gameObject);
    }

    void DoDamage(GameObject other)
    {
        //Debug.Log(other.name);
        if (triggerAmount > 0)
        {
            if(other.CompareTag("Player"))
            {
                
                HealthGeneric health = other.GetComponent<HealthGeneric>();
                Debug.Log(health);
                if(damageShieldOnly && health.health > 1)
                    DealDamage(health);
                else if(!damageShieldOnly)
                    DealDamage(health);
               
               //for the projectiles
                if(destroyAfterDamage)
                {
                    var newRot = Quaternion.Euler(0, 0, -Vector3.Angle(GetComponent<Rigidbody2D>().velocity, -collision.GetContact(0).normal));
                    //Instantiate(hitEffect, collision.GetContact(0).point,newRot);
                    hitEffect.SetActive(true);
                    hitEffect.transform.position = collision.GetContact(0).point;
                    hitEffect.transform.rotation = newRot;
                    gameObject.GetComponent<ObjectID>().Disable();
                }
                triggerAmount--;
            }
            
            if(other.CompareTag("Hair") && destroyHair)
            {
                PlayerController p =  other.GetComponentInParent<PlayerController>();
                if(!p.hairMask.activeInHierarchy)
                {
                    p.hairMask.SetActive(true);
                    DialogueSequence dialogue = p.GetComponent<DialogueSequence>();
                    dialogue.dialogues[0].text = "MY HAIR!";
                    dialogue.dialogues[0].diallogueInterval = 2;
                    dialogue.StartDialogue(p.gameObject);
                    p._anim.SetTrigger("Sad");
                    p.GetComponent<AudioSource>().PlayOneShot(p.hairSlicedAudio);
                    StatsManager.Instance.AddToAStat(1,"HaircutsTaken");
                    GameObject tempObj = Instantiate(p.slicedHair, other.transform.position, p.slicedHair.transform.rotation);
                    Destroy(tempObj, 1f);
                }
            }
        }
       
    }
    
    public void DealDamage(HealthGeneric health)
    {
        health.TakeDamage(damageAmount);
        if(hitSoundEffect != null)
            GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(hitSoundEffect);    
    }
}
