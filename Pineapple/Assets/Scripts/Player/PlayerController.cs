using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private class Direction {
        public const float left = -1.0f;
        public const float right = 1.0f;
        public const float none = 0.0f;
    }


    public float startSpeed;
    public float speed;
    public bool jumpable = true;
    public bool immobile = false;

    private Animator _anim;
    private float _horiMove;
    private bool _jump = false;
    private CharacterController2D _characterController;
    private Rigidbody2D _rigidBody;

    void Awake()
    {
        _characterController = GetComponent<CharacterController2D>();
        _anim = GetComponentInChildren<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
        speed = startSpeed;
    }

    void Update()
    {
        setMovement();
        setAnimations();
    }

    void FixedUpdate()
    {
        float moveDistance = _horiMove * speed * Time.fixedDeltaTime;

        _characterController.Move(moveDistance, false, _jump);
        _jump = false;
    }

    public void OnLanding()  // TODO: Doesn't look like this is being used???
    {
        _anim.SetBool("Jump", false);
    }

    private void setMovement()
    {
        _horiMove = Direction.none;

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
                        setJump();
                        break;
                }
            }
        }
    }

    private void checkKeyboardMovement()
    {
        _horiMove = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            setJump();
        }
    }

    private void setJump()
    {
        if (jumpable)
        {
            _jump = true;
            _anim.SetBool("Jump", true);
        }
    }

    private void setAnimations()
    {
        _anim.SetFloat("HoriMove", Mathf.Abs(_horiMove));
        _anim.SetFloat("yVelocity", _rigidBody.velocity.y);
        _anim.SetBool("Grounded", _characterController.m_Grounded);
    }
}
