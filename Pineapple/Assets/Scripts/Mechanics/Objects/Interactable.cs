using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable: MonoBehaviour
{
    virtual public void DoAction(GameObject player)
    {
        Debug.Log("Did Action");
    }
} 
