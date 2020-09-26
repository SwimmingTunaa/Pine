using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerPrefsToText : MonoBehaviour
{
    public string playerPrefLabel;
    public string extraText;
    // Start is called before the first frame update
    private TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    void OnEnable()
    {
       text.text = PlayerPrefs.GetInt(playerPrefLabel).ToString() + extraText;
    }

}
