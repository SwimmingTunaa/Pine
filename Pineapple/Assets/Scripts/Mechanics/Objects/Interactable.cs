using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable: MonoBehaviour
{
    //change this to an icon
    public Sprite buttonBg;
    public string activateButtonText = "Move";

    virtual public void DoAction(GameObject player)
    {
        Debug.Log("Did Action");
    }
} 
