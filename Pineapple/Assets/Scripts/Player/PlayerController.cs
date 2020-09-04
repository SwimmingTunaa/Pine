﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cameraFollowTarget;
    public static bool jumpPressed;
    public bool pausePlayer;
    [Header("Running inputs")]
    [HideInInspector] public float startSpeed;
    public float speed;
    public float maxSpeed; 
    public bool jumpable = true;

    [Header("Flying Inputs")]
    public float flySpeed;
    private bool fly;

    [Header("Hair")]
    public GameObject slicedHair;
    public GameObject hairMask;
    public AudioClip hairSlicedAudio;

    [Header("Hover Board")]
    public GameObject hoverBoard;

    [HideInInspector] public Animator _anim;
    [HideInInspector] public float _horiMove;
    private CharacterController2D _characterController;
    private Rigidbody2D _rigidBody;

    void Awake()
    {
        _characterController = GetComponent<CharacterController2D>();
        
        _rigidBody = GetComponent<Rigidbody2D>();
        startSpeed = speed;
    }
    
    void Start()
    {
        _anim = CharacterManager.activeVisual.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if(!pausePlayer)
        {
            setAnimations();
            _characterController.Move(speed * Time.deltaTime);

            if(fly) _characterController.Fly(flySpeed);
            //_jump = false;
        }
    }
    public void OnPointerPressed(bool triggered)
    {
        //triggered by press jump button event
        if(triggered && !pausePlayer && !_characterController.isFlying)
        {
            setJump(1f); 
        }
        if(_characterController.isFlying)
        {
            fly = triggered;
        }
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

    public IEnumerator EquipHoverBoard()
    {
        yield return new WaitForSeconds(.2f);
        _characterController.isFlying = true;
        Time.timeScale = 0;
        _anim.SetBool("Fierce", true);
        GameManager.Instance.vfxVirtualCamera.gameObject.SetActive(true);
        hoverBoard.SetActive(true);
        yield return new WaitForSecondsRT(0.7f);
        _anim.SetBool("Fly", true);
        _anim.SetBool("Fierce", false);
        GameManager.Instance.vfxVirtualCamera.gameObject.SetActive(false);
        yield return new WaitForSecondsRT(.8f);
        Time.timeScale = 1;
        _characterController.AddForce(Vector2.up * 1000f);
    }

    public void UnequipHoverBoard()
    {
        _anim.SetBool("Fly", false);
        hoverBoard.SetActive(false);
    }
}
