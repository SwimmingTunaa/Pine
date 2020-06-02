using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public bool startPaused;
    public GameManager gameManager;
    public GameObject gameUI;
    public DialogueSequence dialogue;
    public Button PlayButton;
    public GameObject followCamera, debuffCamera;
    public GameObject[] objectsToToggleOn;
    public GameObject[] objectsToToggleOff;

    void Start()
    {
        if(PlayerPrefs.GetInt("Retry") == 0)
            MainMenuDefualt(startPaused);
        else
            if(PlayerPrefs.GetInt("Retry") == 1)
            {
                MainMenuDisable(1);
                PlayerPrefs.SetInt("Retry", 0);
            }
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
        gameManager.enabled = !enable;
        foreach (GameObject g in objectsToToggleOn)
        {
            g.SetActive(!enable);
        }
        foreach (GameObject g in objectsToToggleOff)
        {
            g.SetActive(enable);
        }
        debuffCamera.SetActive(!enable);
        yield return new WaitForSeconds(waitTime);
        MainMenuDefualt(enable);
        followCamera.SetActive(!enable);
        debuffCamera.SetActive(enable);
    }

     public void MainMenuDefualt(bool enable)
    {
        Statics.paused = enable;
        gameUI.SetActive(!enable);
        GameManager.PauseGame(enable);
        this.gameObject.SetActive(enable);       
    }
}
 