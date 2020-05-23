using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    public float spawnInterval = 1;
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

    public bool SpawnTimer()
    {
        _timer += Time.deltaTime;
        if(_timer > spawnInterval)
        {
            _timer = 0f;
            return true;
        }
        return false;
    }

    public abstract void DoSpawn();
}
