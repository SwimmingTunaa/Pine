using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public void pauseGame()
    {
        Time.timeScale = 0; // TODO: Not sure if this is correct. Something about timescale pausing all scripts and everything?
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
    }

    public void DisablePlayerInput(bool enabled)
    {
        _player.GetComponent<PlayerController>().enabled = enabled; 
    }

    
}
