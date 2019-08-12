using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DialogueSequence : MonoBehaviour
{
    public bool stopPlayerMoving;
    public DialogueConfig[] dialogues;
    public SpeechBubble speechBubble;

    private InteractButton _interactBtn;
    private PlayerController _playerController;
    private float _timer = 0;
    private int _currentDialogue = 0;
    private bool _scriptRunning = false;

    private void Awake()
    {
        _playerController = GameObject
            .FindGameObjectWithTag("Player")
            .GetComponent<PlayerController>();

        _interactBtn = GameObject
            .FindGameObjectWithTag("InteractBtn")
            .GetComponent<InteractButton>();
    }

    private void Update()
    {
        if (_scriptRunning)
        {
            _timer += Time.deltaTime;

            bool lastDialogueFlag = _currentDialogue == dialogues.Length-1;
            bool touched = userTapped();

            // Are we showing the last dialogue
            if (lastDialogueFlag)
            {
                // Has the user indicated they've finished reading
                if (touched || (!stopPlayerMoving && timeCheck()))
                {
                    Invoke("endDialogue", Constants.MAX_TAP_TIME);
                }
            }
            // Should we show next dailogue
            else if ((timeCheck() || touched)) // TODO: If stopPlayerMoveing is true, I think we should skip the touch here and let it play out over time.
            {
                _timer = 0;
                _currentDialogue++;
                setSpeechbubble(dialogues[_currentDialogue]);
            }
        }
    }

    private bool timeCheck()
    {
        return _timer >= dialogues[_currentDialogue].diallogueInterval;
    }

    public void startDialogue()
    {
        _scriptRunning = true;
        if(stopPlayerMoving)
        {
            _playerController.immobile = true;
            _interactBtn.set(gameObject, "Skip", null, Enums.InteractColor.deactivate, null);
            _interactBtn.setDoAction(gameObject, endDialogue);
        }

        setSpeechbubble(dialogues[_currentDialogue]);
    }

    public void endDialogue()
    {
        _scriptRunning = false;
        
        // Check if it was this script that stopped the player moving.
        if (stopPlayerMoving)
        {
            // If so let them move again.
            _playerController.immobile = false;
            _interactBtn.resetBtn(gameObject);
        }

        speechBubble.gameObject.SetActive(false);
        //gameObject.SetActive(false);
    }

    public void setSpeechbubble(DialogueConfig dialogue)
    {
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
