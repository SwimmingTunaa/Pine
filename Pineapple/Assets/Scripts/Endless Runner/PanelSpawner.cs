using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSpawner : MonoBehaviour
{
    public GameObject startingPanel;
    public float spawnSpeed = 1;
    public ObjectPoolManager objectPoolManager;

    private ObjectPools _pool;
    private float _timer;
    private bool _firstSpawn = true;
    private  GameObject _previousSpawn;
    private List<GameObject> _panelsToSpawn = new List<GameObject>();

    void Awake()
    {
      
    }
    void Start()
    {
          //get the begining pool which is the house pool
        _pool = objectPoolManager.spawnedObjectPool[0];
        InitialSpawn();
    }

    void SpawnPanels()
    {
        if(_panelsToSpawn == null || _panelsToSpawn.Count == 0)
            return;
        
        SpriteRenderer nextSpawn = _panelsToSpawn[0].GetComponentInChildren<SpriteRenderer>();
        SpriteRenderer previousSprite = _previousSpawn.GetComponentInChildren<SpriteRenderer>();
        if(!_pool.spawnedObjectPool.Contains(nextSpawn.transform.parent.gameObject))
        {
            _panelsToSpawn[0].transform.position = GetNextSpawnPos(previousSprite, nextSpawn);
            _panelsToSpawn[0].SetActive(true);
            _previousSpawn = _panelsToSpawn[0];
            _panelsToSpawn.Remove(_previousSpawn);
        } 
    }
    
    void InitialSpawn()
    {   
        //spawn the start panels and then some queued panles afterwards
        for(int i = 0; i <= 3; i++)
        {
            SetPanels(_pool.spawnedObjectPool);
            if(_firstSpawn)
            {
                _panelsToSpawn[0].transform.position = GetNextSpawnPos(startingPanel.GetComponentInChildren<SpriteRenderer>(),
                                                                    _panelsToSpawn[0].GetComponentInChildren<SpriteRenderer>());
                _panelsToSpawn[0].SetActive(true);
                _previousSpawn = _panelsToSpawn[0];
                _panelsToSpawn.RemoveAt(0);
                _firstSpawn = false;
            }
            SpawnPanels();
            SetPanels(_pool.spawnedObjectPool);
        }
        
    }

    public void DestroyPanels(GameObject other, List<GameObject> poolType)
    {
        other.SetActive(false);
        poolType.Add(other);
        SetPanels(_pool.spawnedObjectPool);
        SpawnPanels();
    }

    Vector2 GetNextSpawnPos(SpriteRenderer previousSpawnSPos, SpriteRenderer nextSpawn)
    {
        Vector2 newSpawnPos = new Vector2((previousSpawnSPos.transform.position.x + (previousSpawnSPos.sprite.bounds.extents.x * previousSpawnSPos.gameObject.transform.localScale.x))
                                             + (nextSpawn.sprite.bounds.extents.x * nextSpawn.transform.localScale.x),
                                             previousSpawnSPos.transform.position.y);
        return newSpawnPos;
    }

    bool SpawnTimer()
    {
        _timer += Time.deltaTime;
        if(_timer > spawnSpeed)
        {
            _timer = 0f;
            return true;
        }
        return false;
    }

    void SetPanels(List<GameObject> poolType)
    {
        GameObject tempObj = poolType[Random.Range(0,poolType.Count)];
        if(!_panelsToSpawn.Contains(tempObj))
        {
            _panelsToSpawn.Add(tempObj);
            poolType.Remove(tempObj);
        } 
    }
}
