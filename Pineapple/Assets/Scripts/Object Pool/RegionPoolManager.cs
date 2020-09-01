using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RegionPoolManager : MonoBehaviour
{ 
   public int duplicateAmount = 1; 
   public List<Region> RegionsToPool;
   public Dictionary<string, Region> regionDic = new Dictionary<string, Region>();
   public static Region nextRegion;

   void Awake()
   {
      Initialize();
      nextRegion = null;
   }

   public void GetNextRegion()
   {
      bool r = true;
      int index = Random.Range(0, RegionsToPool.Count);
      while(r)
      {
         if(MasterSpawner.Instance.activeRegion != RegionsToPool[index]) 
         {
            nextRegion = RegionsToPool[index];
            r = false;
         }
      }
   }

   public void Initialize()
   {
      foreach(Region r in RegionsToPool)
      {
         r.Initialise();
         regionDic.Add(r.tag, r);
      }
   }
}
