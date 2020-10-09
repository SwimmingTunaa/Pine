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
    public bool itemSpawned;

    void Awake()
    {
        instance = this;
    }

    public override void DoSpawn()
    {
        getPoolToSpawnFrom();
    }

    void getPoolToSpawnFrom()
    {
        var randomIndexGetter = new cummulativeCalculator<ObjectPools>();
        randomIndex = randomIndexGetter.GetRandomEntryIndex(poolToSpawn);
        ObjectPools poolToUse = poolToSpawn[randomIndex];
        //use sticker pool if an item is already spawned
        if(itemCurrentlyActive || itemSpawned) poolToUse = poolToSpawn[2];
        else
            //checks to see if its item pools to make sure to only spawn one item at a time;
            if(!itemSpawned && poolToUse != poolToSpawn[2]) itemSpawned = true;
        GameObject tempObj = poolToUse.GetNextItem();
        if(tempObj == null) return;
        tempObj.SetActive(true); 
        tempObj.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;        
    }

    public void ChangeRewardPoolSpawnChances(float stickerSpawnChance, float itemSpawnChance,float sItemSpawnChance)
    {
        poolToSpawn[0].runTimeSpawnChanceValue = itemSpawnChance;
        poolToSpawn[1].runTimeSpawnChanceValue = stickerSpawnChance;
        poolToSpawn[2].runTimeSpawnChanceValue = sItemSpawnChance;
    }
}
