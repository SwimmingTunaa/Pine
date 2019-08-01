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
        Vector2 newSpawnPos = new Vector2((previousSpawnSPos.transform.position.x + (previousSpawnSPos.sprite.bounds.extents.x * previousSpawnSPos.gameObject.transform.localScale.x))
                                             + (nextSpawn.sprite.bounds.extents.x * nextSpawn.transform.localScale.x),
                                             previousSpawnSPos.transform.position.y);
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
