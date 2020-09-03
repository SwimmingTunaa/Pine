using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RegionPoolManager : MonoBehaviour
{ 
   public int duplicateAmount = 1; 
   public List<Region> primaryRegionsToPool;
   public List<Region> secondaryRegions;
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
      while(r)
      {
         int index = Random.Range(0, primaryRegionsToPool.Count);
         if(MasterSpawner.Instance.activeRegion != primaryRegionsToPool[index]) 
         {
            nextRegion = primaryRegionsToPool[index];
            r = false;
         }
      }
   }

   public void Initialize()
   {
      foreach(Region r in primaryRegionsToPool)
      {
         r.Initialise();
         regionDic.Add(r.tag, r);
      }

       foreach(Region r in secondaryRegions)
      {
         r.Initialise();
         regionDic.Add(r.tag, r);
      }
      
   }
}
