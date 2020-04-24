using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sticker : Item
{
    [Header("Sticker")]
    public int value;
    public GameObject deathEffect;
    private StatsManager sm;
    [HideInInspector] public GameObject moveToTarget;
    [HideInInspector] public bool move;
    private GameManager effectParentObj;


    void Start()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Random.Range(-90f,90f));
        sm = GameObject.FindGameObjectWithTag("GameController").GetComponent<StatsManager>();
        triggerAmount = 1;
    }
    void OnEnable()
    {
        deathEffect.transform.SetParent(this.gameObject.transform);
        deathEffect.transform.position = this.gameObject.transform.position;
        gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(triggerAmount > 0 && other.CompareTag("Player"))
            DoAction(other.gameObject);
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
        if(sm != null)
        {
            AudioSource a = sm.gameObject.GetComponent<AudioSource>();
            a.Stop();
            a.PlayOneShot(itemObject.pickUpSound);
        }
        deathEffect.SetActive(true);
        deathEffect.transform.parent = null;
        sm.stickerCollected += value;
        move = false;
        base.DoAction(player);
    }

    public void moveStickerToTarget(GameObject target, float speed)
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * (speed + target.GetComponent<Rigidbody2D>().velocity.x));
    }
}
