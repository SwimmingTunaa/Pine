using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObjectOnTrigger : MonoBehaviour
{
    public int triggerAmount = 1;
    public LayerMask whoCanTriggerThis;
    public bool toggle;
    public GameObject[] objects;

    void OnEnable() => triggerAmount = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(triggerAmount > 0 && GetComponent<Collider2D>().IsTouchingLayers(whoCanTriggerThis))
        {
            triggerAmount--;
            foreach (GameObject go in objects)
            {
                go.SetActive(toggle);
            }
        }
       
    }
}
