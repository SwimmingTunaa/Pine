using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public bool startPaused;
    public GameObject gameUI;
    public DialogueSequence dialogue;
    public Button PlayButton;

    void Start()
    {
        MainMenuDefualt(startPaused);
        //totalStickers.text = PlayerPrefs.GetInt("TotalStickers").ToString();
    }

    public void MainMenuEnable(float waitTime)
    {
        StartCoroutine(MainMenuWait(true, waitTime));
    }

    public void MainMenuDisable(float waitTime)
    {
        PlayButton.interactable = false;
        StartCoroutine(MainMenuWait(false, waitTime));
    }

    public IEnumerator MainMenuWait(bool enable, float waitTime)
    {
        Debug.Log(waitTime);
        yield return new WaitForSeconds(waitTime);
        MainMenuDefualt(enable);
    }

     public void MainMenuDefualt(bool enable)
    {
        Statics.paused = enable;
        gameUI.SetActive(!enable);
        GameManager.pauseGame(enable);
        Debug.Log("paused = " + Statics.paused);
        this.gameObject.SetActive(enable);
        /*/if(enable)
            dialogue.startDialogue();
        else
            dialogue.endDialogue();*/
    }
}
 