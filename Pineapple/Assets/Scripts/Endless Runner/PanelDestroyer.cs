using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelDestroyer : MonoBehaviour
{   
    public PanelSpawner pSpawner;
    public ObjectPoolManager objectPoolManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<ObjectID>())
        {
            ObjectID id = other.GetComponent<ObjectID>();
            if(id.objectType == ObjType.Panel)
            {
                pSpawner.SpawnSets();
            }
            if(!id.selfDestroy) id.Disable();
        }
    }
}
