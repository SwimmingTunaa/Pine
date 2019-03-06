using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Moveable : Interactable
{
    public bool holdable;   //TODO: make player able to hold items without it falling when off the edge?
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

    private List<Collider2D> _collisions = new List<Collider2D>();
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

    private void Update()
    {
        /* TODO (OBJ FALL) Checking what colliders were found in Stay and Enter
        if (_active)
        {
            Debug.Log(_collisions.Count);
            foreach (Collider2D x in _collisions)
            {
                Debug.Log(x.tag);
                Debug.Log(x.name);
            }
        }
        */

        //make player let go of object if either one is falling
        if (_active && !holdable && (!_playerCharController.m_Grounded || !_grounded)) // TODO (OBJ FALL): if this has no colliders with on the floor
        {
            MoveObject();
        }
        else if (!_active && _thisRb.velocity.y >= 0f)
        {
            _thisRb.velocity = Vector2.zero;
            _thisRb.bodyType = RigidbodyType2D.Kinematic;
            _thisRb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.tag != "Player")
            _grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag != "Player")
            _grounded = true;
    }
    /* TODO (OBJ FALL) Work out if I'm no longer colliding with another below - maybe use a separate box collider at the bottom of the moveable obj to help?
    private void FixedUpdate()
    {
        _collisions.Clear();
    }

   

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name)
    }

    
    */

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
            StartCoroutine(releaseMovable());
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

    private IEnumerator releaseMovable()
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
        yield return new WaitForSeconds(.1f);


    }

}
