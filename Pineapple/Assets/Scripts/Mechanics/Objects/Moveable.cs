using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : Interactable
{
    public float playerMoveSpeedPenalty;
    private bool _parentThisObj = true;
    private PlayerController _playerController;
    private Rigidbody2D _thisRb;
    void Start()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _thisRb = this.gameObject.GetComponent<Rigidbody2D>();
    }
    public override void DoAction(GameObject player)
    {
        base.DoAction(player);
        HingeJoint2D hinge =  player.GetComponent<HingeJoint2D>();
         
        _playerController.jumpable = !_parentThisObj;
        player.GetComponent<CharacterController2D>().flippable = !_parentThisObj;
        if(_parentThisObj)
        {
            hinge.enabled = true;
          _playerController.speed += playerMoveSpeedPenalty;
            if(_thisRb.bodyType != RigidbodyType2D.Dynamic)
                _thisRb.bodyType = RigidbodyType2D.Dynamic;
            _thisRb.constraints = RigidbodyConstraints2D.FreezePositionY;
            hinge.connectedBody = _thisRb;
        }
        else
        {
            hinge.enabled = false;
            _thisRb.constraints = RigidbodyConstraints2D.FreezePositionX;
           _playerController.speed =  _playerController.startSpeed;
            hinge.connectedBody = null;
        }
        _parentThisObj = !_parentThisObj;
        
    }
}
