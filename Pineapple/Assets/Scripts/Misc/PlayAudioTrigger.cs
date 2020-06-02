using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class PlayAudioTrigger : MonoBehaviour
{
    public int triggerAmount = 1;
    public AudioClip audioClip;

   void OnTriggerEnter2D(Collider2D other)
   {
       if(triggerAmount > 0)
       {
            triggerAmount--;
            GetComponent<AudioSource>().PlayOneShot(audioClip);
       }
   }
}
