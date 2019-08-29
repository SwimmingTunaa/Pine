using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjType{
   Panel,
   Obstacle,
   Sticker,
   Pickups
}

public class ObjectID : MonoBehaviour
{
   public ObjType objectType;
   public ObjectPools parentPool;
   
   public void CreateID(ObjType type, ObjectPools pool)
   {
      objectType = type;
      parentPool = pool;
   }

   public void Disable()
   {
      if(!parentPool.spawnedObjectPool.Contains(gameObject))
         parentPool.spawnedObjectPool.Add(gameObject);
      gameObject.SetActive(false);
   }
}
