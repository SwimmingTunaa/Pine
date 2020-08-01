using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBoardHeath : EnemyHealth
{
    [Header("Floor board details")]
    public Sprite crackedWood;
    public static float HEALTH;
    public static bool floorIsOpened;
    
    void Start()
    {
        FloorBoardHeath.HEALTH = health;
        health = HEALTH;
    }

    public override void TakeDamage(float damage)
    {
        if(HEALTH > 0 && !dead)
        {
            HEALTH -= damage;
            health = HEALTH;
            UpdateState();
        }
        if(HEALTH <= 0 && !dead)
            Dead();
    }

    void UpdateState()
    {
        if(HEALTH == 0)
                floorIsOpened = true; 
            else    
                if(HEALTH == 1)
                {
                    foreach (Transform child in transform)
                    {
                        child.GetComponent<SpriteRenderer>().sprite = crackedWood;
                    }
                }
    }
}
