using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxToggle : MonoBehaviour
{
    public bool toggle;
    public int triggerAmount = 1;
    public string parallaxName;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && triggerAmount > 0 && parallaxName!= null)
        {
            triggerAmount--;
            ParallaxManager.instance.ToggleParallax(toggle, parallaxName);
        }
    }

    void OnEnable()
    {
        triggerAmount = 1;
    }
}
