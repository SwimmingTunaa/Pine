using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : Spawner
{   
    public ObstaclePoolConfig currentLevelConfig;
    public ObstacleLevelConfig levelConfig;


    [Space(10)]
    public GameObject[] spawnPoints;
    public Collider2D floorCollider;

    GameObject tempGO;
    private List<ObstaclePool> _obstacleSpawnPools;
    private Dictionary <Enums.ObstacleSpawnPoint, Vector3> spawnpointConfig;
    

    //TODO: spawn obejct on floor, but if object overlap each other spawn them next to eachother instead. Spawn every x interval.
    //Divide the panle up into 3 to spawn objects there? 

    void Start()
    {
        currentLevelConfig = levelConfig.startLevel;
    }
 
    public override void DoSpawn()
    {
        SpawnAllObstacles();
    }

    void SpawnAllObstacles()
    {
        _obstacleSpawnPools = new List<ObstaclePool>();
        _obstacleSpawnPools.Add(currentLevelConfig.top);
        _obstacleSpawnPools.Add(currentLevelConfig.mid);
        _obstacleSpawnPools.Add(currentLevelConfig.bot);

        //get random pool to spawn
        var randomIndexGetter = new cummulativeCalculator<ObstaclePool>();
        int randomIndex = randomIndexGetter.GetRandomEntryIndex(_obstacleSpawnPools);
        ObstaclePool poolToUse = _obstacleSpawnPools[randomIndex]; 
        tempGO = GetObjToSpawn(poolToUse);
        createNewObstacle();
        tempGO.SetActive(true);
        //get the spwanpoint positions from the config
      
        spawnpointConfig = new Dictionary <Enums.ObstacleSpawnPoint, Vector3>
        {
            {currentLevelConfig.top.spawnPoint, spawnPoints[0].transform.position},
            {currentLevelConfig.mid.spawnPoint, spawnPoints[1].transform.position},
            {currentLevelConfig.bot.spawnPoint, GetFloorSpawnPoint(currentLevelConfig, tempGO.GetComponent<Collider2D>())}
        };
        //spawn object from the pool
        Spawn(spawnpointConfig[poolToUse.spawnPoint]);
        poolToUse.spawnedObjectPool.Remove(tempGO);
    }
    
    GameObject GetObjToSpawn(ObstaclePool pool)
    {
        return pool.spawnedObjectPool[Random.Range(0,pool.spawnedObjectPool.Count)];
    }

    void Spawn(Vector3 spawnPoint)
    {
        tempGO.transform.position = spawnPoint;   
    }

    public Vector3 GetFloorSpawnPoint(ObstaclePoolConfig config, Collider2D objectCollider)
    {
        if(floorCollider != null && config.bot != null)
        {
            float yPos = (floorCollider.transform.position.y + floorCollider.bounds.extents.y) + (objectCollider.bounds.extents.y + Mathf.Abs(objectCollider.offset.y));

            Vector3 _floorSpawnPoint = new Vector3(spawnPoints[2].transform.position.x, yPos, spawnPoints[2].transform.position.z); 
            return _floorSpawnPoint;                                         
        }
        return spawnPoints[3].transform.position;
    }

    void createNewObstacle()
    {
        //instantiate a new obj if the current one is already active
        if(tempGO.activeInHierarchy)
        {
            GameObject newObj = Instantiate(tempGO,transform.position, transform.rotation);
            newObj.layer = tempGO.layer;
            newObj.tag = tempGO.tag;
            newObj.GetComponent<ObjectID>().CreateID(ObjType.Obstacle, tempGO.GetComponent<ObjectID>().parentPool);
            tempGO = newObj;
            tempGO.SetActive(false);
        }
    }

    public void changePoolSpawnChance(float top, float mid, float bot)
    {
        currentLevelConfig.top.spawnChanceValue = top;
        currentLevelConfig.mid.spawnChanceValue = mid;
        currentLevelConfig.bot.spawnChanceValue = bot;
    }
} 
