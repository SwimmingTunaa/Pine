using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public bool startPaused;
    public GameObject gameUI;
    public DialogueSequence dialogue;
    public TextMeshProUGUI furthestTravelled;
    public TextMeshProUGUI mostStickersCollected;
    public TextMeshProUGUI totalStickers;
    public Animator playerAnim;

    void Start()
    {
        MainMenuDefualt(startPaused);
        furthestTravelled.text = PlayerPrefs.GetInt("LongestDistanceTravelled").ToString() + "m";
        mostStickersCollected.text = PlayerPrefs.GetInt("StickersCollected").ToString();
        //totalStickers.text = PlayerPrefs.GetInt("TotalStickers").ToString();
    }

    public void MainMenuEnable(float waitTime)
    {
        StartCoroutine(MainMenuWait(true, waitTime));
    }

    public void MainMenuDisable(float waitTime)
    {
        StartCoroutine(MainMenuWait(false, waitTime));
    }

    public IEnumerator MainMenuWait(bool enable, float waitTime)
    {
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
 