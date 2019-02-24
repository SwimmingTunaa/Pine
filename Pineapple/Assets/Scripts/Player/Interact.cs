using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    public float InteractDistance;
    public InteractButton interactButton;

    void Update()
    {
        RayCast();
    }
    void RayCast()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3.up * 0.5f), Vector2.right * transform.localScale.x, InteractDistance);
        Debug.DrawRay(transform.position + (Vector3.up * 0.5f), Vector2.right * transform.localScale.x * InteractDistance, Color.red);

        if(hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable"))
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


