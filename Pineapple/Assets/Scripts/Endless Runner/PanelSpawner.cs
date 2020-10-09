using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PanelSpawner : Spawner
{
    public static PanelSpawner _Instance;
    public static PanelSpawner Instance{get{return _Instance;}}
    public int intialSpawnAmount = 2;
    public GameObject startingPanel;
    public GameObject topPanelHolder;
    public GameObject currentPanelHolder;
    public GameObject bottomPanelHolder;

    public ObjectPools _pool;
    [HideInInspector] public bool _firstSpawn = true;
    private GameObject _nextPanelToSpawn;
    private GameObject _firstPanel;
    public GameObject _currentStartingPanel;
    public GameObject _originalStartingPanel;

    void Awake()
    {
        if(_Instance == null)
        {
            _Instance = this;
            //DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }

        startingPanel = DontDestroy._instance.startingPanel;
        _firstSpawn = true;
        //currentPanelHolder = DontDestroy._instance.panelHolderCurrent;
        // topPanelHolder = DontDestroy._instance.panelHolderTop;
        // bottomPanelHolder = DontDestroy._instance.panelHolderBot;
    }

    public override void DoSpawn()
    {
        if(_nextPanelToSpawn == null)
            return;

        SpriteRenderer nextSpawn = _nextPanelToSpawn.GetComponentInChildren<SpriteRenderer>();
        SpriteRenderer previousSprite = _previousSpawn.GetComponentInChildren<SpriteRenderer>();
        //move panel to position
        _nextPanelToSpawn.transform.position = GetNextSpawnPosHorizontal(previousSprite, nextSpawn);
        _nextPanelToSpawn.transform.parent = currentPanelHolder.transform;
        //spawn panel
        _nextPanelToSpawn.SetActive(true);

        //check to see if the gameobject is from the same region as the previous panel
        if(!_nextPanelToSpawn.gameObject.CompareTag(_previousSpawn.gameObject.tag))
        {
            //change the pool to match the region
                _pool = RegionPoolManager.Instance.regionDic[_nextPanelToSpawn.gameObject.tag].panels;
//                print("Panel Tag: " + _nextPanelToSpawn.tag);
            //change the region to match the next set of panels
                //MasterSpawner.Instance.activeRegion = RegionPoolManager.regionDic[_nextPanelToSpawn.gameObject.tag];
        }
        _previousSpawn = _nextPanelToSpawn;
        _nextPanelToSpawn = _pool.GetNextItem(); 
    }
    
    
    public void InitialSpawn()
    {   
        //get next Region
        RegionPoolManager.Instance.GetNextRegion();
        //get the current active region
        _pool = MasterSpawner.Instance.activeRegion.panels;
        //get the next panel to spawn
        _nextPanelToSpawn = _pool.GetNextItem(); 
        //spawn the start panels and then some queued panels afterwards
        for(int i = 0; i <= intialSpawnAmount; i++)
        {
            if(_firstSpawn)
            {
                Vector3 spawnPos = GetNextSpawnPosHorizontal(startingPanel.GetComponentInChildren<SpriteRenderer>(), _nextPanelToSpawn.GetComponentInChildren<SpriteRenderer>());
                FirstPanelSpawn(spawnPos);
                _firstSpawn = false;
                //_nextStartingPanel = startingPanel;
            }
            else
            {
                _nextPanelToSpawn = _pool.GetNextItem(); 
                DoSpawn();
            }
        } 
        _currentStartingPanel = startingPanel;
        SpawnPanelsDown(); 
    }
    
    void SpawnPanelsDown()
    {
        var nextRegionPanelPool = RegionPoolManager.Instance.nextRegion.panels;
        bool initialSpawn = true;
        var _nextBelowPanel = nextRegionPanelPool.GetNextItem(); 
        GameObject previousPanel = null;
        
        for (int i = 0; i < 2; i++)
        {
            if(initialSpawn)
            {
                 Vector3 spawnPos = GetNextSpawnPosVertical(_firstPanel == null ? startingPanel.GetComponentInChildren<SpriteRenderer>() : 
                                                            _firstPanel.GetComponentInChildren<SpriteRenderer>(),
                                                            _nextBelowPanel.GetComponentInChildren<SpriteRenderer>());
                _nextBelowPanel.transform.position = spawnPos;
                _nextBelowPanel.SetActive(true);
                _nextBelowPanel.transform.parent = bottomPanelHolder.transform;
                previousPanel = _nextBelowPanel;
                _firstPanel = _nextBelowPanel;                
                nextRegionPanelPool = RegionPoolManager.Instance.regionDic[_firstPanel.tag].panels;
                _nextBelowPanel = nextRegionPanelPool.GetNextItem(); 
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
                        nextRegionPanelPool = RegionPoolManager.Instance.regionDic[_nextBelowPanel.gameObject.tag].panels;
                    }
                    previousPanel = _nextBelowPanel;
                    _nextBelowPanel.transform.parent = bottomPanelHolder.transform;
                    //set a the last spawned panel to be the new starting panel
                    startingPanel = _nextBelowPanel; 
                    //set the new region to last panel spawned
                    RegionPoolManager.Instance.nextRegion = RegionPoolManager.Instance.regionDic[startingPanel.gameObject.tag];

                    if(i + 1 <= 2) _nextBelowPanel = nextRegionPanelPool.GetNextItem(); 
                }
        }
    }

    public void ChangePanelSpawnPoint()
    {
        //change the region
        MasterSpawner.Instance.activeRegion = RegionPoolManager.Instance.nextRegion;

        //remove all panels from the top panel holder and turn othe panels off
        
        int botChildCount = bottomPanelHolder.transform.childCount;
        int currentChildCount = currentPanelHolder.transform.childCount;

        List<GameObject> nextTopPanels = new List<GameObject>();
        for(int i = 0; i < currentChildCount; i ++)
        {
            if(i < 3)
            {
                nextTopPanels.Add(currentPanelHolder.transform.GetChild(0).gameObject);
                currentPanelHolder.transform.GetChild(0).SetParent(topPanelHolder.transform);
                StartCoroutine(DelayDisable(nextTopPanels[i]));
            }
            else
            {
                currentPanelHolder.transform.GetChild(0).gameObject.SetActive(false);
                currentPanelHolder.transform.GetChild(0).SetParent(RegionPoolManager._Instance.transform);
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

    public void ClearPanelHolders()
    {
        int topChildCount = topPanelHolder.transform.childCount;
        for(int i = 0; i < topChildCount; i ++)
        {
            topPanelHolder.transform.GetChild(0).gameObject.SetActive(false);
            topPanelHolder.transform.GetChild(0).SetParent(RegionPoolManager.Instance.gameObject.transform);          
        }

        int curChildCount = currentPanelHolder.transform.childCount;
        for(int i = 0; i < curChildCount; i ++)
        {
            currentPanelHolder.transform.GetChild(0).gameObject.SetActive(false);
            currentPanelHolder.transform.GetChild(0).SetParent(RegionPoolManager.Instance.gameObject.transform);          
        }
        
        int botChildCount = bottomPanelHolder.transform.childCount;
        for(int i = 0; i < botChildCount; i ++)
        {
            bottomPanelHolder.transform.GetChild(0).gameObject.SetActive(false);
            bottomPanelHolder.transform.GetChild(0).SetParent(RegionPoolManager.Instance.gameObject.transform);          
        }
    
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
            _pool = RegionPoolManager.Instance.regionDic[_nextPanelToSpawn.gameObject.tag].panels;
            MasterSpawner.Instance.activeRegion = RegionPoolManager.Instance.regionDic[_nextPanelToSpawn.gameObject.tag];
        }
            
        _nextPanelToSpawn = _pool.GetNextItem(); 
    }

    public void SpawnSets()
    {
        _nextPanelToSpawn = _pool.GetNextItem(); 
        DoSpawn();
    }
}
