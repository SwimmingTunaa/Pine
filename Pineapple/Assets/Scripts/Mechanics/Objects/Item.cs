using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public ItemObject itemObject;
    public Collider2D otherCollider;
    public int triggerAmount;

    private Inventory _inventory;

    void Awake()
    {
        _inventory = GameObject.FindGameObjectWithTag("GameController") .GetComponent<Inventory>();
        itemObject.Initialize(this.gameObject);
    }

    //At the moment the item gets pick up when the player walks over it
    //Not sure if we want to do pick up with the interact button
    void OnTriggerEnter2D(Collider2D other)
    {
        if(otherCollider.transform == other.transform && triggerAmount > 0)
        {
            DoAction(other.gameObject);
        }
    }
    public override void DoAction(GameObject player)
    {
        triggerAmount--;
        base.DoAction(player);
        _inventory.inventoryItems.Add(itemObject);

        if(itemObject.pickUpSound)
            GetComponent<AudioSource>().PlayOneShot(itemObject.pickUpSound);
        if(itemObject.deathFX)
            Instantiate(itemObject.deathFX,transform.position,transform.rotation);
        
        gameObject.SetActive(false);
    }
}
