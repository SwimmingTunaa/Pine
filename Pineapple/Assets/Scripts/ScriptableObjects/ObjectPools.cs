using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChanceValue
{
    float Chance {get;}
}

[CreateAssetMenu(menuName = "Pools/Object Pool")]
public class ObjectPools : ScriptableObject, IChanceValue
{
   public bool manual;
   public bool shouldExpand;

   /////////////////////////////
   public float spawnChanceValue;
   public float runTimeSpawnChanceValue{get; set;}
   public float Chance{get {return spawnChanceValue;}}
   public ObjType objectType;
   public int duplicateAmount = 1;
   public List<GameObject> objectPool = new List<GameObject>();
   public List<GameObject> spawnedObjectPool = new List<GameObject>();

   void OnEnable()
   {
      runTimeSpawnChanceValue = spawnChanceValue;
   }

   public void Initialise()
   {
      spawnedObjectPool.Clear();
      foreach(GameObject p in objectPool)
      {
         for(int i = 0; i < duplicateAmount; i++)
         {
            GameObject tempPoolObj = Instantiate(p);
            tempPoolObj.SetActive(false);
            tempPoolObj.transform.parent = RegionPoolManager.Instance.gameObject.transform;
            if(!tempPoolObj.GetComponent<ObjectID>())
            {
               ObjectID id = tempPoolObj.AddComponent<ObjectID>() as ObjectID;
               id.CreateID(objectType);
            }
            spawnedObjectPool.Add(tempPoolObj);
         }
      }
   }
   void AddObj(GameObject objToAdd)
   {
      objectPool.Add(objToAdd);
   }
   public GameObject GetNextItem()
    {
        GameObject objectToPool = null;
        for (int i = 0; i < spawnedObjectPool.Count; i++)
        {
            GameObject tempObj = spawnedObjectPool[Random.Range(0,spawnedObjectPool.Count)];
            objectToPool = tempObj;
            if(!tempObj.activeSelf)
                return tempObj;
        }
        if (shouldExpand) 
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.transform.SetParent(RegionPoolManager.Instance.gameObject.transform);
            if(obj.GetComponent<ObjectID>() == null)
                obj.GetComponent<ObjectID>().CreateID(ObjType.Obstacle);
            obj.SetActive(false);
            spawnedObjectPool.Add(obj);
            return obj;
        } 
        else 
            return null;
    }
}
