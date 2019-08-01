using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerMagnet : PickUpsBase
{   
    public float effectRadius;
    public float transitionSpeed;
    private bool active;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && triggerAmount > 0)
        {
            DoAction(other.gameObject);
        }
        if(other.GetComponent<Sticker>() != null && active)
        {
            other.GetComponent<Sticker>().moveToTarget  = other.gameObject;
            other.GetComponent<Sticker>().move = true;
        }
    }

    public override void DoAction(GameObject player)
    {
        base.DoAction(player);
        transform.parent = player.transform;
        transform.position = player.transform.position;
        GetComponent<CircleCollider2D>().radius = effectRadius;
        active = true;
    }
}
