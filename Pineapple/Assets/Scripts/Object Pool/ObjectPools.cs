using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChanceValue
{
    float Chance {get;}
}

public class ObjectPools : MonoBehaviour, IChanceValue
{
   public bool manual;
   /////////////////////////////
   public float spawnChanceValue;
   public float Chance{get {return spawnChanceValue;}}
   public ObjType objectType;
   public int duplicateAmount = 1;
   public List<GameObject> objectPool = new List<GameObject>();
   public List<GameObject> startTranstionPool = new List<GameObject>();
   public List<GameObject> endTranstionPool = new List<GameObject>();

   public List<GameObject> spawnedObjectPool = new List<GameObject>();
   [HideInInspector] public List<GameObject> spawnedStartTranstionPool = new List<GameObject>();
   [HideInInspector] public List<GameObject> spawnedEndTranstionPool = new List<GameObject>();

   void Awake()
   {
      if(!manual)
      {
         spawnedObjectPool.Clear();
         Initialize(objectPool, spawnedObjectPool, objectType);
      }
   }

   public void Initialize(List<GameObject> pool, List<GameObject> poolToAddTo, ObjType type)
   {
      foreach(GameObject p in pool)
      {
         for(int i = 0; i < duplicateAmount; i++)
         {
            GameObject tempPoolObj = Instantiate(p, transform.position, p.transform.rotation);
            tempPoolObj.SetActive(false);
            ObjectID id = tempPoolObj.AddComponent<ObjectID>() as ObjectID;
            id.CreateID(type, this.GetComponent<ObjectPools>());
            poolToAddTo.Add(tempPoolObj);
         }
      }
   }

   void AddObj(GameObject objToAdd)
   {
      objectPool.Add(objToAdd);
   }
}
