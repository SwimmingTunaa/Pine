using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DialogueSequence : MonoBehaviour
{
    public bool stopPlayerMoving;
    public bool randomDia;
    public DialogueConfig[] dialogues;
    public SpeechBubble speechBubble;

    private float _timer = 0;
    private int _currentDialogue = 0;
    private bool _scriptRunning = false;
    private bool _lastDialogueFlag;

    void Start()
    {
        dialogues[_currentDialogue].character = CharacterManager.activeCharacter;
    }

    private void Update()
    {
        if (_scriptRunning)
        {
            _timer += Time.deltaTime;

            _lastDialogueFlag = randomDia ? true : _currentDialogue == dialogues.Length-1;

            // Are we showing the last dialogue
            if (_lastDialogueFlag)
            {
                // Has the user indicated they've finished reading
                if ((!stopPlayerMoving && TimeCheck()))
                {
                    EndDialogue();
                }
            }
            // Should we show next dailogue
            else if ((TimeCheck())) // TODO: If stopPlayerMoveing is true, I think we should skip the touch here and let it play out over time.
            {
                _timer = 0;
                _currentDialogue++;
                SetSpeechbubble(dialogues[_currentDialogue]);
            }
        }
    }

    private bool TimeCheck()
    {
        return _timer >= dialogues[_currentDialogue].diallogueInterval;
    }

    public void StartDialogue(GameObject character)
    {
        _scriptRunning = true;
        dialogues[_currentDialogue].character = character;
        SetSpeechbubble(dialogues[_currentDialogue]);
    }

    public void StartRandomDialogueSequence(GameObject character)
    {
        _scriptRunning = true;
        int randomInt = Random.Range(0, dialogues.Length);
        _currentDialogue = randomInt;
        dialogues[_currentDialogue].character = character;
        SetSpeechbubble(dialogues[_currentDialogue]);
    }

    public void StartRandomDialogueSequence()
    {
        _scriptRunning = true;
        int randomInt = Random.Range(0, dialogues.Length);
        _currentDialogue = randomInt;
        dialogues[_currentDialogue].character = CharacterManager.activeCharacter;
        SetSpeechbubble(dialogues[_currentDialogue]);
    }

    public void EndDialogue()
    {
        speechBubble.gameObject.SetActive(false);
        _scriptRunning = false;
        //gameObject.SetActive(false);
    }

    public void SetSpeechbubble(DialogueConfig dialogue)
    {
        dialogues[_currentDialogue].character = CharacterManager.activeCharacter;
        //turn off and on object so animation replays
        speechBubble.gameObject.SetActive(false);
        speechBubble.gameObject.SetActive(true);
        speechBubble.set(dialogue);
    }

    private bool userTapped()
    {
        return TouchUtility.state == Enums.TouchState.tapped ||
            Input.GetKeyUp(KeyCode.Return); //removed the space inut cause it was interfering with the dialog
    }
}
