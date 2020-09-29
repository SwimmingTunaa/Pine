using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixLevels : MonoBehaviour
{
    public static MixLevels Instance;
    public AudioMixer masterMixer;
    public float fadeSpeed = 2;
    public AudioSource GOMaudio;
    private bool _fadeBGM;

    public void Awake()
    {
        if(Instance == null)
        Instance = this;
    }

    public void FadeBGMtoGOM(bool fade)
    {
        _fadeBGM = fade;
    }

    public void Update()
    {
        if(_fadeBGM)
        {
            GOMaudio.gameObject.SetActive(true);
            bool result = (masterMixer.GetFloat("BGM Vol",out float value));
            bool result2 = (masterMixer.GetFloat("GOM Vol",out float value2));
            masterMixer.SetFloat("BGM Vol", Mathf.Lerp(value , -80f, Time.deltaTime * fadeSpeed));
            masterMixer.SetFloat("GOM Vol", Mathf.Lerp(value2, -7f, Time.deltaTime * fadeSpeed));
            if(value == -80f)
                _fadeBGM = false;
        }
    }
}
