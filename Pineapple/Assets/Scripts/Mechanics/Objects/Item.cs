using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public ItemObject itemObject;
    public Collider2D otherCollider;
    public int triggerAmount;
    public bool addThisItemToInventory;
    private Inventory _inventory;

    void Awake()
    {
        if(addThisItemToInventory)
            _inventory = GameObject.FindGameObjectWithTag("GameController").GetComponent<Inventory>();
        itemObject.Initialize(this.gameObject);
    }

    //At the moment the item gets pick up when the player walks over it
    //Not sure if we want to do pick up with the interact button. Maybe pick up with interact button will be like holding an item
    //eg. holding a gun or something alike
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && triggerAmount > 0)
        {
            DoAction(other.gameObject);
        }
    }
    public override void DoAction(GameObject player)
    {
        triggerAmount--;
        base.DoAction(player);

        //add to inventory
        if(addThisItemToInventory)
            _inventory.AddItem(itemObject);

        //Only plays these if they are not null
        if(itemObject.pickUpSound)
            GetComponent<AudioSource>().PlayOneShot(itemObject.pickUpSound);
        if(itemObject.deathFX)
            Instantiate(itemObject.deathFX,transform.position,transform.rotation);
        
        gameObject.SetActive(false);
    }
}
