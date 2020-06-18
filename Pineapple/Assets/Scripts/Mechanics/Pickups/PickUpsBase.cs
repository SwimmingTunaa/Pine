﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpsBase : MonoBehaviour
{
    [Header("Base")]
    public int triggerAmount = 1;
    public PickUpObject item;
    private SpriteRenderer _visual;
    private float _timer;
    [HideInInspector] public bool _timerActive;

    void Awake()
    {
        _visual = GetComponentInChildren<SpriteRenderer>();
    }

    void OnEnable()
    {
        triggerAmount = 1;
        _visual.enabled = true;
    }

    public virtual void DoAction(GameObject player)
    {
        triggerAmount--;
        _timerActive = true;
        //Only plays these if they are not null
        if(item.pickUpSound)
            GetComponent<AudioSource>().PlayOneShot(item.pickUpSound);
        if(item.deathFX)
            Instantiate(item.deathFX);
        //_visual.enabled = false;
    }

    public bool Timer(float interval)
    {
        _timer += Time.deltaTime;
        if(_timer > interval)
        {
            _timer = 0f;
            return true;
        }
        return false;
    }
    
    public virtual void DisablePickUp()
    {
        if(GetComponent<ObjectID>() != null)
            GetComponent<ObjectID>().Disable();
        _timerActive = false;
        //Debug.Log("Item Disabled");
        triggerAmount = 1;
    }

    public virtual void Update()
    {
        if(_timerActive && item.effectDuration > 0 && Timer(item.effectDuration))
            DisablePickUp();
    }
}
