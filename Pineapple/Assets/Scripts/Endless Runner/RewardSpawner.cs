using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardSpawner : Spawner
{
    public List<ObjectPools> poolToSpawn = new List<ObjectPools>();
    public GameObject[] spawnPoints;
    public int randomIndex;
    private List<ObjectPools> poolToSpawnInstances = new List<ObjectPools>();

    void Awake()
    {
        for (int i = 0; i < poolToSpawn.Count;  i++)
        {
            poolToSpawnInstances.Add(Instantiate(poolToSpawn[i]));
            poolToSpawnInstances[i].Initialise();
        }
    }

    public override void DoSpawn()
    {
        getPoolToSpawnFrom();
    }

    void getPoolToSpawnFrom()
    {
        var randomIndexGetter = new cummulativeCalculator<ObjectPools>();
        randomIndex = randomIndexGetter.GetRandomEntryIndex(poolToSpawnInstances);
        ObjectPools poolToUse = poolToSpawnInstances[randomIndex];
        GameObject tempObj = GetNextItem(poolToUse.spawnedObjectPool);
        tempObj.SetActive(true); 
        tempObj.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
    }

    public void ChangeRewardPoolSpawnChances(float stickerSpawnChance, float itemSpawnChance,float sItemSpawnChance)
    {
        poolToSpawnInstances[0].spawnChanceValue = itemSpawnChance;
        poolToSpawnInstances[1].spawnChanceValue = stickerSpawnChance;
        poolToSpawnInstances[2].spawnChanceValue = sItemSpawnChance;
    }
}
