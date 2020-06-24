using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRegionChanger : MonoBehaviour
{
    public bool immediatelySpawnObstacle;
    public Region regionToChangeTo;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            //clear all current active obstacles
            foreach(GameObject g in ObstacleSpawner.Instance.activeObstacles)
            {
                g.GetComponent<ObjectID>().Disable();
            }
            ObstacleSpawner.Instance.activeObstacles.Clear();
            //set new region
            MasterSpawner.Instance.activeRegion = regionToChangeTo;

            if (immediatelySpawnObstacle)
            {
                ObstacleSpawner.Instance.DoSpawn();
            }
        }
    }
}
