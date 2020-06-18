using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChanceValue
{
    float Chance {get;}
}

[CreateAssetMenu(menuName = "Pools/Object Pool")]
public class ObjectPools : ScriptableObject, IChanceValue
{
   public bool manual;

   /////////////////////////////
   public float spawnChanceValue;
   public float Chance{get {return spawnChanceValue;}}
   public ObjType objectType;
   public int duplicateAmount = 1;
   public List<GameObject> objectPool = new List<GameObject>();
   public List<GameObject> spawnedObjectPool = new List<GameObject>();

   /*void Awake()
   {
      if(!manual)
      {  
         Initialize();
      }
   }*/

   public void Initialise()
   {
      spawnedObjectPool.Clear();
      foreach(GameObject p in objectPool)
      {
         for(int i = 0; i < duplicateAmount; i++)
         {
            GameObject tempPoolObj = Instantiate(p);
            tempPoolObj.SetActive(false);
            ObjectID id = tempPoolObj.AddComponent<ObjectID>() as ObjectID;
            id.CreateID(objectType);
            spawnedObjectPool.Add(tempPoolObj);
         }
      }
   }
   void AddObj(GameObject objToAdd)
   {
      objectPool.Add(objToAdd);
   }
}
