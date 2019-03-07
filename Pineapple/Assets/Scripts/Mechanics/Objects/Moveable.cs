using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Moveable : Interactable
{
    public bool holdable;
    public string deactiveButtonText = "Drop";
    public Enums.Weight playerMoveSpeedPenalty = Enums.Weight.lite;

    private bool _active = false;
    private Vector3 _playerOffset;
    private Rigidbody2D _thisRb;

    private GameObject _player;
    private CharacterController2D _playerCharController;
    private PlayerController _playerController;
    private HingeJoint2D _playerHingeJoint;

    private InteractButton _interactBtn;

    [SerializeField]
    private BoxCollider2D _baseCollider;

    private bool _grounded;

    void Start()
    {
        // TODO: set a default buttonBg?
        _grounded = !holdable;

        _thisRb = GetComponent<Rigidbody2D>();

        _player = GameObject.FindGameObjectWithTag("Player");
        _playerCharController = _player.GetComponent<CharacterController2D>();
        _playerController = _player.GetComponent<PlayerController>();
        _playerHingeJoint = _player.GetComponent<HingeJoint2D>();

        _interactBtn = GameObject
            .FindGameObjectWithTag("InteractBtn")
            .GetComponent<InteractButton>();

        reset();
    }

    private void reset()
    {
        _thisRb.bodyType = RigidbodyType2D.Kinematic;
        _thisRb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }

    private void FixedUpdate()
    {
        if (_active)
        {
            Collider2D[] colliders = new Collider2D[5];
            ContactFilter2D contactFilter = new ContactFilter2D();
            _baseCollider.OverlapCollider(contactFilter, colliders);

            int collisionCount = 0;

            foreach (Collider2D x in colliders)
            {
                if (x != null && x.name != _baseCollider.name)
                {
                    collisionCount++;
                }
            }

            if (collisionCount == 0)
            {
                _grounded = false;
            }
        }
        
    }

    private void Update()
    {
        //make player let go of object if either one is falling
        if (_active && !holdable && (!_playerCharController.m_Grounded || !_grounded))
        {
            MoveObject();
            if (!_grounded) _thisRb.velocity = _thisRb.velocity + Vector2.down;
        }
        else if (!_active && _thisRb.velocity.y >= 0f)
        {
            _grounded = true;
            _thisRb.velocity = Vector2.zero;
            _thisRb.bodyType = RigidbodyType2D.Kinematic;
            _thisRb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            
        }
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
        _playerController.speed -= (float)playerMoveSpeedPenalty; // TODO: Possibly want to change how speed is calculated?

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

        // Ensure that the player collider resets for the interation button
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
