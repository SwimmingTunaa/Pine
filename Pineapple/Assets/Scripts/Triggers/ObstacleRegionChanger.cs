using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRegionChanger : MonoBehaviour
{
    public bool immediatelySpawnObstacle;
    public bool disableAllOtherObstacles;
    public Region regionToChangeTo;
    private int _triggerAmount = 1;

    void OnEnable()
    {
        _triggerAmount = 1;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && _triggerAmount > 0)
        {
            //clear all current active obstacles
            if(disableAllOtherObstacles)
            {
                ObstacleSpawner.Instance.ClearActiveObstacles();
            }
            //set new region
            MasterSpawner.Instance.activeRegion = regionToChangeTo;

            if (immediatelySpawnObstacle)
            {
                ObstacleSpawner.Instance.DoSpawn();
            }
        }
    }
}
