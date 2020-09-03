using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    public static ParallaxManager instance;
    public bool timerActive;
    public float delayTransitionTime;
    public GameObject currentActiveParallax;

    public GameObject mainParallax;
    public GameObject caveParallax;
    public GameObject cloudParallax;

    public Dictionary<string, GameObject> parallaxDic;

    private GameObject _previousParallax;
    private float _timer;

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

    void Update()
    {
        if(_previousParallax && timerActive && Timer(delayTransitionTime))
        {
            _previousParallax?.SetActive(false);
            timerActive = false;
        }
    }

    public bool Timer(float interval)
    {
        _timer += Time.deltaTime;
        if(_timer > interval)
        {
            _timer = 0f;
            return true;
        }
        return false;
    }
    
    public void ToggleParallax(bool activeState, string parallaxName)
    {
        parallaxDic[parallaxName].SetActive(activeState);
    }

    public void ChangeParallax()
    {
        if(!parallaxDic[MasterSpawner.Instance.activeRegion.tag].activeInHierarchy)
            _previousParallax = currentActiveParallax;
        else
            _previousParallax = null;
        _timer = 0;
        timerActive = true;
        //turn on new parallax
        currentActiveParallax = parallaxDic[MasterSpawner.Instance.activeRegion.tag];
        currentActiveParallax.SetActive(true);
    }

    IEnumerator DelayDisable(GameObject obj)
    {
        yield return new WaitForSeconds(3f);
        obj.SetActive(false);
    }
}
