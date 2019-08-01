using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private struct Direction {
        public const float left = -1.0f;
        public const float right = 1.0f;
        public const float none = 0.0f;
    }
    public static bool jumpPressed;

    [HideInInspector] public float startSpeed;
    public float speed;
    public float MaxSpeed;
    public bool jumpable = true;
    public bool immobile = false;

    [Header("Hair")]
    public GameObject slicedHair;
    public GameObject hairMask;
    public AudioClip hairSlicedAudio;
    [HideInInspector] public bool _haircut;


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

    void Update()
    {
        setMovement();
        setAnimations();
    }

    void FixedUpdate()
    {
        float moveDistance = speed * Time.fixedDeltaTime;

        _characterController.Move(moveDistance, false, _jump);
        _jump = false;
    }

    public void OnLanding()  // TODO: Doesn't look like this is being used???
    {
        _anim.SetBool("Jump", false);
    }

    private void setMovement()
    {
        _horiMove = Mathf.Abs(speed);

        if (!immobile)
        {
            checkKeyboardMovement();

            if (TouchUtility.touched)
            {
                switch(TouchUtility.state)
                {
                    case Enums.TouchState.pressedLeft:
                        _horiMove = Direction.left;
                        break;
                    case Enums.TouchState.pressedRight:
                        _horiMove = Direction.right;
                        break;
                    case Enums.TouchState.tapped:
                        setJump(1f);
                        break;
                }
            }
        }
    }

    private void checkKeyboardMovement()
    {
        //_horiMove = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            setJump(1f);
        }
    }

    public void OnPointerPressed(bool triggered)
    {
        if(triggered && jumpable)
            setJump(1f); // was character controller
        jumpPressed = triggered;
    }

    public void setJump(float multiplier)
    {
        if (jumpable)
        {
            //_jump = true;
            _characterController.Jump(true, multiplier);
            _anim.SetBool("Jump", true);
        }
    }

    private void setAnimations()
    {
        _anim.SetFloat("HoriMove", Mathf.Abs(_horiMove));
        _anim.SetFloat("yVelocity", _rigidBody.velocity.y);
        _anim.SetBool("Grounded", _characterController.m_Grounded);
        _anim.SetFloat("Speed", speed);
    }
}
