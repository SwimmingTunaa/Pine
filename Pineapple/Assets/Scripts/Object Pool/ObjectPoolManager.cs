using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{ 
   public int duplicateAmount = 1; 
   public List<ObjectPools> objectPools;
   public List<ObjectPools> spawnedObjectPool;   

   void Awake()
   {
      Initialize(objectPools, spawnedObjectPool);
   }

   public void Initialize(List<ObjectPools> pool, List<ObjectPools> poolToAddTo)
      {
         for(int i = 0; i < duplicateAmount; i++)
         {
            foreach(ObjectPools p in pool)
            {
               GameObject tempPoolObj = Instantiate(p.gameObject, transform.position, p.transform.rotation);
               tempPoolObj.SetActive(false);
               tempPoolObj.transform.parent = gameObject.transform;
               poolToAddTo.Add(tempPoolObj.GetComponent<ObjectPools>());
            }
         }
      }
}
