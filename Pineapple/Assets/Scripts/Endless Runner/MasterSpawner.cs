using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSpawner : MonoBehaviour
{
    public static MasterSpawner Instance;

    public bool active = true;
    public float spawnInterval = 1;
    public Region activeRegion;
    public float minDistance, maxDistance;
    [Header("Projectile Spawner")]
    public float pSpawnChance;  
    public ProjectileSpawner projectileSpawner;
    [Header("Obstacle Spawner")]
    public float oSpawnChance;
    public ObstacleSpawner obstacleSpawner;
    [Header("Reward Spawner")]
    public float rSpawnChance;  
    public RewardSpawner RewardSpawner;
    [Header("Critter Spawner")]
    public CritterSpawner critterSpawner;
    [Header("Progression")]
    public int minSpawnAmount, maxSpawnAmount;
    public int minRewardAmount, maxRewardAmount;
    public LevelProgressionSystem levelPro;

    private int _spawnAmount;
    private int _rewardAmount;
    private Dictionary<Spawner, float>  _challengeSpawnerList = new Dictionary<Spawner, float>();
    private Dictionary<Spawner, float>  _rewardSpawnerList = new Dictionary<Spawner, float>();
    private float _randomInterval;
    private int pickUpSpawned; //the amount of pickup already spawned;
    
    void Awake()
    {
        if (Instance != null) 
                Destroy(gameObject);
            else
                Instance = this;
                
        //add these to the challenges
        AddToChanceList(projectileSpawner, pSpawnChance);
        AddToChanceList(obstacleSpawner, oSpawnChance);
        _rewardSpawnerList.Add(RewardSpawner, rSpawnChance);
    }
    void Start()
    {
        IniatilizeSpawners(active);
        _spawnAmount = Random.Range(minSpawnAmount,maxSpawnAmount);
        _randomInterval = Random.Range(minDistance, maxDistance);
        spawnInterval = Statics.DistanceTraveled + _randomInterval;
        Debug.Log("Spawn Amount: " + _spawnAmount);
        if(Statics.playerRestartedGame)
        {
            _spawnAmount = 0;
            _rewardAmount = 2;
        }
           
    }

    void OnEnable()
    {
        spawnInterval = Statics.DistanceTraveled + _randomInterval;
    }

    void Update()
    {
        DoSpawn();
    }

    public void DoSpawn()
    {
        if(Statics.DistanceTraveled >= spawnInterval)
        {
            critterSpawner.DoSpawn();
            spawnInterval = getSpawnInterval();
            if(_spawnAmount <=0 && _rewardAmount <=0)
            {
                _spawnAmount = Random.Range(minSpawnAmount, maxSpawnAmount);  
                if(levelPro != null && levelPro.roundsCompleted >= 1)
                {
                    //make the level harder
                    levelPro.difficultyLvl += 1;
                    levelPro.SetNewCheckpoints(Random.Range(75f,100f),Random.Range(75f,150f), Random.Range(100f,150f),Random.Range(75f,100f));
                }
                return;
            }
            Debug.Log("Spawn amount: " + _spawnAmount);
            Debug.Log("Reward Spawn amount: " + _rewardAmount);
            if(_spawnAmount > 0)
            {
                _spawnAmount--;
                if(_spawnAmount <=0)
                {
                    //set how many rewards to spawn in
                    _rewardAmount = Random.Range(minRewardAmount, maxRewardAmount);
                    pickUpSpawned = 0;
                }    
                SpawnType(_challengeSpawnerList);
            }
            else if(_spawnAmount <=0 && _rewardAmount > 0)
            {
                _rewardAmount--;
                //change the spawn distance for rewards then revert it back to the original
                float tempMin = minDistance;
                float tempMax = maxDistance;
                minDistance = 15f;
                maxDistance = 18f;

                SpawnType(_rewardSpawnerList);

                minDistance = tempMin;
                maxDistance = tempMax;
            }    
        }
    }

    public void AddToChanceList(Spawner spawner, float chance)
    {
        _challengeSpawnerList.Add(spawner, chance);
    }
    
    void SpawnType(Dictionary<Spawner, float> spawnerType)
    {
        float val = Random.value;
            spawnInterval += Random.Range(minDistance, maxDistance);
            foreach(KeyValuePair<Spawner, float> s in spawnerType)
            {
                if(val <= s.Value)
                {
                    //make sure the reward only spawn pick ups items once
                    if(s.Key == RewardSpawner && pickUpSpawned == 0)
                    {
                        if(RewardSpawner.poolToSpawn[RewardSpawner.randomIndex].objectType == ObjType.Pickups)
                        {
                            
                            pickUpSpawned++;
                            //check to see if its the special item spawner
                            if(RewardSpawner.poolToSpawn[RewardSpawner.randomIndex] == RewardSpawner.poolToSpawn[2])
                            {
                                s.Key.DoSpawn();
                                //make sure no more reward spawn after special item has been spawned
                                _rewardAmount = 0;
                            }
                            else
                            {
                                //spawn normal pick ups instead and add an xtra reward so it spawns stickers
                                s.Key.DoSpawn();
                                _rewardAmount += 1;
                            }
                        }
                    }
                    else 
                        //spawns the stickers
                        s.Key.DoSpawn();
                }
                else
                    val -= s.Value;
            }
    }

    public void ChangeSpawnerTypeChance(float newPchance, float newOchance)
    {
        _challengeSpawnerList[projectileSpawner] = newPchance;
        _challengeSpawnerList[obstacleSpawner] = newOchance;
    }

    void IniatilizeSpawners(bool active)
    {
        projectileSpawner.enabled = active;
        obstacleSpawner.enabled = active;
        RewardSpawner.enabled = active;
    }

    public void ChangeSpawnAmount(int newSpawnAmount)
    {
        _spawnAmount = newSpawnAmount;
    }

    float getSpawnInterval()
    {
        _randomInterval = Random.Range(minDistance, maxDistance);
        return Statics.DistanceTraveled + _randomInterval;
    }
}
