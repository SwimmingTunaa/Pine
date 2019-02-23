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

    /*private void setHorizontalMovement()
    {
        _horiMove = Direction.none;

        if (!immobile || !_characterController.m_Grounded)
        {
            checkKeyboardMovement();   

            if (Input.touchCount > 0)
            {
                _touchTime += Time.deltaTime;

                Touch touch = Input.GetTouch(0);

                if (_touchTime > Constants.MAX_TAP_TIME)
                {
                    //move right if right side of screen, otherwise assume left
                    _horiMove = (touch.position.x > Screen.width / 2) ? Direction.right : Direction.left;
                }

                // TODO: Quite sure this isn't needed. If touch ended last update, HoriMove will be 0 and this gets called after in update()
                if (touch.phase == TouchPhase.Ended)
                {
                    if (_touchTime <= Constants.MAX_TAP_TIME)
                    {
                        setJump();
                    }
                    _touchTime = 0;
                    //_anim.SetFloat("HoriMove", 0);
                }
            }
        }
    }*/

    private void setMovement()
    {
        _horiMove = Direction.none;

        if (!immobile)
        {
            checkKeyboardMovement();

            if (TouchUtility.touched)
            {
                if (TouchUtility.pressedRight)
                {
                    _horiMove = Direction.right;
                }
                else if (TouchUtility.pressedLeft)
                {
                    _horiMove = Direction.left;
                }
                else if (TouchUtility.tapped)
                {
                    setJump();
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
