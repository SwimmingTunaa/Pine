using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    public float InteractDistance;

    private InteractButton _interactBtn;
    private CharacterController2D _char2D;
    private bool _colliding = false;

    void Start()
    {
        _char2D = GetComponent<CharacterController2D>();

        _interactBtn = GameObject
            .FindGameObjectWithTag("InteractBtn")
            .GetComponent<InteractButton>();
    }

    void Update()
    {        
        RayCast();  
    }

    void RayCast()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3.up * 0.5f), Vector2.right * transform.localScale.x, InteractDistance);
        Debug.DrawRay(transform.position + (Vector3.up * 0.5f), Vector2.right * transform.localScale.x * InteractDistance, Color.red);

        // Simple checks to save performance
        if ((hit.collider != null && _colliding) || (hit.collider == null && !_colliding))
        {
            return;
        }

        if (hit.collider != null && 
            hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable") && 
            _char2D.m_Grounded) // TODO: Do we need to be grounded here? What if we are jumping to collect an item, or interact with a rope etc?
        {
            _colliding = true;
            // Set the GUI Interaction Button with the collided Interactable
            Interactable i = hit.collider.gameObject.GetComponent<Interactable>();

            // Do different stuff based on what type of interactable it is
            if (i is Moveable)
            {
                Moveable m = (Moveable)i;
                SpriteRenderer sRenderer = m.GetComponentInChildren<SpriteRenderer>();

                Sprite bg = m.buttonBg != null ? m.buttonBg : sRenderer.sprite;

                _interactBtn.set(gameObject, m.activateButtonText, bg, Enums.InteractColor.activate, m);
            }
        }
        else
        {
            _colliding = false;
            _interactBtn.resetBtn(gameObject);
        }
    }

    public void resetColliding()
    {
        _colliding = false;
    }
}


