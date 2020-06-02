using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObjectOnTrigger : MonoBehaviour
{
    public int triggerAmount = 1;
    public bool toggle;
    public GameObject[] objects;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(triggerAmount > 0)
        {
            triggerAmount--;
            foreach (GameObject go in objects)
            {
                go.SetActive(toggle);
            }
        }
       
    }
}
