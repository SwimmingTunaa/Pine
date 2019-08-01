using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float bounceStrengthY = 1f;
    public float bounceStrengthX = 50f;
    public AudioClip bounceSFX;

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(bounceStrengthX,bounceStrengthY), ForceMode2D.Impulse);
            other.gameObject.GetComponent<AudioSource>().PlayOneShot(bounceSFX);
            other.gameObject.GetComponent<PlayerController>()._anim.SetTrigger("Surprise");
        }
    }
}
