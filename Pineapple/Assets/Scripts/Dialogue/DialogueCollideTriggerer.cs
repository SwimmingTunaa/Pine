using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCollideTriggerer : DialogueSequence
{
    public Collider2D triggerCollider;
    public int triggerAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerCollider.transform == other.transform && triggerAmount > 0)
        {
            triggerAmount--;
            startDialogue();
        }
    }
}
