using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPools : MonoBehaviour
{
   public List<GameObject> objectPool = new List<GameObject>();
   public List<GameObject> startTranstionPool = new List<GameObject>();
   public List<GameObject> endTranstionPool = new List<GameObject>();

    public List<GameObject> spawnedObjectPool = new List<GameObject>();
   [HideInInspector] public List<GameObject> spawnedStartTranstionPool = new List<GameObject>();
   [HideInInspector] public List<GameObject> spawnedEndTranstionPool = new List<GameObject>();

   void Awake()
   {
      Initialize(objectPool, spawnedObjectPool);
   }

   public void Initialize(List<GameObject> pool, List<GameObject> poolToAddTo)
   {
      foreach(GameObject p in pool)
      {
         GameObject tempPoolObj = Instantiate(p, transform.position, p.transform.rotation);
         tempPoolObj.SetActive(false);
         ObjectID id = tempPoolObj.AddComponent<ObjectID>() as ObjectID;
         id.nameID = gameObject.name;
         poolToAddTo.Add(tempPoolObj);
      }
   }
}
