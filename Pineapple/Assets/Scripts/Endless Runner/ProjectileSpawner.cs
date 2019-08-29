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
    
    private float _halfHeight;
    private float _halfWidth;
    private float _newY;

    void Awake()
    {
        projectilePool = Instantiate(projectilePool, transform.position, transform.rotation);
    }

    void Update()
    {
        //bounce this obj up and down
        _newY = Mathf.Sin(moveSpeed * Time.time) * yAmplitude;
        transform.position = new Vector3(transform.position.x, _newY, transform.position.z);
    }
    
    public override void DoSpawn()
    {
        StartCoroutine(RandomSpawnType());
    }

    public IEnumerator RandomSpawnType()
    {
        _halfHeight = Camera.main.orthographicSize;
        _halfWidth  = Camera.main.aspect * _halfHeight; 
        spawnType = (SpawnType)Random.Range(0, System.Enum.GetValues(typeof(SpawnType)).Length);
        switch(spawnType)
        {
            case SpawnType.line:
                SpawnLine();
            break;
            case SpawnType.wave:
                StartCoroutine(SpawnWave());
            break;
        }
        masterSpawner.enabled = false;
        yield return new WaitForSeconds(0.75f);
        masterSpawner.enabled = true;
    }

    public void SpawnLine()
    {
        int r = (int)Random.Range(spawnAmount.x,spawnAmount.y);
        for(int i = 0; i < r; i++)
        {
            StartCoroutine(SpawnObject(Random.Range(-yAmplitude, yAmplitude)));
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

    IEnumerator SpawnObject(float yPos)
    {
        GameObject o = projectilePool.spawnedObjectPool[Random.Range(0,projectilePool.spawnedObjectPool.Count)];
        projectilePool.spawnedObjectPool.Remove(o);
        yield return StartCoroutine(Warning(o,yPos));
        o.SetActive(true);
        o.transform.position = new Vector3(Camera.main.transform.position.x + _halfWidth, yPos, transform.position.z);
    }

    IEnumerator Warning(GameObject obj, float yPos)
    {
        GameObject w = obj.GetComponent<SpawnWarningBubble>().warningBubbleClone;
        if(!w.activeInHierarchy)
        {
            w.SetActive(true);
            w.transform.position = new Vector3(Camera.main.transform.position.x + _halfWidth, yPos, transform.position.z);
            yield return new WaitForSeconds(1.5f);
            w.SetActive(false);
        }
    }
}
