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

    GameObject _tempGO;
    private List<ObstaclePool> _obstacleSpawnPools;
    private Dictionary <Enums.ObstacleSpawnPoint, Vector3> spawnpointConfig = new Dictionary<Enums.ObstacleSpawnPoint, Vector3>();
    
    void Awake()
    {
        currentLevelConfig = levelConfig.startLevel;
    }
    void Start()
    {
        Initialize();
    }
 
    public override void DoSpawn()
    {
        SpawnAllObstacles();
    }

    void SpawnAllObstacles()
    {
        Initialize();
        //get random pool to spawn
        var randomIndexGetter = new cummulativeCalculator<ObstaclePool>();
        int randomIndex = randomIndexGetter.GetRandomEntryIndex(_obstacleSpawnPools);
        //get previous spawned object
        Transform previousObj = _tempGO != null ? _tempGO.transform : null;
//        Debug.Log(previousObj + " previous Spawn");
        ObstaclePool poolToUse = _obstacleSpawnPools[randomIndex]; 
        _tempGO = GetObjToSpawn(poolToUse);
        //instantiate a new obj if the current one is already active
        createNewObstacle();
        _tempGO.SetActive(true);
        //update the floor spawn point so that it includes the temp objects collider difference
        spawnpointConfig[currentLevelConfig.bot.spawnPointChoice] = GetFloorSpawnPoint(currentLevelConfig, _tempGO.GetComponent<Collider2D>());
        //Find the spawnpoint so that it spawn further from the previous spawn item
        float getXDifference = previousObj != null ? GetFurthestObjectDistance(previousObj.transform) - previousObj.position.x : 0;
        float newX = spawnpointConfig[poolToUse.spawnPointChoice].x + 
        (previousObj != null ? (GetFurthestObjectDistance(previousObj.transform) < spawnPoints[0].transform.position.x ? 0 : getXDifference) : 0);
        Vector3 NewSpawnPoint = new Vector2 (newX, spawnpointConfig[poolToUse.spawnPointChoice].y);
        //spawn object from the pool
        Spawn(NewSpawnPoint);
        poolToUse.spawnedObjectPool.Remove(_tempGO);
    }

    public void Initialize()
    {
        _obstacleSpawnPools = new List<ObstaclePool>();
        _obstacleSpawnPools.Add(currentLevelConfig.top);
        _obstacleSpawnPools.Add(currentLevelConfig.mid);
        _obstacleSpawnPools.Add(currentLevelConfig.bot);
        //get the spwanpoint positions from the config
        spawnpointConfig = new Dictionary <Enums.ObstacleSpawnPoint, Vector3>
        {
            {currentLevelConfig.top.spawnPointChoice, spawnPoints[0].transform.position},
            {currentLevelConfig.mid.spawnPointChoice, spawnPoints[(int)Random.Range(1,2)].transform.position},
            {currentLevelConfig.bot.spawnPointChoice, _tempGO == null ? floorCollider.transform.position : GetFloorSpawnPoint(currentLevelConfig, _tempGO.GetComponent<Collider2D>())},
        };
    }

    float GetFurthestObjectDistance(Transform go)
    {
        //get transform of previous spawned object and then find the distance for the last child object
        float tempxPos = 0;
        foreach(Transform child in go)
        {
            if(child.transform.localPosition.x < tempxPos)
                tempxPos = child.transform.localPosition.x;
        }
        //Debug.Log(tempxPos);
        return tempxPos;
    }
    
    GameObject GetObjToSpawn(ObstaclePool pool)
    {
        return pool.spawnedObjectPool[Random.Range(0,pool.spawnedObjectPool.Count)];
    }

    void Spawn(Vector2 spawnPoint)
    {
        _tempGO.transform.position = spawnPoint;   
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
        if(_tempGO.activeInHierarchy)
        {
            GameObject newObj = Instantiate(_tempGO,transform.position, transform.rotation);
            newObj.layer = _tempGO.layer;
            newObj.tag = _tempGO.tag;
            newObj.GetComponent<ObjectID>().CreateID(ObjType.Obstacle, _tempGO.GetComponent<ObjectID>().parentPool);
            _tempGO = newObj;
            _tempGO.SetActive(false);
        }
    }

    public void changePoolSpawnChance(float top, float mid, float bot)
    {
        currentLevelConfig.top.spawnChanceValue = top;
        currentLevelConfig.mid.spawnChanceValue = mid;
        currentLevelConfig.bot.spawnChanceValue = bot;
    }

    public void UpdateLevelConfig(ObstaclePoolConfig newPoolConfig)
    {
        currentLevelConfig = newPoolConfig;
        Initialize();
    }
} 
