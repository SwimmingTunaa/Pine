using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    public float speed;

    private Animator _anim;
    private float _horiMove;
    private bool _jump = false;
    private CharacterController2D _characterController;
    private Rigidbody2D _rigidBody;

    // Update is called once per frame

    void Awake()
    {
        _characterController = GetComponent<CharacterController2D>();
        _anim = GetComponentInChildren<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        _horiMove = Input.GetAxisRaw("Horizontal") * speed;
        _anim.SetFloat("HoriMove", Mathf.Abs(_horiMove));
        _anim.SetFloat("yVelocity", _rigidBody.velocity.y);
        if (Input.GetButtonDown("Jump"))
        {
            _jump = true;
            _anim.SetBool("Jump", true);
        }
        _anim.SetBool("Grounded", _characterController.m_Grounded);
    }

    void FixedUpdate()
    {
        _characterController.Move(_horiMove * Time.fixedDeltaTime, false, _jump);
        _jump = false;
    }

    public void OnLanding()
    {
        _anim.SetBool("Jump", false);
    }
}
