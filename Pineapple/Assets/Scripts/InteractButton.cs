using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractButton : MonoBehaviour
{
    public Interactable InteractableObject;
    public CharacterController2D charController;
    private GameObject _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");        
    }
    public void DoAction()
    {   
        if(InteractableObject == null)
        {
            charController.Jump(true);
        }else
            InteractableObject.DoAction(_player);
    }
}
