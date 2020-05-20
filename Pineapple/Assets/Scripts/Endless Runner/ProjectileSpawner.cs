using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum SpawnType
    {
        wave,
        line
    }
    
public class ProjectileSpawner : Spawner
{       
    public MasterSpawner masterSpawner;
    [Header("Object Pool")]
    public ObjectPools projectilePool;

    [Header("Spawner Details")]
    public SpawnType spawnType;
    public float yAmplitude; //how high and low the spawnermoves
    public float moveSpeed;
    [MinMaxSlider(1,5)] public Vector2 spawnAmount = new Vector2(1, 3);
    public AudioClip warningSound;
    
    [HideInInspector]public float disableOSpawnerTimer = 2;
    [HideInInspector]public float warningTimer = 1.5f;
    private float _halfHeight;
    private float _halfWidth;
    private float _newY;
    private Camera _camera;
    void Awake()
    {
        projectilePool = Instantiate(projectilePool, transform.position, transform.rotation);
        _camera = Camera.main;
    }

    void Update()
    {
        //bounce this obj up and down
        _newY = _camera.transform.position.y + Mathf.Sin(moveSpeed * Time.time) * yAmplitude;
        transform.position = new Vector3(transform.position.x, _newY, transform.position.z);
    }
    
    public override void DoSpawn()
    {
        StartCoroutine(RandomSpawnType());
    }

    public IEnumerator RandomSpawnType()
    {
        
        spawnType = (SpawnType)Random.Range(0, System.Enum.GetValues(typeof(SpawnType)).Length);
        if(masterSpawner != null) masterSpawner.enabled = false;
        switch(spawnType)
        {
            case SpawnType.line:
                SpawnLine();
            break;
            case SpawnType.wave:
                StartCoroutine(SpawnWaveHoming());
            break;
        }
        yield return new WaitForSeconds(disableOSpawnerTimer);
        if(masterSpawner != null) masterSpawner.enabled = true;
    }

    public void SpawnLine()
    {
        int r = (int)Random.Range(spawnAmount.x,spawnAmount.y);
        float yPos = _newY;
        for(int i = 0; i < r; i++)
        {
            StartCoroutine(SpawnObject(yPos));
            //check the distance between the yPos
            if(yPos + 1 < _camera.transform.position.y + yAmplitude)
                yPos += Random.Range(1.75f, 2.5f);
            else
                 yPos -= Random.Range(3f,5f);
        }
    }

    public IEnumerator SpawnWave()
    {
        int r = (int)Random.Range(spawnAmount.x,spawnAmount.y);
        for(int i = 0; i < r; i++)
        { 
           StartCoroutine(SpawnObject(_newY));
           yield return new WaitForSeconds(spawnInterval);
        }
    }

    public IEnumerator SpawnWaveHoming()
    {
        int r = (int)Random.Range(spawnAmount.x,spawnAmount.y);
        for(int i = 0; i < r; i++)
        { 
           StartCoroutine(SpawnObject(CharacterManager.activeCharacter.transform.position.y));
           yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator SpawnObject(float yPos)
    {
        _halfHeight = _camera.orthographicSize;
        _halfWidth  = _camera.aspect * _halfHeight; 
        GameObject o = projectilePool.spawnedObjectPool[Random.Range(0,projectilePool.spawnedObjectPool.Count)];
        projectilePool.spawnedObjectPool.Remove(o);
        //wait for this Warning to finish then continue
        yield return StartCoroutine(Warning(o,yPos));
        //reset trailrenderer
        o.GetComponentInChildren<TrailRenderer>().Clear();
        o.SetActive(true);
        o.transform.position = new Vector3(Camera.main.transform.position.x + _halfWidth, yPos, transform.position.z);
    }

    IEnumerator Warning(GameObject obj, float yPos)
    {
        GameObject w = obj.GetComponent<SpawnWarningBubble>().warningBubbleClone;
        if(!w.activeInHierarchy)
        {
            w.SetActive(true);
            GetComponent<AudioSource>().PlayOneShot(warningSound);
            w.transform.position = new Vector3(Camera.main.transform.position.x + _halfWidth, yPos, transform.position.z);
            yield return new WaitForSeconds(warningTimer);
            w.SetActive(false);
        }
    }
}
