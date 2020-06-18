using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : Spawner
{   
    public GameObject[] spawnPoints;
    public Collider2D floorCollider;

    private ObstaclePoolConfig _currentRegionLevel;
    private GameObject _NextObstacleToSpawn;
    private List<ObstaclePool> _obstacleSpawnPools;
    private Dictionary <Enums.ObstacleSpawnPoint, Vector3> spawnpointConfig = new Dictionary<Enums.ObstacleSpawnPoint, Vector3>();
    
    void Awake()
    {
        
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
        int randomIndex = randomIndexGetter.GetRandomEntryIndex(_currentRegionLevel.poolList);
        //get previous spawned object
        Transform previousObj = _NextObstacleToSpawn != null ? _NextObstacleToSpawn.transform : null;
//       Debug.Log(previousObj + " previous Spawn");
        ObstaclePool poolToUse = _currentRegionLevel.poolList[randomIndex]; 
        _NextObstacleToSpawn = GetNextItem(poolToUse.spawnedObjectPool);
        _NextObstacleToSpawn.SetActive(true);
        //update the floor spawn point so that it includes the temp objects collider difference
        spawnpointConfig[_currentRegionLevel.bot.spawnPointChoice] = GetFloorSpawnPoint(_currentRegionLevel, _NextObstacleToSpawn.GetComponent<Collider2D>());
        //Find the spawnpoint so that it spawn further from the previous spawn item
        float getXDifference = previousObj != null ? GetFurthestObjectDistance(previousObj.transform) - previousObj.position.x : 0;
        float newX = spawnpointConfig[poolToUse.spawnPointChoice].x + 
        (previousObj != null ? (GetFurthestObjectDistance(previousObj.transform) < spawnPoints[0].transform.position.x ? 0 : getXDifference) : 0);
        Vector3 NewSpawnPoint = new Vector2 (newX, spawnpointConfig[poolToUse.spawnPointChoice].y);
        //spawn object from the pool
        Spawn(NewSpawnPoint);
    }

    public void Initialize()
    {
        _currentRegionLevel = MasterSpawner.Instance.activeRegion.obstaclePoolsLevelInstances[0];
        //get the spwanpoint positions from the config
        spawnpointConfig = new Dictionary <Enums.ObstacleSpawnPoint, Vector3>
        {
            {_currentRegionLevel.top.spawnPointChoice, spawnPoints[0].transform.position},
            {_currentRegionLevel.mid.spawnPointChoice, spawnPoints[(int)Random.Range(1,2)].transform.position},
            {_currentRegionLevel.bot.spawnPointChoice, _NextObstacleToSpawn == null ? floorCollider.transform.position : GetFloorSpawnPoint(_currentRegionLevel, _NextObstacleToSpawn.GetComponent<Collider2D>())},
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
        _NextObstacleToSpawn.transform.position = spawnPoint;   
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

    public void changePoolSpawnChance(float top, float mid, float bot)
    {
        _currentRegionLevel.top.spawnChanceValue = top;
        _currentRegionLevel.mid.spawnChanceValue = mid;
        _currentRegionLevel.bot.spawnChanceValue = bot;
    }

    public void UpdateLevelConfig(ObstaclePoolConfig newPoolConfig)
    {
        _currentRegionLevel = newPoolConfig;
        Initialize();
    }
} 
