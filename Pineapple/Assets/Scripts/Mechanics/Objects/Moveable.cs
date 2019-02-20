using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : Interactable
{
    public Enums.Weight playerMoveSpeedPenalty;
    private bool _parentThisObj = true;
    public override void DoAction(GameObject player)
    {
        base.DoAction(player);
        HingeJoint2D hinge =  player.GetComponent<HingeJoint2D>();
        Rigidbody2D thisRb = this.gameObject.GetComponent<Rigidbody2D>();
        player.GetComponent<PlayerController>().jumpable = !_parentThisObj;
        player.GetComponent<CharacterController2D>().flippable = !_parentThisObj;
        if(_parentThisObj)
        {
            hinge.enabled = true;
            player.GetComponent<PlayerController>().speed -= (float)playerMoveSpeedPenalty;
            if(thisRb.bodyType != RigidbodyType2D.Dynamic)
                thisRb.bodyType = RigidbodyType2D.Dynamic;
            thisRb.constraints = RigidbodyConstraints2D.FreezePositionY;
            hinge.connectedBody = thisRb;
        }
        else
        {
            hinge.enabled = false;
            thisRb.constraints = RigidbodyConstraints2D.FreezePositionX;
            player.GetComponent<PlayerController>().speed =  player.GetComponent<PlayerController>().startSpeed;
            hinge.connectedBody = null;
        }
        _parentThisObj = !_parentThisObj;
        
    }
}
