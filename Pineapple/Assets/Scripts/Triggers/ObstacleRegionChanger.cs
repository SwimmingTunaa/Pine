using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRegionChanger : MonoBehaviour
{
    public Region regionToChangeTo;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            
        }
    }
}
