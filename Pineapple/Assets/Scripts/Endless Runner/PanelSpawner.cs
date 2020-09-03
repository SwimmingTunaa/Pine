using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PanelSpawner : Spawner
{
    public static PanelSpawner Instance;
    public int intialSpawnAmount = 2;
    public GameObject startingPanel;
    public RegionPoolManager RegionPoolManager;
    public GameObject topPanelHolder;
    public GameObject currentPanelHolder;
    public GameObject bottomPanelHolder;
    public SpriteRenderer blackBarTop, blackBarBot;
    public float barTransitionSpeed = 40f;

    private bool _changeBlackBarHeight;
    public ObjectPools _pool;
    [HideInInspector] public bool _firstSpawn = true;
    private GameObject _nextPanelToSpawn;
    private GameObject _firstPanel;
    private float panelHalfSize;
    [HideInInspector] public GameObject _currentStartingPanel;

    void Awake()
    {
        if(Instance != this)
            Instance = this;
    }

    void Start()
    {
        InitialSpawn();
    }

    void LateUpdate()
    {

        if(_changeBlackBarHeight) ChangeBlackBarHeight();
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
        {
            //change the pool to match the region
                _pool = RegionPoolManager.regionDic[_nextPanelToSpawn.gameObject.tag].panels;
            //change the region to match the next set of panels
                //MasterSpawner.Instance.activeRegion = RegionPoolManager.regionDic[_nextPanelToSpawn.gameObject.tag];
        }
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
        SetBlackBarHeight();
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
                    {
                        //change the pool to match the region
                        nextRegionPanelPool = RegionPoolManager.regionDic[_nextBelowPanel.gameObject.tag].panels;
                    }
                    previousPanel = _nextBelowPanel;
                    _nextBelowPanel.transform.parent = bottomPanelHolder.transform;
                    //set a the last spawned panel to be the new starting panel
                    startingPanel = _nextBelowPanel; 

                    if(i + 1 <= 2) _nextBelowPanel = GetNextItem(nextRegionPanelPool.spawnedObjectPool);
                }
        }
    }

    public void SetBlackBarHeight()
    {
        panelHalfSize = startingPanel.GetComponentInChildren<SpriteRenderer>().size.y/2;
        _changeBlackBarHeight = true;
        _currentStartingPanel = startingPanel;
      
    }

    void ChangeBlackBarHeight()
    {
        Vector3 topNewYPos = new Vector3(blackBarTop.transform.position.x, _currentStartingPanel.transform.position.y + panelHalfSize + blackBarTop.bounds.extents.y);
        Vector3 botNewYPos = new Vector3(blackBarTop.transform.position.x, _currentStartingPanel.transform.position.y - panelHalfSize - blackBarBot.bounds.extents.y);
        blackBarTop.transform.position = Vector3.MoveTowards(blackBarTop.transform.position, topNewYPos, Time.deltaTime* barTransitionSpeed);
        blackBarBot.transform.position = Vector3.MoveTowards(blackBarBot.transform.position, botNewYPos, Time.deltaTime* barTransitionSpeed);
          
        if(blackBarTop.transform.position == topNewYPos && blackBarBot.transform.position == botNewYPos) _changeBlackBarHeight = false;                                           
    }

    public void ChangePanelSpawnPoint()
    {
        //change the region
        MasterSpawner.Instance.activeRegion = RegionPoolManager.nextRegion;

        //remove all panels from the top panel holder and turn othe panels off
        
        int botChildCount = bottomPanelHolder.transform.childCount;
        int currentChildCount = currentPanelHolder.transform.childCount;
        int topChildCount = topPanelHolder.transform.childCount;

        //remove all panels from the top panel holder and turn other panels off
        //TODO: Make Timer
        for(int i = 0; i < topChildCount; i ++)
        {
           //StartCoroutine(DelayDisable(topPanelHolder.transform.GetChild(i).gameObject));
           //topPanelHolder.transform.GetChild(i).gameObject.SetActive(false);
            topPanelHolder.transform.GetChild(0).gameObject.SetActive(false);
            topPanelHolder.transform.GetChild(0).SetParent(null);
        }

        //move the current panels to the top panel holder
        for(int i = 0; i < currentChildCount; i ++)
        {
            if(i < 3)
            {
                currentPanelHolder.transform.GetChild(0).SetParent(topPanelHolder.transform);
            }
            else
            {
                currentPanelHolder.transform.GetChild(0).gameObject.SetActive(false);
                currentPanelHolder.transform.GetChild(0).SetParent(null);
            }            
        }

      

        //parent bot panel holder panels into the current panel holder
        for(int i = 0; i < botChildCount; i ++)
        {
            bottomPanelHolder.transform.GetChild(0).SetParent(currentPanelHolder.transform);
        }

        //the bot panel holder is set in the panel spawner script under the SpawnPanelsDown() method

        //setup the spawner to spawn at the new starting panel
        _firstSpawn = true;
        InitialSpawn();
    }

    IEnumerator DelayDisable(GameObject obj)
    {
        yield return new WaitForSeconds(1f);
        obj.SetActive(false);
        obj.transform.parent = null;
    }

    void FirstPanelSpawn(Vector3 spawnPos)
    {
        _nextPanelToSpawn.SetActive(true);
        _nextPanelToSpawn.transform.position = spawnPos;
        _nextPanelToSpawn.transform.parent = currentPanelHolder.transform;
        _previousSpawn = _nextPanelToSpawn;
        //check to see if is previous panel matches the same region
        if(!_nextPanelToSpawn.gameObject.CompareTag(startingPanel.gameObject.tag))
        {
            //change the pool to match the region
            _pool = RegionPoolManager.regionDic[_nextPanelToSpawn.gameObject.tag].panels;
            MasterSpawner.Instance.activeRegion = RegionPoolManager.regionDic[_nextPanelToSpawn.gameObject.tag];
        }
            
        _nextPanelToSpawn = GetNextItem(_pool.spawnedObjectPool);
    }

    public void SpawnSets()
    {
        _nextPanelToSpawn = GetNextItem(_pool.spawnedObjectPool);
        DoSpawn();
    }
}
