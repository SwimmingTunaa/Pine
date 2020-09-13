using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainTrigger : MonoBehaviour
{
    public Vector2 forceDirection;
    private CharacterController2D character;
    private PlayerController playerController;
    private float startFlySpeed;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!character && other.CompareTag("Player"))
        {
            playerController = other.GetComponent<PlayerController>();
            character = other.GetComponent<CharacterController2D>();
            startFlySpeed = playerController.flySpeed;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            print("get rained on");
            character.AddForce(forceDirection);
            playerController.flySpeed = 0;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
            playerController.flySpeed = startFlySpeed;
    }
}
