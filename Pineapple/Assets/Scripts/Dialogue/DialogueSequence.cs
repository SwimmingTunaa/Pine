using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSequence : MonoBehaviour
{
    public DialogueConfig[] dialogues;
    public GameObject speechbubble;
    public int dialogueInterval = 4;

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
            bool touched = shouldShowNextDialogue();

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
        setSpeechbubble(dialogues[_currentDialogue]);
        // TODO: Switch on speechbubble
    }

    private void endDialogue()
    {
        _playerController.immobile = false;
        // TODO: Switch off speechbubble

        Destroy(gameObject);
    }

    private void iterateDialogue()
    {
        _currentDialogue++;
        setSpeechbubble(dialogues[_currentDialogue]);
    }

    private void setSpeechbubble(DialogueConfig dialogue)
    {
        print(dialogue.text);
        //  speechbubble.transform.position = d.character.transform.position + offset;
        //  speechbubble.GetComponentInChildren<TextMesh>().text = d.text;
        //  speechbubble.GetComponent<SpriteRenderer>.sprite = d.bubbleSize;
    }

    private bool shouldShowNextDialogue()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            return touch.phase == TouchPhase.Ended;
        }

        return Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.Space);
    }
}
