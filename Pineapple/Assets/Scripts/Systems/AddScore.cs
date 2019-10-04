﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScore : MonoBehaviour
{
    public int triggerAmount = 1;
    public int scoreToAdd;
    public string scoreMessage;
    public bool isTrigger; 
    
    private StatsManager stats;
    private int startTriggerAmount;
    void Awake()
    {
         startTriggerAmount = triggerAmount;
    }

    void Start()
    {
        stats = GameObject.FindGameObjectWithTag("GameController").GetComponent<StatsManager>();
        triggerAmount = startTriggerAmount;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Hair") && triggerAmount > 0 && isTrigger)
        {
            triggerAmount--;
            stats.AddToScore(scoreToAdd, scoreMessage);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Hair") && triggerAmount > 0 && !isTrigger)
        {
            triggerAmount--;
            stats.AddToScore(scoreToAdd, scoreMessage);
        }
    }
}
