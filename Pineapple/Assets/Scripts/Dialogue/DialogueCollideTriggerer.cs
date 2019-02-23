using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCollideTriggerer : MonoBehaviour
{
    public Collider2D triggerCollider;
    private DialogueSequence _ds;

    private void Awake()
    {
        _ds = this.GetComponent<DialogueSequence>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerCollider.transform == other.transform)
        {
            _ds.startDialogue();
        }
    }
}
