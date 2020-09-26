using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideScrollBar : MonoBehaviour
{
    public float transitionSpeed = 1;
    private Color startBGColor;
    private Color startBarColor;
    private Color targetColor;
    private Color targetColorTwo;

    private Image BGimage;
    private Scrollbar scrollbar;
    private bool trigger;
    void Awake()
    {
        scrollbar = GetComponent<Scrollbar>();
        BGimage = GetComponent<Image>();

        startBGColor = BGimage.color;
        startBarColor = scrollbar.image.color;
        targetColor = new Color(startBGColor.r,startBGColor.g,startBGColor.b,255);
        targetColorTwo =  new Color(startBarColor.r,startBarColor.g,startBarColor.b,255);
    }

    public void ShowScrollBar()
    {
        if(scrollbar?.value <= 0.6f)
        {
            trigger = true;
        }
        else
            {
                trigger = false;
            }
    }

    void Update()
    {
        if(trigger && (BGimage.color != targetColor || scrollbar.image.color != targetColorTwo))
        {
            BGimage.color = Color.Lerp(BGimage.color,targetColor, Time.deltaTime * transitionSpeed);
            scrollbar.image.color = Color.Lerp(scrollbar.image.color,targetColorTwo, Time.deltaTime * transitionSpeed);
        }
        if(!trigger && (BGimage.color != startBGColor || scrollbar.image.color != startBarColor))
        {
            BGimage.color = Color.Lerp(BGimage.color,startBGColor, Time.deltaTime * 8f);
            scrollbar.image.color = Color.Lerp(scrollbar.image.color,startBarColor, Time.deltaTime * 8f);
        }
    }
}
