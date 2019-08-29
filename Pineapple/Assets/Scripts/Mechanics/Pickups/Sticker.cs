using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sticker : Item
{
    public int value;
    private GameManager gm;
    [HideInInspector] public GameObject moveToTarget;
    [HideInInspector] public bool move;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        triggerAmount = 1;
    }
    void OnEnable()
    {
        gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if((other.CompareTag("Player") || other.CompareTag("Hair")) && triggerAmount > 0)
        {
            DoAction(other.gameObject);
        }
    }

    void Update()
    {
        if(move)
        {
            moveStickerToTarget(GameManager._player.gameObject, 7f);
        }
           
    }

    public override void DoAction(GameObject player)
    {
        if(gm != null && !gm.gameObject.GetComponent<AudioSource>().isPlaying)
            gm.gameObject.GetComponent<AudioSource>().PlayOneShot(itemObject.pickUpSound);
        gm.stickerCollected += value;
        move = false;
        base.DoAction(player);
    }

    public void moveStickerToTarget(GameObject target, float speed)
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * speed);
    }
}
