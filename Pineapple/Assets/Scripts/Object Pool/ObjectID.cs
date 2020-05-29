using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjType{
   Panel,
   Obstacle,
   Sticker,
   Pickups,
   Critter
}

public class ObjectID : MonoBehaviour
{
   public ObjType objectType;
   public ObjectPools parentPool;
   public bool selfDestroy;
   
   public void CreateID(ObjType type, ObjectPools pool)
   {
      objectType = type;
      parentPool = pool;
   }

   public void Disable()
   {
      if(!parentPool.spawnedObjectPool.Contains(gameObject))
         parentPool.spawnedObjectPool.Add(gameObject);
      //remove this panel from the parent panel holder
      gameObject.transform.parent = null;
      foreach(Transform t in transform)
      {
         t.gameObject.SetActive(true);
      }
      gameObject.SetActive(false);

   }
}
