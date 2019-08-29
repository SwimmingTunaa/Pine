using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardSpawner : Spawner
{
    public List<ObjectPools> poolToSpawn = new List<ObjectPools>();
    public GameObject[] spawnPoints;
    public float timerMin, timerMax;

    public override void DoSpawn()
    {
        getPoolToSpawnFrom();
    }

    void getPoolToSpawnFrom()
    {
        var randomIndexGetter = new cummulativeCalculator<ObjectPools>();
        int randomIndex = randomIndexGetter.GetRandomEntryIndex(poolToSpawn);
        ObjectPools poolToUse = poolToSpawn[randomIndex];
        GameObject tempObj = poolToUse.spawnedObjectPool[Random.Range(0,poolToUse.spawnedObjectPool.Count)];
        tempObj.SetActive(true); 
        tempObj.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
        poolToUse.spawnedObjectPool.Remove(tempObj);
    }

    public void ChangeRewardPoolSpawnChances(float stickerSpawnChance, float itemSpawnChance)
    {
        poolToSpawn[0].spawnChanceValue = itemSpawnChance;
        poolToSpawn[1].spawnChanceValue = stickerSpawnChance;
    }
}
