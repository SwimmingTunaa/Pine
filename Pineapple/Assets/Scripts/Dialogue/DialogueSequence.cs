using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DialogueSequence : MonoBehaviour
{
    public bool stopPlayerMoving;
    public DialogueConfig[] dialogues;
    public GameObject speechbubble;

    private PlayerController _playerController;
    private SpeechBubbleSize _speechBubbleSize;
    private float _timer = 0;
    private int _currentDialogue = 0;
    private bool _running = false; //not the player running but the script
    private void Awake()
    {
        _playerController = GameObject
            .FindGameObjectWithTag("Player")
            .GetComponent<PlayerController>();
        _speechBubbleSize = speechbubble.GetComponent<SpeechBubbleSize>();
    }

    private void Update()
    {
        if (_running)
        {
            _timer += Time.deltaTime;

            bool lastDialogueFlag = _currentDialogue == dialogues.Length-1;
            bool touched = userTapped();

            // TODO: if (interactionButton.pressed) endDialogue()
            // TODO: If stopPlayingMoving == false, make dialogue end automatically after the last dialogue interval? 

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
                iterateDialogue();
    
            }
        }
        //dialogue follow player hack
        if(speechbubble.activeInHierarchy)
        {
            speechbubble.transform.position = dialogues[_currentDialogue].character.transform.position + Vector3.up * 3f;
        }
    }

    public void startDialogue()
    {
        _running = true;
        if(stopPlayerMoving)
             _playerController.immobile = true;
        setSpeechbubble(dialogues[_currentDialogue]);
    }

    private void endDialogue()
    {
        if(_playerController.immobile)
            _playerController.immobile = false;
        speechbubble.SetActive(false);
        gameObject.SetActive(false);
    }

    private void iterateDialogue()
    {
        _currentDialogue++; 
        setSpeechbubble(dialogues[_currentDialogue]);
    }

    private void setSpeechbubble(DialogueConfig dialogue)
    {
        //turn off and on object so animation replays
        speechbubble.SetActive(false);
        speechbubble.SetActive(true);
        speechbubble.GetComponentInChildren<SpriteRenderer>().sprite = _speechBubbleSize.GetSpeechBubbleSize(dialogue);
        speechbubble.GetComponentInChildren<TextMeshPro>().text = dialogue.text;
    }

    private bool userTapped()
    {
        return TouchUtility.state == Enums.TouchState.tapped ||
            Input.GetKeyUp(KeyCode.Return) ||
            Input.GetKeyUp(KeyCode.Space);
    }
}
