using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterSpawner : Spawner
{
    public ObjectPools critterPool;
    [MinMaxSlider(1,5)] public Vector2 amountToSpawn;
    public Transform spawnPoint;

    void Awake()
    {
        critterPool.Initialise();
    }

    public override void DoSpawn()
    {
        if(critterPool.spawnedObjectPool.Count > 0)
        {
            int randomSpawnAmount = (int)Random.Range(amountToSpawn.x, amountToSpawn.y);
            Vector3 newSpawnPos = new Vector3(spawnPoint.position.x + Random.Range(-3, 2), spawnPoint.position.y, spawnPoint.position.z);
            for (int i = 0; i < randomSpawnAmount; i++)
            {
                GameObject tempObj =  GetNextItem(critterPool.spawnedObjectPool);
                tempObj.SetActive(true);
                tempObj.transform.position = newSpawnPos;
            }
        }
       
    }
        
}
