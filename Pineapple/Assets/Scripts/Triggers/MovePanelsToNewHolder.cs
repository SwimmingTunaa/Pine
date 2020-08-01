using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PanelHolderLocation
{
    Top, Mid, Bot
}
public class MovePanelsToNewHolder : MonoBehaviour
{   
    public Region regionToChangeTo;
    public PanelHolderLocation panelHolderLocation;
    private Collider2D _newFloorSpawnPoint;
    private PanelHolderInfo _panelHolderInfo;
    private PanelSpawner _panelSpawner;
    private Dictionary<PanelHolderLocation, PanelHolderConfig> _configs;
    private int triggerAmount = 1;

    void Awake()
    {
        _panelHolderInfo = GameObject.FindObjectOfType<PanelHolderInfo>();
        _panelSpawner = GameObject.FindObjectOfType<PanelSpawner>();
    }
    void Start()
    {
        _configs = new Dictionary<PanelHolderLocation, PanelHolderConfig>
        {
            {PanelHolderLocation.Top, _panelHolderInfo.panelHolderConfigTop},
            {PanelHolderLocation.Mid, _panelHolderInfo.panelHolderConfigMid},
            {PanelHolderLocation.Bot, _panelHolderInfo.panelHolderConfigBot}
        };
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(triggerAmount > 0 && other.CompareTag("Player"))
        {
            _newFloorSpawnPoint = _configs[panelHolderLocation].startingPanel.GetComponent<Collider2D>();
            ChangePanelSpawnPoint(_configs[panelHolderLocation]);
        }
    }

    void ChangePanelSpawnPoint(PanelHolderConfig panelSetToChangeTo)
    {
        triggerAmount--;
        //remove the trigger panel so it doesnt spawn again
        //TODO: change this when adding in a new panel set 
        //_panelSpawner._pool.spawnedObjectPool.RemoveAll(gameObject => gameObject.name == "Bathroom");
        //parent the current panel holder to the camera follow
        _panelSpawner.panelHolder.transform.parent = GameObject.FindObjectOfType<FollowCamera>().transform;
        //set up the new panel holder
        _panelSpawner.panelHolder = panelSetToChangeTo.parentPanelHolder;
        //unparent the current panel holder
        _panelSpawner.panelHolder.transform.parent = null;

        //setup the new floor spawnpoint for the obstacle spawner

        ObstacleSpawner.floorCollider = _newFloorSpawnPoint;
        MasterSpawner.Instance.activeRegion = regionToChangeTo;
        //setup the spawner to spawn at the new starting panel
        _panelSpawner.startingPanel = panelSetToChangeTo.startingPanel;
        _panelSpawner._firstSpawn = true;
        _panelSpawner.InitialSpawn();
    }
}
