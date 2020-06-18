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
   public bool selfDestroy;
   
   public void CreateID(ObjType type)
   {
      objectType = type;
   }

   public void Disable()
   {
      //remove this panel from the parent panel holder
      foreach(Transform t in transform)
      {
         //turns on all child object
         t.gameObject.SetActive(true);
      }
      gameObject.SetActive(false);
   }
}
