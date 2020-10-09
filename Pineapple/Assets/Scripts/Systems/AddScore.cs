using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScore : MonoBehaviour
{    public int triggerAmount = 1;
    public string statReference;
    public IntVariable intToAddTo;

    private int startTriggerAmount;
    void Awake()
    {
         startTriggerAmount = triggerAmount;
    }

    void Start()
    {
        triggerAmount = 1;
    }

    void OnDisable()
    {
        triggerAmount = 1;
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
            triggerAmount--;
            StatsManager.Instance?.AddToAStat(1, statReference);
            if(intToAddTo) intToAddTo.RuntimeValue += 1;
        }
    }
}
