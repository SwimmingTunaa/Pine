using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RegionPoolManager : MonoBehaviour
{
   public static RegionPoolManager _Instance; 
   public static RegionPoolManager Instance{get{return _Instance;}} 
   public int duplicateAmount = 1; 
   public List<Region> primaryRegionsToPool;
   public List<Region> secondaryRegions;
   public Dictionary<string, Region> regionDic = new Dictionary<string, Region>();
   public Region nextRegion;

   void Awake()
   {
      if(_Instance == null)
      {
         _Instance = this;
         Initialize();
         //DontDestroyOnLoad(this);
      }
      else
         Destroy(this.gameObject);
         
      foreach(Transform t in gameObject.transform)
      {
         t.gameObject.SetActive(false);
      }
      nextRegion = null;
      GetNextRegion();
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
