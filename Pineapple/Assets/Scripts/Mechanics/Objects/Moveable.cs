using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Moveable : Interactable
{
    public Enums.Weight playerMoveSpeedPenalty = Enums.Weight.lite;
    private bool _parentThisObj = true;
    private Rigidbody2D _thisRb;
    private CharacterController2D _char2D;
    void Start()
    {
        _thisRb = GetComponent<Rigidbody2D>();
        _char2D = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
        initialize();
    }

    void initialize()
    {
        _thisRb.bodyType = RigidbodyType2D.Kinematic;
        _thisRb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }

    public override void DoAction(GameObject player)
    {
        base.DoAction(player);
        PlayerController pController = player.GetComponent<PlayerController>();
        HingeJoint2D hinge =  player.GetComponent<HingeJoint2D>();
        
        MoveObject(_parentThisObj, hinge, pController);
        _parentThisObj = !_parentThisObj;
    }

    public void MoveObject(bool enabled, HingeJoint2D hinge, PlayerController playerController)
    {
        hinge.enabled = enabled;
        playerController.jumpable = !enabled;
        playerController.GetComponent<CharacterController2D>().flippable = !enabled;
        if(enabled)
        {
            playerController.speed -= (float)playerMoveSpeedPenalty;
            if(_thisRb.bodyType != RigidbodyType2D.Dynamic)
                _thisRb.bodyType = RigidbodyType2D.Dynamic;
            _thisRb.constraints = RigidbodyConstraints2D.FreezePositionY;
            hinge.connectedBody = _thisRb;
        }
        else
        {
            hinge.connectedBody = null;
            _thisRb.constraints = RigidbodyConstraints2D.FreezePositionX;
            if(_thisRb.velocity.y >= 0f )
            {
                _thisRb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            }
            playerController.speed = playerController.startSpeed;
        }
    }
}
