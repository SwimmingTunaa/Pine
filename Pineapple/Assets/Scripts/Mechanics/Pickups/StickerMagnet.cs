using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerMagnet : PickUpsBase
{   
    public float effectRadius;
    public float transitionSpeed;
    public GameObject activeEffect;
    public GameObject visual;
    private bool active;

    void Start()
    {
        visual.SetActive(true);
        activeEffect.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && triggerAmount > 0)
        {
            DoAction(other.gameObject);
        }
        if(other.GetComponent<Sticker>() != null && active)
        {
            other.GetComponent<Sticker>().move = true;
        }
    }

    public override void DoAction(GameObject player)
    {
        activeEffect.SetActive(true);
        visual.SetActive(false);
        base.DoAction(player);
        transform.parent = player.transform;
        transform.position = player.transform.position;
        GetComponent<CircleCollider2D>().radius = effectRadius;
        active = true;
    }
}
