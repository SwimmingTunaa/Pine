using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScore : MonoBehaviour
{
    public int triggerAmount = 1;
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

    void OnEnable()
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
        if(other.CompareTag("Player") || other.CompareTag("Hair") && triggerAmount > 0)
        {
            StatsManager.Instance?.AddToAStat(1, statReference);
        }
    }
}
