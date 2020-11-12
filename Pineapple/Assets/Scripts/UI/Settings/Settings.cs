using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [Header("Music")]
    public float musicMaxVolume = 0.7f;
    public AudioSource musicSource;
    public GameObject musicChecker;

    [Header("Audio")]
    public GameObject audioChecker;

    void Start()
    {
        if(PlayerPrefs.GetInt("FirstTimeStart") == 1)
        {
            musicSource.volume = PlayerPrefs.GetFloat("Music Volume") == 0 ? 0 : musicMaxVolume;
            musicChecker.SetActive(PlayerPrefs.GetFloat("Music Volume") == 1 ? false : true);
            AudioListener.volume = PlayerPrefs.GetFloat("Audio Volume");
            audioChecker.SetActive(PlayerPrefs.GetFloat("Audio Volume") == 1 ? false : true);
        }
        else
        {
            musicSource.volume = musicMaxVolume;
            musicChecker.SetActive(PlayerPrefs.GetFloat("Music Volume") == 1 ? false : true);
            AudioListener.volume = 1;
            audioChecker.SetActive(AudioListener.volume == 1 ? false : true);
        }
    }

    public void ToggleMusic()
    {
        musicSource.volume = musicSource.volume == musicMaxVolume ? 0 : musicMaxVolume;
        PlayerPrefs.SetFloat("Music Volume", musicSource.volume == musicMaxVolume ? 1: 0);
        musicChecker.SetActive(PlayerPrefs.GetFloat("Music Volume") == 1 ? false : true);
    }

    public void ToggleMusic(GameObject marker)
    {
        musicSource.volume = musicSource.volume == musicMaxVolume ? 0 : musicMaxVolume;
        PlayerPrefs.SetFloat("Music Volume", musicSource.volume == musicMaxVolume ? 1 : 0);
        marker.SetActive(PlayerPrefs.GetFloat("Music Volume") == 1 ? false : true);
    }

    public void ToggleAudio()
    {
        AudioListener.volume = AudioListener.volume == 1 ? 0: 1;
        PlayerPrefs.SetFloat("Audio Volume", AudioListener.volume);
        audioChecker.SetActive(AudioListener.volume == 1 ? false : true);
    }

    public void ToggleAudio(GameObject marker)
    {
        AudioListener.volume = AudioListener.volume == 1 ? 0: 1;
        PlayerPrefs.SetFloat("Audio Volume", AudioListener.volume);
        marker.SetActive(AudioListener.volume == 1 ? false : true);
    }

    public void InitialiseAudioMarker(GameObject marker)
    {
        marker.SetActive(AudioListener.volume == 1 ? false : true);
    }

      public void InitialiseMusicMarker(GameObject marker)
    {
        marker.SetActive(musicSource.volume == 1 ? false : true);
    }

    
}
