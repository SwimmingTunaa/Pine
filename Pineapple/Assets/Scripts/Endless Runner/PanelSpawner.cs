using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PanelSpawner : Spawner
{
    public int intialSpawnAmount = 2;
    public GameObject startingPanel;
    public RegionPoolManager RegionPoolManager;
    public GameObject topPanelHolder;
    public GameObject currentPanelHolder;
    public GameObject bottomPanelHolder;

    public ObjectPools _pool;
    [HideInInspector] public bool _firstSpawn = true;
    private GameObject _nextPanelToSpawn;
    private GameObject _firstPanel;

    void Start()
    {
        InitialSpawn();
    }

    public override void DoSpawn()
    {
        if(_nextPanelToSpawn == null)
            return;

        SpriteRenderer nextSpawn = _nextPanelToSpawn.GetComponentInChildren<SpriteRenderer>();
        SpriteRenderer previousSprite = _previousSpawn.GetComponentInChildren<SpriteRenderer>();
        _nextPanelToSpawn.transform.position = GetNextSpawnPosHorizontal(previousSprite, nextSpawn);
        _nextPanelToSpawn.transform.parent = currentPanelHolder.transform;
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
        //get next Region
        RegionPoolManager.GetNextRegion();
        //get the current active region
        _pool = MasterSpawner.Instance.activeRegion.panels;
        //get the next panel to spawn
        _nextPanelToSpawn = GetNextItem(_pool.spawnedObjectPool);
        //spawn the start panels and then some queued panels afterwards
        for(int i = 0; i <= intialSpawnAmount; i++)
        {
            if(_firstSpawn)
            {
                Vector3 spawnPos = GetNextSpawnPosHorizontal(startingPanel.GetComponentInChildren<SpriteRenderer>(), _nextPanelToSpawn.GetComponentInChildren<SpriteRenderer>());
                FirstPanelSpawn(spawnPos);
                _firstSpawn = false;
            }
            else
            {
                _nextPanelToSpawn = GetNextItem(_pool.spawnedObjectPool);
                DoSpawn();
            }
        } 
        SpawnPanelsDown(); 
    }

    void SpawnPanelsDown()
    {
        var nextRegionPanelPool = RegionPoolManager.nextRegion.panels;
        bool initialSpawn = true;
        var _nextBelowPanel = GetNextItem(nextRegionPanelPool.spawnedObjectPool);
        GameObject previousPanel = null;

        for (int i = 0; i < 2; i++)
        {
            if(initialSpawn)
            {
                 Vector3 spawnPos = GetNextSpawnPosVertical(_firstPanel == null ? startingPanel.GetComponentInChildren<SpriteRenderer>() : 
                                                            _firstPanel.GetComponentInChildren<SpriteRenderer>(),
                                                            _nextBelowPanel.GetComponentInChildren<SpriteRenderer>());
                _nextBelowPanel.SetActive(true);
                _nextBelowPanel.transform.position = spawnPos;
                _nextBelowPanel.transform.parent = bottomPanelHolder.transform;
                previousPanel = _nextBelowPanel;
                _firstPanel = _nextBelowPanel;                
                _nextBelowPanel = GetNextItem(nextRegionPanelPool.spawnedObjectPool);

                initialSpawn = false;
            } 
            else
                {
                    SpriteRenderer nextSprite = _nextBelowPanel.GetComponentInChildren<SpriteRenderer>();
                    SpriteRenderer previousSprite = previousPanel.GetComponentInChildren<SpriteRenderer>();
                    _nextBelowPanel.transform.position = GetNextSpawnPosHorizontal(previousSprite, nextSprite);
                    _nextBelowPanel.SetActive(true);

                    //check to see if the gameobject is from the same region as the previous panel
                    if(!_nextBelowPanel.gameObject.CompareTag(previousPanel.gameObject.tag))
                            //change the pool to match the region
                            nextRegionPanelPool = RegionPoolManager.panelPoolTypeDic[_nextBelowPanel.gameObject.tag];

                    previousPanel = _nextBelowPanel;
                    _nextBelowPanel.transform.parent = bottomPanelHolder.transform;

                    //set a the last spawned panel to be the new starting panel
                    startingPanel = _nextBelowPanel; 

                    if(i + 1 <= 2) _nextBelowPanel = GetNextItem(nextRegionPanelPool.spawnedObjectPool);
                }
        }
    }

    void FirstPanelSpawn(Vector3 spawnPos)
    {
        _nextPanelToSpawn.SetActive(true);
        _nextPanelToSpawn.transform.position = spawnPos;
        _nextPanelToSpawn.transform.parent = currentPanelHolder.transform;
        _previousSpawn = _nextPanelToSpawn;
        //check to see if is previous panel matches the same region
        if(!_nextPanelToSpawn.gameObject.CompareTag(startingPanel.gameObject.tag))
            //change the pool to match the region
            _pool = RegionPoolManager.panelPoolTypeDic[_nextPanelToSpawn.gameObject.tag];
        _nextPanelToSpawn = GetNextItem(_pool.spawnedObjectPool);
    }

    public void SpawnSets()
    {
        _nextPanelToSpawn = GetNextItem(_pool.spawnedObjectPool);
        DoSpawn();
    }
}
