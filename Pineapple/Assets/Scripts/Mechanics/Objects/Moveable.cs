using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Moveable : Interactable
{
    public bool holdable;
    public string deactiveButtonText = "Drop";
    public Enums.Weight playerMoveSpeedPenalty = Enums.Weight.lite;
    public LayerMask canCollideWith;

    private bool _active = false;
    private Vector3 _playerOffset;
    private Rigidbody2D _thisRb;

    private GameObject _player;
    private CharacterController2D _playerCharController;
    private PlayerController _playerController;
    private HingeJoint2D _playerHingeJoint;

    private InteractButton _interactBtn;

    [SerializeField]
    private Vector2 _groundCheckPos;
    private Vector2 _groundedBoxSize;
    private bool _grounded;
    private SpriteRenderer sRenderer;

    void Start()
    {
        _thisRb = GetComponent<Rigidbody2D>();

        _player = GameObject.FindGameObjectWithTag("Player");
        _playerCharController = _player.GetComponent<CharacterController2D>();
        _playerController = _player.GetComponent<PlayerController>();
        _playerHingeJoint = _player.GetComponent<HingeJoint2D>();   
        
        sRenderer = GetComponentInChildren<SpriteRenderer>();
        //Size of the ground check box, it will scale to any sprite size
        _groundedBoxSize = new Vector2 (sRenderer.sprite.bounds.size.x * sRenderer.transform.localScale.x, sRenderer.sprite.bounds.max.y * sRenderer.transform.localScale.y * 0.03f);

        _interactBtn = GameObject
            .FindGameObjectWithTag("InteractBtn")
            .GetComponent<InteractButton>();

        reset();

        /* 
        TODO: make it fall faster based on weight?
        Pseudo:
        rb.gravityscale =* enum.weight 
        */
    }

    private void reset()
    {
        _thisRb.bodyType = RigidbodyType2D.Kinematic;
        _thisRb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }

    private void FixedUpdate()
    {
        //create an overlap box at the lowest point of sprite
        _groundCheckPos = getGroundCheckPos();
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_groundCheckPos, _groundedBoxSize, 0, canCollideWith);
         //cause it includes its own collider
        if(colliders.Length == 1 && colliders[0].gameObject == gameObject)
        {
            _grounded = false; 
            return;
        }
        
        if(!_grounded)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    _grounded = true;
                } 
            } 
        }
    }

    private void Update()
    {
        //make player let go of object if either one is falling
        if (_active && !holdable && (!_playerCharController.m_Grounded || !_grounded))
        {
            MoveObject();
        }
        else if (!_active && _grounded)
        {
            _thisRb.velocity = Vector2.zero;
            _thisRb.bodyType = RigidbodyType2D.Kinematic;
            _thisRb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        //makes sure the object continues falling even if not active
        else if(!_grounded)
        {
            _thisRb.bodyType = RigidbodyType2D.Dynamic;
            _thisRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }    
    }

    private Vector2 getGroundCheckPos()
    {
        return new Vector2(sRenderer.gameObject.transform.position.x, sRenderer.gameObject.transform.position.y - sRenderer.bounds.size.y/2);
    }

    public override void DoAction(GameObject player)
    {
        base.DoAction(player);
        MoveObject();
    }

    public void MoveObject()
    {
        _active = !_active;

        _playerHingeJoint.enabled = _active;
        _playerController.jumpable = !_active;
        _playerCharController.flippable = holdable ? true : !_active;

        if(_active)
        {
            grabMovable();
        }
        else
        {
            releaseMovable();
        }
    }

    private void grabMovable()
    {
        _playerController.speed -= (float)playerMoveSpeedPenalty;

        if (_thisRb.bodyType != RigidbodyType2D.Dynamic)
        {
            _thisRb.bodyType = RigidbodyType2D.Dynamic;
        }
        //parent the GO to the player so that it can flip with the player
        if(holdable)
        {
            gameObject.transform.parent = _player.transform;
        }    
        // Enable moving on any axis, but not rotate
        _thisRb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Connect this movable obj to the player
        _playerHingeJoint.connectedBody = _thisRb;

        // Update the interaction button to reflect our grabbing
        _interactBtn.setColor(_player, Enums.InteractColor.deactivate);
        _interactBtn.setText(_player, deactiveButtonText);
    }

    private void releaseMovable()
    {
        // Disconnect the player and the moveable obj
        _playerHingeJoint.connectedBody = null;

        // Return player speed to it's default
        _playerController.speed = _playerController.startSpeed;

        // Ensure that the player collider resets for the interaction button
        _player
            .GetComponent<Interact>()
            .resetColliding();
        //unparent the GO
        if(holdable)
        {
            gameObject.transform.parent = null;
        }   
    }

}
