using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterSpawner : Spawner
{
    public ObjectPools critterPool;
    [MinMaxSlider(1,5)] public Vector2 amountToSpawn;
    public LayerMask whatIsGround;
    public bool changeSizeOnSpawn = true;
    private Vector3 groundPos;
    private RaycastHit2D hit;

    public override void DoSpawn()
    {
        hit = Physics2D.Raycast(gameObject.transform.position, -Vector3.up, 8f, whatIsGround);
        if(hit)
            groundPos = hit.point;

        if(critterPool.spawnedObjectPool.Count > 0)
        {
            int randomSpawnAmount = (int)Random.Range(amountToSpawn.x, amountToSpawn.y);
            Vector3 newSpawnPos = new Vector3(groundPos.x + Random.Range(-3, 2), groundPos.y, groundPos.z);
            for (int i = 0; i < randomSpawnAmount; i++)
            {
                GameObject tempObj =  critterPool.GetNextItem();
                tempObj.SetActive(true);
                if(changeSizeOnSpawn)
                {
                    //reset the scale
                    tempObj.transform.localScale = Vector3.one;
                    //change the size of the critter
                    tempObj.transform.localScale *= Random.Range(1f, 1.4f);
                }
                //move the critter into spawn pos
                tempObj.transform.position = newSpawnPos;
            }
        }
       
    }
        
}
