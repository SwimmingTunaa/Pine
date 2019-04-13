using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelDestroyer : MonoBehaviour
{
    public PanelSpawner panelSpawner;
    public ObjectPoolManager objectPoolManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach(ObjectPools o in objectPoolManager.spawnedObjectPool)
        {
            if(other.GetComponent<ObjectID>().nameID == o.name)
            {
                panelSpawner.DestroyPanels(other.gameObject, o.spawnedObjectPool);
            }
        }
    }
}
