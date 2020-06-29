﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    
    public bool shouldExpand;
    private float _timer;
    [HideInInspector] public  GameObject _previousSpawn;

    public Vector2 GetNextSpawnPos(SpriteRenderer previousSpawnSPos, SpriteRenderer nextSpawn)
    {
        //get the half size of the previous sprite
        float previousX = (previousSpawnSPos.size.x/2 * previousSpawnSPos.gameObject.transform.localScale.x);
        //get the half size of the next sprite
        float nextX = (nextSpawn.size.x/2 * nextSpawn.transform.localScale.x);
        //translate the new sprite to the right by previous X
        Vector2 newSpawnPos = new Vector2((previousSpawnSPos.transform.position.x + previousX) + nextX, previousSpawnSPos.transform.parent.position.y);
        return newSpawnPos;
    }

    /*public bool SpawnTimer()
    {
        _timer += Time.deltaTime;
        if(_timer > spawnInterval)
        {
            _timer = 0f;
            return true;
        }
        return false;
    }*/

    public abstract void DoSpawn();

    public GameObject GetNextItem(List<GameObject> poolType)
    {
        GameObject objectToPool = null;
        for (int i = 0; i < poolType.Count; i++)
        {
            GameObject tempObj = poolType[Random.Range(0,poolType.Count)];
            objectToPool = tempObj;
            if(!tempObj.activeInHierarchy)
                return tempObj;
        }
        return null;
        /*if (shouldExpand) 
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            if(GetComponent<ObjectID>() == null)
                obj.GetComponent<ObjectID>().CreateID(ObjType.Obstacle);
            obj.SetActive(false);
            poolType.Add(obj);
            return obj;
        } 
        else 
            return null;*/
    }
}
