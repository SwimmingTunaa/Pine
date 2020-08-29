using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUp : PickUpsBase
{
    [Header("Shield")]
    public float shieldStrength;
    public AudioClip deathSound;
    public GameObject shieldHealthBar;
    public GameObject hitEffect;

    private PlayerHealth health;
    private List<GameObject> healthBars = new List<GameObject>();

    void Awake()
    {
        foreach(Transform t in shieldHealthBar.transform)
        {
            healthBars.Add(t.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(triggerAmount > 0 && other.CompareTag("Player"))
        {
            health = other.gameObject.GetComponent<PlayerHealth>();
            DoAction(health.gameObject);
        }
    }

    public override void DoAction(GameObject player)
    {
        ShieldPickUpObject s = item.Instance as ShieldPickUpObject;
        health.AddShield(s.shieldStrength, healthBars);
        base.DoAction(player);
        health.hitEffect = hitEffect;
        Outfits outfit = player.GetComponentInChildren<Outfits>();
        transform.parent = outfit.pickUpSpawnPoint;
        transform.position = outfit.pickUpSpawnPoint.position;
        //set the health bar to the shield strength
        for (int i = 0; i < s.shieldStrength; i++)
        {
            healthBars[i].SetActive(true);
        }
        GetComponentInChildren<Animator>().Play("Shield Start");
    }

    public override void Update()
    {
        if(health != null && health.health <= 1 && health.shieldActive)
        {
            health.shieldActive = false;
            if(deathSound != null)
            GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(deathSound);
            transform.parent = null;
            GetComponent<ObjectID>().Disable();
        }
    }
}
