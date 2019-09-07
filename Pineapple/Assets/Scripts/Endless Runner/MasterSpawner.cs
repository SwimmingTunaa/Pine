using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSpawner : Spawner
{
    public float minDistance, maxDistance;
    [Header("Projectile Spawner")]
    public float pSpawnChance;  
    public ProjectileSpawner projectileSpawner;
    [Header("Obstacle Spawner")]
    public float oSpawnChance;
    public ObstacleSpawner obstacleSpawner;
    [Header("Sticker Spawner")]
    public float sSpawnChance;  
    public RewardSpawner RewardSpawner;
    [Header("Progression")]
    public int minSpawnAmount, maxSpawnAmount;
    public int minRewardAmount, maxRewardAmount;
    public LevelProgressionSystem levelPro;

    private int _spawnAmount;
    private int _rewardAmount;
    private Dictionary<Spawner, float>  _challengeSpawnerList = new Dictionary<Spawner, float>();
    private Dictionary<Spawner, float>  _rewardSpawnerList = new Dictionary<Spawner, float>();
    private Rigidbody2D _playerRb;
    private float _randomInterval;

    void Start()
    {
        IniatilizeSpawners();
        _playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        AddToChanceList(projectileSpawner, pSpawnChance);
        AddToChanceList(obstacleSpawner, oSpawnChance);
        _rewardSpawnerList.Add(RewardSpawner, sSpawnChance);
        _spawnAmount = Random.Range(minSpawnAmount,maxSpawnAmount);
        _randomInterval = Random.Range(minDistance, maxDistance);
        spawnInterval = GameManager._distanceTraveled + _randomInterval;
        Debug.Log("Spawn Amount: " + _spawnAmount);
    }

    void OnEnable()
    {
        spawnInterval = GameManager._distanceTraveled + _randomInterval;
    }

    void Update()
    {
        DoSpawn();
    }

    public override void DoSpawn()
    {
        //Debug.Log(_randomInterval);
        if(GameManager._distanceTraveled >= spawnInterval)
        {
            spawnInterval = getSpawnInterval();
            if(_spawnAmount <=0 && _rewardAmount <=0)
            {
                _spawnAmount = Random.Range(minSpawnAmount, maxSpawnAmount);  
                if(levelPro != null && levelPro.roundsCompleted >= 1)
                {
                    levelPro.difficultyLvl = Random.Range(0,1);
                    levelPro.SetNewCheckpoints(Random.Range(75f,100f),Random.Range(75f,150f), Random.Range(100f,150f));
                }
                return;
            }

            if(_spawnAmount > 0)
            {
                _spawnAmount--;
//                Debug.Log("Spawn Amount: " + _spawnAmount);
                if(_spawnAmount <=0)
                {
                    _rewardAmount = Random.Range(minRewardAmount, maxRewardAmount);
                    //Debug.Log("Reward Amount: " + _rewardAmount);
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
                    Debug.Log("Spawning " + spawnerType + " Stuff");
                    s.Key.DoSpawn();
                    break;
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

    void IniatilizeSpawners()
    {
        projectileSpawner.enabled = true;
        obstacleSpawner.enabled = true;
        RewardSpawner.enabled = true;
    }

    float getSpawnInterval()
    {
        _randomInterval = Random.Range(minDistance, maxDistance);
        return GameManager._distanceTraveled + _randomInterval;
    }
}
