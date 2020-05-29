using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [Header("Music")]
    public AudioSource musicSource;
    public GameObject musicChecker;

    [Header("Audio")]
    public GameObject audioChecker;

    void Start()
    {
        musicSource.volume = PlayerPrefs.GetFloat("Music Volume");
        musicChecker.SetActive(PlayerPrefs.GetFloat("Music Volume") == 1 ? false : true);
        AudioListener.volume = PlayerPrefs.GetFloat("Audio Volume");
        audioChecker.SetActive(PlayerPrefs.GetFloat("Audio Volume") == 1 ? false : true);
    }

    public void ToggleMusic()
    {
        musicSource.volume = musicSource.volume == 1 ? 0 : 1;
        PlayerPrefs.SetFloat("Music Volume", musicSource.volume);
        musicChecker.SetActive(PlayerPrefs.GetFloat("Music Volume") == 1 ? false : true);
    }

    public void ToggleMusic(GameObject marker)
    {
        musicSource.volume = musicSource.volume == 1 ? 0 : 1;
        PlayerPrefs.SetFloat("Music Volume", musicSource.volume);
        marker.SetActive(PlayerPrefs.GetFloat("Music Volume") == 1 ? false : true);
    }

    public void ToggleAudio()
    {
        AudioListener.volume = AudioListener.volume == 1 ? 0: 1;
        PlayerPrefs.SetFloat("Audio Volume", AudioListener.volume);
        audioChecker.SetActive(PlayerPrefs.GetFloat("Audio Volume") == 1 ? false : true);
    }

    public void ToggleAudio(GameObject marker)
    {
        AudioListener.volume = AudioListener.volume == 1 ? 0: 1;
        PlayerPrefs.SetFloat("Audio Volume", AudioListener.volume);
        marker.SetActive(PlayerPrefs.GetFloat("Audio Volume") == 1 ? false : true);
    }

    public void InitialiseAudioMarker(GameObject marker)
    {
        marker.SetActive(PlayerPrefs.GetFloat("Audio Volume") == 1 ? false : true);
    }

      public void InitialiseMusicMarker(GameObject marker)
    {
        marker.SetActive(PlayerPrefs.GetFloat("Music Volume") == 1 ? false : true);
    }

    
}
