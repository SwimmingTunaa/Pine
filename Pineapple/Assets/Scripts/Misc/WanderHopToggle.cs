using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderHopToggle : MonoBehaviour
{
    public Wander wander;
    
    private AudioSource audioS;
    void Start(){
        audioS = GetComponent<AudioSource>();
    }
    void ToggleHopOn()
    {
        wander.ToggleMove(false);
    }
     void ToggleHopOff()
    {
        wander.ToggleMove(true);
    }

    void PlayAudio(AudioClip clip)
    {
        audioS.PlayOneShot(clip);
    }
}
