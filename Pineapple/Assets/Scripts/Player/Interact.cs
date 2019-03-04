using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interact : MonoBehaviour
{
    public float InteractDistance;
    public InteractButton interactButton;

    [SerializeField]
    private Image _interactButtonBg;
    [SerializeField]
    private TextMeshProUGUI _interactButtonText;

    private CharacterController2D _char2D;
    private HingeJoint2D _charHingeJoint;

    void Start()
    {
        _char2D = GetComponent<CharacterController2D>();
        _charHingeJoint = GetComponent<HingeJoint2D>();
    }

    void Update()
    {
        //make player let go of object if they are falling
        if(!_char2D.m_Grounded && _charHingeJoint.enabled && _charHingeJoint.connectedAnchor != null)
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

        Image btnImg = interactButton.GetComponentInChildren<Image>();


        if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable") && _char2D.m_Grounded)
        {
            // Set the GUI Interaction Button with the collided Interactable
            Interactable i = hit.collider.gameObject.GetComponent<Interactable>();
            interactButton.InteractableObject = i;
            _interactButtonText.text = i.buttonText;

            if (i.buttonBg)
            {
                _interactButtonBg.sprite = i.buttonBg;
                _interactButtonBg.color = new Color(255, 255, 255);
            }
            else
            {
                SpriteRenderer sRenderer = i.GetComponentInChildren<SpriteRenderer>();
                _interactButtonBg.sprite = sRenderer.sprite;
                _interactButtonBg.color = sRenderer.color;
            }
            _interactButtonBg.enabled = true;

            btnImg.color = Color.green;
        }
        else
        {
            btnImg.color = Color.white;
            interactButton.InteractableObject = null;
            _interactButtonBg.enabled = false;
            _interactButtonText.text = "Jump";
        }
    }
}


