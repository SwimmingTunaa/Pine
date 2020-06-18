using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PanelSpawner : Spawner
{
    public GameObject startingPanel;
    public RegionPoolManager RegionPoolManager;
    public GameObject panelHolder;

    public ObjectPools _pool;
    [HideInInspector] public bool _firstSpawn = true;
    private GameObject _nextPanelToSpawn;

    void Start()
    {
        //get the begining pool which is the house pool
        _pool = MasterSpawner.Instance.activeRegion.panels;
        InitialSpawn();
    }

    public override void DoSpawn()
    {
        if(_nextPanelToSpawn == null)
            return;

        SpriteRenderer nextSpawn = _nextPanelToSpawn.GetComponentInChildren<SpriteRenderer>();
        SpriteRenderer previousSprite = _previousSpawn.GetComponentInChildren<SpriteRenderer>();
        _nextPanelToSpawn.transform.position = GetNextSpawnPos(previousSprite, nextSpawn);
        _nextPanelToSpawn.transform.parent = panelHolder.transform;
        _nextPanelToSpawn.SetActive(true);
        //check to see if the gameobject is from the same region as the previous panel
        if(!_nextPanelToSpawn.gameObject.CompareTag(_previousSpawn.gameObject.tag))
                //change the pool to match the region
                _pool = RegionPoolManager.panelPoolTypeDic[_nextPanelToSpawn.gameObject.tag];
        _previousSpawn = _nextPanelToSpawn;
        _nextPanelToSpawn = GetNextItem(_pool.spawnedObjectPool); 
    }
    
    public void InitialSpawn()
    {   
        _nextPanelToSpawn = GetNextItem(_pool.spawnedObjectPool);
        //spawn the start panels and then some queued panels afterwards
        for(int i = 0; i <= 4; i++)
        {
            if(_firstSpawn)
            {
                _firstSpawn = false;
                _nextPanelToSpawn.transform.position = GetNextSpawnPos(startingPanel.GetComponentInChildren<SpriteRenderer>(),
                                                                    _nextPanelToSpawn.GetComponentInChildren<SpriteRenderer>());
                _nextPanelToSpawn.SetActive(true);
                _previousSpawn = _nextPanelToSpawn;
                if(!_nextPanelToSpawn.gameObject.CompareTag(startingPanel.gameObject.tag))
                    //change the pool to match the region
                    _pool = RegionPoolManager.panelPoolTypeDic[_nextPanelToSpawn.gameObject.tag];
                _nextPanelToSpawn = GetNextItem(_pool.spawnedObjectPool);
            }else
            {
                _nextPanelToSpawn = GetNextItem(_pool.spawnedObjectPool);
                DoSpawn();
            }
        }  
    }

    public void SpawnSets()
    {
        _nextPanelToSpawn = GetNextItem(_pool.spawnedObjectPool);
        DoSpawn();
    }
}
