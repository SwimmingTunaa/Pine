using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    public static ParallaxManager instance;
    public GameObject caveParallax;

    public Dictionary<string, GameObject> parallaxDic;

    void Start()
    {
        instance = this;
        parallaxDic = new Dictionary<string, GameObject>
        {
            {caveParallax.name, caveParallax}
        };
    }
    
    public void ToggleParallax(bool activeState, string parallaxName)
    {
        parallaxDic[parallaxName].SetActive(activeState);
    }

    public void ChangeParallax(string parallaxName)
    {
        if(!parallaxDic[parallaxName].activeInHierarchy)
        {
            //turn off other parallax
            foreach(KeyValuePair<string, GameObject> p in parallaxDic)
            {
                if(p.Value.activeInHierarchy) p.Value.SetActive(false);
            }
            //turn on new parallax
            parallaxDic[parallaxName].SetActive(true);
        }
    }
}
