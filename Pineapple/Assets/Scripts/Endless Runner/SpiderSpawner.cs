using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderSpawner : Spawner
{       
    public float spawnIntervalTime = 1;
    public static SpiderSpawner instance;

    [Header("Spawner Details")]
    public ObjectPools spiderPool;
    public GameObject spawnPoint;
    public AudioClip warningSound;
    public float warningTime = 1.5f;
    
    private float _halfHeight;
    private float _halfWidth;
    private float _newY;
    private Camera _camera;
    private Vector3 newSpawnPos = new Vector3();
    private RaycastHit2D _hit;

    void Awake()
    {
        instance = this;
        _camera = Camera.main;
    }

    void Start()
    {
        spiderPool.Initialise();
    }
    
    public override void DoSpawn()
    {
        StartCoroutine(SpawnObject());
    }

    IEnumerator SpawnObject()
    {
        _halfHeight = _camera.orthographicSize;
        _halfWidth  = _camera.aspect * _halfHeight; 
        GameObject nextSpawn = GetNextItem(spiderPool.spawnedObjectPool);
        //wait for this Warning to finish then continue
        yield return StartCoroutine(Warning(nextSpawn));
        nextSpawn.SetActive(true);
        newSpawnPos = new Vector3(_camera.transform.position.x - _halfWidth, _hit.point.y, transform.position.z);
        nextSpawn.transform.position = newSpawnPos - new Vector3(2,0,0);
    }

    IEnumerator Warning(GameObject obj)
    {
        GameObject wBubble = obj.GetComponent<SpawnWarningBubble>().warningBubbleClone;
        if(!wBubble.activeInHierarchy)
        {
            wBubble.SetActive(true);
            if(warningSound)
                GetComponent<AudioSource>().PlayOneShot(warningSound);
            //set the bubble to the side of the left screen
            _hit = Physics2D.Raycast(spawnPoint.transform.position, - Vector2.up, 8f, 1 << LayerMask.NameToLayer("Ground"));
            newSpawnPos = new Vector3(_camera.transform.position.x - _halfWidth, _hit.point.y, transform.position.z);
            wBubble.transform.position = newSpawnPos;
            wBubble.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + 180, transform.rotation.z);
            yield return new WaitForSeconds(warningTime);
            wBubble.SetActive(false);
        }
    }
}
