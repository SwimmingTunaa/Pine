using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pauseGame()
    {
        Time.timeScale = 0; // TODO: Not sure if this is correct. Something about timescale pausing all scripts and everything?
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
    }
}
