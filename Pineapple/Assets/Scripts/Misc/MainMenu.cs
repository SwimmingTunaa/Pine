using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public bool startPaused;
    public GameManager gm;
    public GameObject menuUI;
    public GameObject gameUI;
    public DialogueSequence dialogue;
    public TextMeshProUGUI furthestTravelled;
    public TextMeshProUGUI mostStickersCollected;
    public TextMeshProUGUI totalStickers;

    void Start()
    {
        MainMenuDefualt(startPaused);
        furthestTravelled.text = PlayerPrefs.GetInt("LongestDistanceTravelled").ToString() + "m";
        mostStickersCollected.text = PlayerPrefs.GetInt("StickersCollected").ToString();
        totalStickers.text = PlayerPrefs.GetInt("TotalStickers").ToString();
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
        Statics.paused = enable;
        menuUI.SetActive(enable);
        gameUI.SetActive(!enable);
        gm.pauseGame(enable);
        Debug.Log("paused = " + Statics.paused);
        GameManager._player._anim.SetBool("Sit", enable);
        GameManager._player._anim.SetTrigger("Scared");
        
        /* if(enable)
            dialogue.startDialogue();
        else
            dialogue.endDialogue();*/
    }

     public void MainMenuDefualt(bool enable)
    {
        Statics.paused = enable;
        menuUI.SetActive(enable);
        gameUI.SetActive(!enable);
        gm.pauseGame(enable);
        Debug.Log("paused = " + Statics.paused);
        GameManager._player._anim.SetBool("Sit", enable);
        
        /*/if(enable)
            dialogue.startDialogue();
        else
            dialogue.endDialogue();*/
    }

    public void DelayTransitionEnable(GameObject obj)
    {
        StartCoroutine(Delay(obj, true));
    }
    public void DelayTransitionDisable(GameObject obj)
    {
        StartCoroutine(Delay(obj, false));
    }
   
   IEnumerator Delay(GameObject obj, bool active)
   {
       yield return new WaitForSeconds(0.5f);
       obj.SetActive(active);
   }
   public void ResetStats()
   {
       PlayerPrefs.DeleteKey("HighScore");
       PlayerPrefs.DeleteKey("StickersCollected");
       PlayerPrefs.DeleteKey("FirstTimeStart");
       furthestTravelled.text = "0m";
       mostStickersCollected.text = "0";
   }
}
 