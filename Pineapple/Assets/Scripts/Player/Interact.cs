using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    public float InteractDistance;
    public InteractButton interactButton;

    private CharacterController2D _char2D;

    void Start()
    {
        _char2D = GetComponent<CharacterController2D>();
    }
    void Update()
    {
        //make player let go of object if they are falling
        if(!_char2D.m_Grounded && GetComponent<HingeJoint2D>().enabled && GetComponent<HingeJoint2D>().connectedAnchor != null)
        {
            interactButton.InteractableObject.DoAction(gameObject);
        }
        RayCast();  
    }
    void RayCast()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3.up * 0.5f), Vector2.right * transform.localScale.x, InteractDistance);
        Debug.DrawRay(transform.position + (Vector3.up * 0.5f), Vector2.right * transform.localScale.x * InteractDistance, Color.red);

        if(hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable") && _char2D.m_Grounded)
        {
            interactButton.InteractableObject = hit.collider.gameObject.GetComponent<Interactable>();
            interactButton.GetComponentInChildren<Text>().text = hit.collider.gameObject.GetComponent<Interactable>().buttonText;
        }
        else
        {
            interactButton.InteractableObject = null;
            interactButton.GetComponentInChildren<Text>().text = "Jump";
        }
    }
}


