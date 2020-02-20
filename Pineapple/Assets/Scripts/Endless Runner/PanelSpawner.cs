using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PanelSpawner : Spawner
{
    public GameObject startingPanel;
    public ObjectPoolManager objectPoolManager;
    public GameObject panelHolder;

    [HideInInspector]public ObjectPools _pool;
    [HideInInspector] public bool _firstSpawn = true;
    private List<GameObject> _panelsToSpawn = new List<GameObject>();

    void Start()
    {
          //get the begining pool which is the house pool
        _pool = objectPoolManager.spawnedObjectPool[0];
        InitialSpawn();
    }

    public override void DoSpawn()
    {
        if(_panelsToSpawn == null || _panelsToSpawn.Count == 0)
            return;
        
        SpriteRenderer nextSpawn = _panelsToSpawn[0].GetComponentInChildren<SpriteRenderer>();
        SpriteRenderer previousSprite = _previousSpawn.GetComponentInChildren<SpriteRenderer>();
        if(!_pool.spawnedObjectPool.Contains(nextSpawn.transform.parent.gameObject))
        {
            _panelsToSpawn[0].transform.position = GetNextSpawnPos(previousSprite, nextSpawn);
            _panelsToSpawn[0].transform.parent = panelHolder.transform;
            _panelsToSpawn[0].SetActive(true);
            _previousSpawn = _panelsToSpawn[0];
            _panelsToSpawn.Remove(_previousSpawn);
        } 
    }
    
    public void InitialSpawn()
    {   
        //spawn the start panels and then some queued panels afterwards
        for(int i = 0; i <= 4; i++)
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
            DoSpawn();
            SetPanels(_pool.spawnedObjectPool);
        }  
    }

    public void SpawnSets()
    {
        SetPanels(_pool.spawnedObjectPool);
        DoSpawn();
    }
    
    public void SetPanels(List<GameObject> poolType)
    {
        GameObject tempObj = poolType[Random.Range(0,poolType.Count)];
        if(!_panelsToSpawn.Contains(tempObj))
        {
            _panelsToSpawn.Add(tempObj);
            poolType.Remove(tempObj);
        } 
    }
}
