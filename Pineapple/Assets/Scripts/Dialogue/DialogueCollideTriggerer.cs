using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCollideTriggerer : DialogueSequence
{
    public int triggerAmount = 1;
    public bool randomDialogue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && triggerAmount > 0)
        {
            triggerAmount--;
            if(!speechBubble)
                speechBubble = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().speachBubble;
            if (randomDialogue)  StartRandomDialogueSequence(other.gameObject); else StartDialogue(other.gameObject);
        }
    }

}
