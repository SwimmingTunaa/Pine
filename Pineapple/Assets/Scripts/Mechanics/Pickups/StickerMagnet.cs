using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerMagnet : PickUpsBase
{   
    [Header("Sticker")]
    public GameObject activeEffect;
    public GameObject visual;
    
    void OnEnable()
    {
        visual.SetActive(true);
        activeEffect.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(triggerAmount > 0 && other.CompareTag("Player"))
            DoAction(other.gameObject);
    }

    public override void DoAction(GameObject player)
    {
        activeEffect.SetActive(true);
        visual.SetActive(false);
        base.DoAction(player);
        transform.parent = player.GetComponentInChildren<Outfits>().pickUpSpawnPoint;
        transform.position = player.GetComponentInChildren<Outfits>().pickUpSpawnPoint.position;
    }

}
