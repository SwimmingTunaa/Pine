using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxToggle : MonoBehaviour
{
    private int triggerAmount = 1;
    public string parallaxName;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && triggerAmount > 0)
        {
            triggerAmount--;
            if(parallaxName != "")
                ParallaxManager.instance.ToggleParallax(true, parallaxName);
                else
                    ParallaxManager.instance.ChangeParallax();
        }
    }

    void OnEnable()
    {
        triggerAmount = 1;
    }

      void OnDisable()
    {
        triggerAmount = 1;
    }
}
