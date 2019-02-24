using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSequence : MonoBehaviour
{
    public SpeechBubble speechBubble;
    public int dialogueInterval = 4;
    public DialogueConfig[] dialogues;

    private PlayerController _playerController;
    private float _timer = 0;
    private int _currentDialogue = 0;
    private bool _running = false;
    

    private void Awake()
    {
        _playerController = GameObject
            .FindGameObjectWithTag("Player")
            .GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (_running)
        {
            _timer += Time.deltaTime;

            bool lastDialogueFlag = _currentDialogue == dialogues.Length-1;
            bool touched = userTapped();

            // TODO: if (interactionButton.pressed) endDialogue()

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
            else if (_timer > dialogueInterval || touched)
            {
                _timer = 0;
                iterateDialogue();
            }
        }
    }

    public void startDialogue()
    {
        _running = true;
        _playerController.immobile = true;
        speechBubble.set(dialogues[_currentDialogue]);
        speechBubble.gameObject.SetActive(true);
    }

    private void endDialogue()
    {
        _playerController.immobile = false;
        speechBubble.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void iterateDialogue()
    {
        _currentDialogue++;
        speechBubble.set(dialogues[_currentDialogue]);
    }

    private bool userTapped()
    {
        return TouchUtility.state == Enums.TouchState.tapped ||
            Input.GetKeyUp(KeyCode.Return) ||
            Input.GetKeyUp(KeyCode.Space);
    }
}
