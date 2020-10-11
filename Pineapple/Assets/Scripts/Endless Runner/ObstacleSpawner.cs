using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : Spawner
{   
    public static ObstacleSpawner Instance;
    public GameObject[] spawnPoints;

    private ObstaclePoolConfig _currentRegionLevel;
    private GameObject _NextObstacleToSpawn;
   
    private Dictionary <Enums.ObstacleSpawnPoint, Vector3> spawnpointConfig = new Dictionary<Enums.ObstacleSpawnPoint, Vector3>();
    private RaycastHit2D hit;
    private float newY;
    private Quaternion newRot;

    void Awake() =>  Instance = this;

    void Start() => Initialize();

    public override void DoSpawn() => SpawnAllObstacles();

    void SpawnAllObstacles()
    {
        Initialize();
        //get random pool to spawn
        var randomIndexGetter = new cummulativeCalculator<ObstaclePool>();
        int randomIndex = randomIndexGetter.GetRandomEntryIndex(_currentRegionLevel.poolList);
        //get previous spawned object
        Transform previousObj = _NextObstacleToSpawn != null ? _NextObstacleToSpawn.transform : null;

        //check to see if the poolList contains any pools if not return
        if(_currentRegionLevel.poolList.Count <= 0) return;
        ObstaclePool poolToUse = _currentRegionLevel.poolList[randomIndex]; 
        _NextObstacleToSpawn = poolToUse.GetNextItem();
        _NextObstacleToSpawn.SetActive(true);

        GetSpawnPoints();
      
        //Find the spawnpoint so that it spawn further from the previous spawn item
        float getXDifference = previousObj != null ? GetFurthestObjectDistance(previousObj.transform) - previousObj.position.x : 0;
        float newX = spawnpointConfig[poolToUse.spawnPointChoice].x + 
        (previousObj != null ? (GetFurthestObjectDistance(previousObj.transform) < spawnPoints[0].transform.position.x ? 0 : getXDifference) : 0);
        Vector3 NewSpawnPoint = new Vector2 (newX, spawnpointConfig[poolToUse.spawnPointChoice].y);
        //spawn object from the pool
        Spawn(NewSpawnPoint);
        activeObjects.Add(_NextObstacleToSpawn);
    }

    public void Initialize()
    {   
        int lvl = LevelProgressionSystem.Instance.difficultyLvl;
        _currentRegionLevel = MasterSpawner.Instance.activeRegion.obstaclePoolsLevelInstances[lvl - 1 < 0 ? 0 : lvl - 1];
        //get the spwanpoint positions from the config
        spawnpointConfig.Clear();
    }

    void GetSpawnPoints()
    {
        if(_currentRegionLevel.top) spawnpointConfig.Add(_currentRegionLevel.top.spawnPointChoice, spawnPoints[0].transform.position);
        if(_currentRegionLevel.mid) spawnpointConfig.Add(_currentRegionLevel.mid.spawnPointChoice, spawnPoints[(int)Random.Range(1,spawnPoints.Length)].transform.position);
        if(_currentRegionLevel.bot != null) spawnpointConfig.Add(_currentRegionLevel.bot.spawnPointChoice, GetFloorSpawnPoint(_NextObstacleToSpawn.GetComponent<Collider2D>()));
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
        _NextObstacleToSpawn.transform.rotation = Quaternion.FromToRotation(_NextObstacleToSpawn.transform.up, hit.normal);   
    }

    public Vector3 GetFloorSpawnPoint(Collider2D objectCollider)
    {
        hit = Physics2D.Raycast(transform.position, -transform.up, 10f, 1 << LayerMask.NameToLayer("Ground"));
        if(hit.collider != null)
            newY = hit.point.y;

        float yPos = newY + (objectCollider != null ? objectCollider.bounds.extents.y + Mathf.Abs(objectCollider.offset.y) : spawnPoints[2].transform.position.y);
        Vector3 _floorSpawnPoint = new Vector3(spawnPoints[2].transform.position.x, yPos, spawnPoints[2].transform.position.z); 
        return _floorSpawnPoint;                                         
    }

    public Vector3 GetFloorSpawnPoint(Collider2D objectCollider, float xPosOffset)
    {
        hit = Physics2D.Raycast(transform.position, -transform.up, 10f, 1 << LayerMask.NameToLayer("Ground"));
        if(hit.collider != null)
            newY = hit.point.y;
            
        float yPos = newY + (objectCollider != null ? objectCollider.bounds.extents.y + Mathf.Abs(objectCollider.offset.y) : spawnPoints[2].transform.position.y);
        Vector3 _floorSpawnPoint = new Vector3(spawnPoints[2].transform.position.x + xPosOffset, yPos, spawnPoints[2].transform.position.z); 
        return _floorSpawnPoint;                                         
    }

    public void changePoolSpawnChance(float top, float mid, float bot)
    {
        if(_currentRegionLevel.top) _currentRegionLevel.top.runTimeSpawnChanceValue = top;
        if(_currentRegionLevel.mid) _currentRegionLevel.mid.runTimeSpawnChanceValue = mid;
        if(_currentRegionLevel.bot) _currentRegionLevel.bot.runTimeSpawnChanceValue = bot;
    }

    public void UpdateLevelConfig(ObstaclePoolConfig newPoolConfig)
    {
        _currentRegionLevel = newPoolConfig;
        Initialize();
    }
    public void ClearActiveObstacles()
    {
        foreach(GameObject g in activeObjects)
        {
            g.SetActive(false);
            g.transform.parent = RegionPoolManager.Instance.transform;
        }
    }
} 
