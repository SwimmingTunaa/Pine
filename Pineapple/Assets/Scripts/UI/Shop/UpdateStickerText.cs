using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UpdateStickerText : MonoBehaviour
{
    private TextMeshProUGUI totalStickers;

    void Awake()
    {
        totalStickers = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        totalStickers.text = PlayerPrefs.GetInt("TotalStickers").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        totalStickers.text = PlayerPrefs.GetInt("TotalStickers").ToString();
    }
}
