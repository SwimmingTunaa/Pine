using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerSpawner : Spawner
{
    public ObjectPools stickerPool;
    public GameObject[] spawnPoints;
    public float timerMin, timerMax;

    public override void DoSpawn()
    {
        SpawnSticker();
    }

    void SpawnSticker()
    {
       
        //spawn random sticker at random spawn point           
        GameObject tempObj = stickerPool.spawnedObjectPool[Random.Range(0,stickerPool.spawnedObjectPool.Count)];
        tempObj.SetActive(true);
        tempObj.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
        stickerPool.spawnedObjectPool.Remove(tempObj);
    }
}
