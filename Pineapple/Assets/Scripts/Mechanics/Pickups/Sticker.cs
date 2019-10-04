using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sticker : Item
{
    public int value;
    public GameObject deathEffect;
    private StatsManager sm;
    [HideInInspector] public GameObject moveToTarget;
    [HideInInspector] public bool move;

    void Start()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Random.Range(-45f,45f));
        sm = GameObject.FindGameObjectWithTag("GameController").GetComponent<StatsManager>();
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
            moveStickerToTarget(GameManager._player.gameObject, 9f);
        }
    }

    public override void DoAction(GameObject player)
    {
        if(sm != null && !sm.gameObject.GetComponent<AudioSource>().isPlaying)
            sm.gameObject.GetComponent<AudioSource>().PlayOneShot(itemObject.pickUpSound);
        sm.stickerCollected += value;
        move = false;
        base.DoAction(player);
    }

    public void moveStickerToTarget(GameObject target, float speed)
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * speed);
    }
}
