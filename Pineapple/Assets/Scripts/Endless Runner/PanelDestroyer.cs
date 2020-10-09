using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelDestroyer : MonoBehaviour
{   
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<ObjectID>())
        {
            ObjectID id = other.GetComponent<ObjectID>();
            if(id.objectType == ObjType.Panel)
            {
                PanelSpawner.Instance.SpawnSets();
                other.transform.parent = null;
            }
            if (id.objectType == ObjType.Obstacle)
            {
                ObstacleSpawner.Instance.activeObstacles.Remove(other.gameObject);
            }

            if(!id.selfDestroy) id.Disable();
        }
    }
}
