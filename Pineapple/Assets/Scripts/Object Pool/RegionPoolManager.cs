using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionPoolManager : MonoBehaviour
{ 
   public int duplicateAmount = 1; 
   public List<Region> RegionsToPool;
   public Dictionary<string, ObjectPools> panelPoolTypeDic = new Dictionary<string, ObjectPools>();

   void Awake()
   {
      Initialize();
   }

   public void Initialize()
   {
      foreach(Region r in RegionsToPool)
      {
         r.Initialise();
         panelPoolTypeDic.Add(r.tag, r.panels);
      }
   }
}
