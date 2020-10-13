using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static DontDestroy _instance;
    public GameObject startingPanel;
    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            //DontDestroyOnLoad(this);
        }
        else
            Destroy(this.gameObject);
    }

    void Start()
    {
      
    }
}
