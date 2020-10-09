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

    [Header("Reward Spawner")]
    public float rSpawnChance;  

    [Header("Critter Spawner")]
    public CritterSpawner critterSpawner;

    [Header("Seedling Spawner")]
    public CritterSpawner seedlingSpawner;
    public float seedlingSpawnChance;

    [Header("Progression")]
    public int minSpawnAmount, maxSpawnAmount;
    public int minRewardAmount, maxRewardAmount;
    public LevelProgressionSystem levelPro;

    public Dictionary<string, Spawner>  listOfSpawners = new Dictionary<string, Spawner>();

    private int _spawnAmount;
    private int _rewardAmount;
    private Dictionary<Spawner, float>  _challengeSpawnerList = new Dictionary<Spawner, float>();
    private Dictionary<Spawner, float>  _rewardSpawnerList = new Dictionary<Spawner, float>();
    private float _randomInterval;
    private int pickUpSpawned; //the amount of pickup already spawned;
    private float _startMin;
    private float _startMax;
    
    void Awake()
    {
        if (Instance != null) 
                Destroy(gameObject);
            else
                Instance = this;
        _startMax = maxDistance;
        _startMin = minDistance;
        //add these to the challenges
    }
    void Start()
    {
        AddToChallengeChanceList(projectileSpawner, pSpawnChance);
        AddToChallengeChanceList(ObstacleSpawner.Instance, oSpawnChance);

        listOfSpawners = new Dictionary<string, Spawner>(){
            {projectileSpawner.name, projectileSpawner},
            {ObstacleSpawner.Instance.name, ObstacleSpawner.Instance},
            {RewardSpawner.instance.name, RewardSpawner.instance},
            {SpiderSpawner.instance.name, SpiderSpawner.instance},
            {critterSpawner.name, critterSpawner}
        };

        _rewardSpawnerList.Add(RewardSpawner.instance, rSpawnChance);

        IniatilizeSpawners(active);
        _spawnAmount = Random.Range(minSpawnAmount,maxSpawnAmount);
        _randomInterval = Random.Range(minDistance, maxDistance);
        spawnInterval = Statics.DistanceTraveled + _randomInterval;
        //Debug.Log("Spawn Amount: " + _spawnAmount);
        //if the player restarted then spawn rewards intially
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
            //spawn Mouse
            critterSpawner.DoSpawn();
            spawnInterval = getSpawnInterval();
            if(_spawnAmount <=0 && _rewardAmount <=0)
            {
                _spawnAmount = Random.Range(minSpawnAmount, maxSpawnAmount); 
                RewardSpawner.instance.itemSpawned = false;
                return;
            }
            //Debug.Log("Spawn amount: " + _spawnAmount);
            //Debug.Log("Reward Spawn amount: " + _rewardAmount);
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
                minDistance = 8f;
                maxDistance = 12f;

                SpawnType(_rewardSpawnerList);

                minDistance = tempMin;
                maxDistance = tempMax;
            }    
        }else
            if(Statics.DistanceTraveled >= spawnInterval + 10f)
            {
                //spawn seeddling at random chance
                var randomNum = Random.Range(0f,1f);
                if(randomNum < seedlingSpawnChance) seedlingSpawner.DoSpawn();
            }
            
    }

    public void AddToChallengeChanceList(Spawner spawner, float chance)
    {
        if(!_challengeSpawnerList.ContainsKey(spawner))
            _challengeSpawnerList.Add(spawner, chance);
    }

    public void RemoveFromChallengeList(Spawner spawner)
    {
        if(_challengeSpawnerList.ContainsKey(spawner))
            _challengeSpawnerList.Remove(spawner);
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
                    if(s.Key == RewardSpawner.instance && pickUpSpawned == 0)
                    {
                        if(RewardSpawner.instance.poolToSpawn[RewardSpawner.instance.randomIndex].objectType == ObjType.Pickups)
                        {
                            pickUpSpawned++;
                            //check to see if its the special item spawner
                            if(RewardSpawner.instance.poolToSpawn[RewardSpawner.instance.randomIndex] == RewardSpawner.instance.poolToSpawn[2])
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
        _challengeSpawnerList[ObstacleSpawner.Instance] = newOchance;
    }

    void IniatilizeSpawners(bool active)
    {
        projectileSpawner.enabled = active;
        ObstacleSpawner.Instance.enabled = active;
        RewardSpawner.instance.enabled = active;
    }

    public void ChangeSpawnAmount(int newSpawnAmount)
    {
        _spawnAmount = newSpawnAmount;
    }

    public void ChangeMinMax(float newMin, float newMax, bool defualt)
    {
        if(!defualt)
        {
            minDistance = newMin;
            maxDistance = newMax;
        }
        else
            {
                minDistance = _startMin;
                maxDistance = _startMax;
            }
    }

    float getSpawnInterval()
    {
        _randomInterval = Random.Range(minDistance, maxDistance);
        return Statics.DistanceTraveled + _randomInterval;
    }
}
