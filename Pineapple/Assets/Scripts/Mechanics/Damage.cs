using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public bool destroyHair;
    public float damageAmount;

    void OnTriggerEnter2D(Collider2D other)
    {
       DoDamage(other.gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        DoDamage(other.gameObject);
    }

    void DoDamage(GameObject other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<HealthGeneric>().TakeDamage(damageAmount);
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
                dialogue.startDialogue();
                p._anim.SetTrigger("Sad");
                p.GetComponent<AudioSource>().PlayOneShot(p.hairSlicedAudio);
                GameObject tempObj = Instantiate(p.slicedHair, other.transform.position, p.slicedHair.transform.rotation);
                Destroy(tempObj, 1f);
            }
        }
    }
}
