using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable: MonoBehaviour
{
    //change this to an icon
    public string buttonText;
    public Sprite buttonBg;

    virtual public void DoAction(GameObject player)
    {
        Debug.Log("Did Action");
    }
} 
