using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxToggle : MonoBehaviour
{
    private int triggerAmount = 1;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && triggerAmount > 0)
        {
            triggerAmount--;
            ParallaxManager.instance.ChangeParallax();
        }
    }

    void OnEnable()
    {
        triggerAmount = 1;
    }
}
