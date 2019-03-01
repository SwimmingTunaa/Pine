using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DialogueSequence : MonoBehaviour
{
    public bool stopPlayerMoving;
    public DialogueConfig[] dialogues;
    public SpeechBubble speechBubble;

    private PlayerController _playerController;
    private float _timer = 0;
    private int _currentDialogue = 0;
    private bool _scriptRunning = false;

    private void Awake()
    {
        _playerController = GameObject
            .FindGameObjectWithTag("Player")
            .GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (_scriptRunning)
        {
            _timer += Time.deltaTime;

            bool lastDialogueFlag = _currentDialogue == dialogues.Length-1;
            bool touched = userTapped();

            // TODO: if (interactionButton.pressed) endDialogue()

            // TODO: If stopPlayingMoving == false, make dialogue end automatically after the last dialogue interval? 
            //      - Let's check with Gaurav how he wants this to work. I thought we should force the player to 
            //        end the dialogue incase when the dialgue ends they need to immediately jump out the way of something or whatever.


            // Are we showing the last dialogue
            if (lastDialogueFlag)
            {
                // Has the user indicated they've finished reading
                if (touched)
                {
                    Invoke("endDialogue", Constants.MAX_TAP_TIME);
                }
            }
            // Should we show next dailogue
            else if ((_timer >= dialogues[_currentDialogue].diallogueInterval || touched))
            {
                _timer = 0;
                _currentDialogue++;
                setSpeechbubble(dialogues[_currentDialogue]);
            }
        }
    }

    public void startDialogue()
    {
        _scriptRunning = true;
        if(stopPlayerMoving)
        {
            _playerController.immobile = true;
        }

        setSpeechbubble(dialogues[_currentDialogue]);
    }

    private void endDialogue()
    {
        _scriptRunning = false;

        // Check if it was this script that stopped the player moving.
        if (stopPlayerMoving)
        {
            // If so let them move again.
            _playerController.immobile = false;
        }

        speechBubble.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void setSpeechbubble(DialogueConfig dialogue)
    {
        //turn off and on object so animation replays
        speechBubble.gameObject.SetActive(false);
        speechBubble.gameObject.SetActive(true);
        speechBubble.set(dialogue);
    }

    private bool userTapped()
    {
        return TouchUtility.state == Enums.TouchState.tapped ||
            Input.GetKeyUp(KeyCode.Return) ||
            Input.GetKeyUp(KeyCode.Space);
    }
}
