using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    public List<ObjectPools> listOfPools = new List<ObjectPools>();

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            Invoke("InitialisePools", 0.01f);
        }
        else
            Destroy(this.gameObject);
    }

    void InitialisePools()
    {
        foreach(ObjectPools o in listOfPools)
        {
            o.Initialise();
        }
    }
}
