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
    public StickerSpawner stickerSpawner;

    private Dictionary<Spawner, float>  _spawnerList = new Dictionary<Spawner, float>();
    private Rigidbody2D _playerRb;

    void Start()
    {
        _playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        AddToChanceList(projectileSpawner, pSpawnChance);
        AddToChanceList(obstacleSpawner, oSpawnChance);
        AddToChanceList(stickerSpawner, sSpawnChance);
        spawnInterval = Random.Range(minDistance, maxDistance);
    }

    void OnEnable()
    {
        spawnInterval = GameManager._distanceTraveled + Random.Range(minDistance, maxDistance);
    }

    void Update()
    {
        DoSpawn();
    }

    public override void DoSpawn()
    {
        if(GameManager._distanceTraveled >= spawnInterval)
        {
            float val = Random.value;
            spawnInterval += Random.Range(minDistance, maxDistance);
            foreach(KeyValuePair<Spawner, float> s in _spawnerList)
            {
                if(val <= s.Value)
                {
                    Debug.Log("SpawningStuff");
                    s.Key.DoSpawn();
                    break;
                }
                else
                    val -= s.Value;
            }
        }
    }

    public void AddToChanceList(Spawner spawner, float chance)
    {
        _spawnerList.Add(spawner, chance);
    }

    public void ChangeSpawnerTypeChance(float newPchance, float newOchance, float newSchance)
    {
        _spawnerList[projectileSpawner] = newPchance;
        _spawnerList[obstacleSpawner] = newOchance;
        _spawnerList[stickerSpawner] = newSchance;
    }
}
