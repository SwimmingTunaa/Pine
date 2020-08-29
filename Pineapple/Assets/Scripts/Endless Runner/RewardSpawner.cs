using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardSpawner : Spawner
{
    public static RewardSpawner instance;
    //bool to see if any pick ups are currently active;
    public bool itemCurrentlyActive;
    public List<ObjectPools> poolToSpawn = new List<ObjectPools>();
    public GameObject[] spawnPoints;
    public int randomIndex;
    public List<ObjectPools> poolToSpawnInstances = new List<ObjectPools>();
    public bool itemSpawned;

    void Awake()
    {
        instance = this;
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
        //use sticker pool if an item is already spawned
        if(itemCurrentlyActive || itemSpawned) poolToUse = poolToSpawnInstances[2];
        else
            //checks to see if its item pools to make sure to only spawn one item at a time;
            if(!itemSpawned && poolToUse != poolToSpawnInstances[2]) itemSpawned = true;
        GameObject tempObj = GetNextItem(poolToUse.spawnedObjectPool);
        if(tempObj == null) return;
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
