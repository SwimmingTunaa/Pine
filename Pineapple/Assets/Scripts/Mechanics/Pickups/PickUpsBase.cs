using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpsBase : MonoBehaviour
{
    [Header("Base")]
    public int triggerAmount = 1;
    public AudioClip pickUpSound;
    public GameObject deathFX;
    public float effectDuration;
    private float _initialDuration;
    private SpriteRenderer _visual;

    void Awake()
    {
        _initialDuration = effectDuration;
        _visual = GetComponentInChildren<SpriteRenderer>();
    }

    void OnEnable()
    {
        effectDuration = _initialDuration;
        triggerAmount = 1;
        _visual.enabled = true;
    }

    public virtual void DoAction(GameObject player)
    {
        triggerAmount--;
        //Only plays these if they are not null
        if(pickUpSound)
            GetComponent<AudioSource>().PlayOneShot(pickUpSound);
        if(deathFX)
            Instantiate(deathFX,transform.position,transform.rotation);
        //_visual.enabled = false;
    }
}
