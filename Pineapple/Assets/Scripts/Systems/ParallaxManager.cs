using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    public static ParallaxManager instance;
    public GameObject currentActiveParallax;

    public GameObject mainParallax;
    public GameObject caveParallax;
    public GameObject cloudParallax;

    public Dictionary<string, GameObject> parallaxDic;

    private GameObject _previousParallax;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        parallaxDic = new Dictionary<string, GameObject>
        {
            {"Cave", caveParallax},
            {"Cloud", cloudParallax},
            {"House", mainParallax},
            {"Garden", mainParallax},
            {"Storm", cloudParallax},
            {"Forest", mainParallax}
        };
    }
    
    public void ToggleParallax(bool activeState, string parallaxName)
    {
        parallaxDic[parallaxName].SetActive(activeState);
    }

    public void ChangeParallax()
    {
        _previousParallax = currentActiveParallax;
        if(!parallaxDic[MasterSpawner.Instance.activeRegion.tag].activeInHierarchy)
        {
            
            StartCoroutine(DelayDisable(_previousParallax));

            //turn on new parallax
            currentActiveParallax = parallaxDic[MasterSpawner.Instance.activeRegion.tag];
            currentActiveParallax.SetActive(true);
        }
    }

    IEnumerator DelayDisable(GameObject obj)
    {
        yield return new WaitForSeconds(3f);
        obj.SetActive(false);
    }
}
