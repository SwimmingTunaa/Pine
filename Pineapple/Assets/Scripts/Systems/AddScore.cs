using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScore : MonoBehaviour
{
    public int triggerAmount = 1;
    public int scoreToAdd;
    public string scoreMessage;
    public bool isTrigger; 
    public string statReference;
    
    private int startTriggerAmount;
    void Awake()
    {
         startTriggerAmount = triggerAmount;
    }

    void Start()
    {
        triggerAmount = startTriggerAmount;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        AddScoreAndStats(other.gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        AddScoreAndStats(other.gameObject);
    }

    void AddScoreAndStats(GameObject other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Hair") && triggerAmount > 0 && !isTrigger)
        {
            triggerAmount--;
            StatsManager.Instance.AddToScore(scoreToAdd, scoreMessage);
            if(statReference != null)
                StatsManager.Instance.AddToAStat(1, statReference);
        }
    }
}
