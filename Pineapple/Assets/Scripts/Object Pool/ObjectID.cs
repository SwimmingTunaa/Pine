using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjType{
   Panel,
   Obstacle,
   Sticker,
   Pickups,
   Critter,
   Other
}

public class ObjectID : MonoBehaviour
{
   public ObjType objectType;
   public bool selfDestroy;
   
   public void CreateID(ObjType type)
   {
      objectType = type;
   }

   public void Disable()
   {
      foreach(Transform t in transform)
      {
         //turns on all child object
         t.gameObject.SetActive(true);
      }
      gameObject.SetActive(false);
   }
}
