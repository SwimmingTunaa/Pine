using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    void Awake()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDestroy(){
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
 
    void OnLevelFinishedLoading (Scene previousScene, LoadSceneMode mode) 
    {
        //PanelSpawner.Instance.ClearPanelHolders();  
        PanelSpawner.Instance.InitialSpawn();
        MoveBlackBars.SetBlackBarHeight(DontDestroy._instance.startingPanel.GetComponentInChildren<SpriteRenderer>());
    }

    public static void MoveObjectsToNewScene()
    {
        foreach(GameObject go in PoolManager.instance.listOfPools[3].spawnedObjectPool)
        {
            go.transform.parent = PoolManager.instance.transform;
        }
        foreach(GameObject go in PoolManager.instance.listOfPools[7].spawnedObjectPool)
        {
            go.transform.parent = PoolManager.instance.transform;
        }
        ClearPanelHolders();
    }

    public static void ClearPanelHolders()
    {
        int botChildCount = PanelSpawner._Instance.bottomPanelHolder.transform.childCount;
        for(int i = 0; i < botChildCount; i ++)
        {
            PanelSpawner._Instance.bottomPanelHolder.transform.GetChild(0).gameObject.SetActive(false);
            PanelSpawner._Instance.bottomPanelHolder.transform.GetChild(0).SetParent(RegionPoolManager.Instance.gameObject.transform);          
        }
        int topChildCount = PanelSpawner._Instance.topPanelHolder.transform.childCount;
        for(int i = 0; i < topChildCount; i ++)
        {
            PanelSpawner._Instance.topPanelHolder.transform.GetChild(0).gameObject.SetActive(false);
            PanelSpawner._Instance.topPanelHolder.transform.GetChild(0).SetParent(RegionPoolManager.Instance.gameObject.transform);          
        }

        int curChildCount = PanelSpawner._Instance.currentPanelHolder.transform.childCount;
        for(int i = 0; i < curChildCount; i ++)
        {
            PanelSpawner._Instance.currentPanelHolder.transform.GetChild(0).gameObject.SetActive(false);
            PanelSpawner._Instance.currentPanelHolder.transform.GetChild(0).SetParent(RegionPoolManager.Instance.gameObject.transform);          
        }
        ObstacleSpawner.Instance.ClearActiveObstacles();
    }
}
