using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePanelsToNewHolder : MonoBehaviour
{   
    private PanelSpawner _panelSpawner;
    private int triggerAmount = 1;

    void Awake()
    {
        _panelSpawner = GameObject.FindObjectOfType<PanelSpawner>();
    }

    void OnEnable()
    {
        triggerAmount = 1;
    }
  
    void OnTriggerEnter2D(Collider2D other)
    {
        if(triggerAmount > 0 && other.CompareTag("Player"))
        {
            GameManager.Instance.cameraFollower.move = true;

            ChangePanelSpawnPoint();
            if(MasterSpawner.Instance.activeRegion.tag == "Cloud")
                other.GetComponent<CharacterController2D>().isFlying = true;
            else if (other.GetComponent<CharacterController2D>().isFlying == true && MasterSpawner.Instance.activeRegion.tag != "Cloud")
            {
                other.GetComponent<CharacterController2D>().isFlying = false;
            }
        }
    }

    void ChangePanelSpawnPoint()
    {
        //change the region
        MasterSpawner.Instance.activeRegion = RegionPoolManager.nextRegion;

        //remove all panels from the top panel holder and turn othe panels off
        int topChildCount = _panelSpawner.topPanelHolder.transform.childCount;
        int botChildCount = _panelSpawner.bottomPanelHolder.transform.childCount;
        int currentChildCount = _panelSpawner.currentPanelHolder.transform.childCount;
        for(int i = 0; i < topChildCount; i ++)
        {
            _panelSpawner.topPanelHolder.transform.GetChild(0).gameObject.SetActive(false);
            _panelSpawner.topPanelHolder.transform.GetChild(0).parent = null; 
        }

        //move the current panels to the top panel holder
        for(int i = 0; i < currentChildCount; i ++)
        {
            if(i < 3)
            {
                 _panelSpawner.currentPanelHolder.transform.GetChild(0).gameObject.SetActive(false);
                _panelSpawner.currentPanelHolder.transform.GetChild(0).SetParent(_panelSpawner.topPanelHolder.transform);
            }
            else
            {
                _panelSpawner.currentPanelHolder.transform.GetChild(0).gameObject.SetActive(false);
                _panelSpawner.currentPanelHolder.transform.GetChild(0).SetParent(null);
            }            
        }

        //parent bot panel holder panels into the current panel holder
        for(int i = 0; i < botChildCount; i ++)
        {
            _panelSpawner.bottomPanelHolder.transform.GetChild(0).SetParent(_panelSpawner.currentPanelHolder.transform);
        }

        //the bot panel holder is set in the panel spawner script under the SpawnPanelsDown() method

        //setup the spawner to spawn at the new starting panel
        _panelSpawner._firstSpawn = true;
        _panelSpawner.InitialSpawn();
        triggerAmount--;
    }
}
