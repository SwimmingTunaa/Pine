using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cameraFollowTarget;
    public static bool jumpPressed;
    public bool pausePlayer;
    [HideInInspector] public float startSpeed;
    public float speed;
    public float maxSpeed;
 
    public bool jumpable = true;
    public bool immobile = false;

    [Header("Hair")]
    public GameObject slicedHair;
    public GameObject hairMask;
    public AudioClip hairSlicedAudio;
    [HideInInspector] public bool _haircut;

    public PlayerItemSlots playerItemSlots;

    [HideInInspector] public Animator _anim;
    [HideInInspector] public float _horiMove;
    private bool _jump = false;
    private CharacterController2D _characterController;
    private Rigidbody2D _rigidBody;

    void Awake()
    {
        _characterController = GetComponent<CharacterController2D>();
        _anim = GetComponentInChildren<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
        startSpeed = speed;
    }

    void FixedUpdate()
    {
        if(!pausePlayer)
        {
            setAnimations();
            _characterController.Move(speed * Time.fixedDeltaTime, false, _jump);
            _jump = false;
        }
    }
    public void OnPointerPressed(bool triggered)
    {
        Debug.Log("Jumped Pressed");
        if(triggered && !pausePlayer)
            setJump(1f); // was character controller
        jumpPressed = triggered;
    }

    public void setJump(float multiplier)
    {
        if (jumpable)
        {
            //_jump = true;
            _characterController.Jump(true, multiplier);
            _anim.SetBool(!_characterController.canDoubleJump ? "DoubleJump" : "Jump", true);
        }
    }

    public void OnLanding()
	{
		//to use with the OnLand event if we need anything to happen.
        //_anim.SetBool(!_characterController.canDoubleJump ? "DoubleJump" : "Jump", false);
	}

    private void setAnimations()
    {
        _anim.SetFloat("HoriMove", Mathf.Abs(_horiMove));
        _anim.SetFloat("yVelocity", _rigidBody.velocity.y);
        _anim.SetBool("Grounded", _characterController.m_Grounded);
        _anim.SetFloat("Speed", speed);
        if(_rigidBody.velocity.y < -2f)
        {
            _anim.SetBool("DoubleJump", false);
        }
    }

    public void AddForce(Vector2 forceAmount)
    {
        _rigidBody.velocity = Vector2.zero;
        _rigidBody.AddForce(forceAmount, ForceMode2D.Impulse);
        Debug.Log("Add Force");
    }

    public bool StoppedMoving()
    {
        return _rigidBody.velocity == Vector2.zero ? true : false;
    }
}
