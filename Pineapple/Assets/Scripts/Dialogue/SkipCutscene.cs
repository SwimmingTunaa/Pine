using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.UI;

public class SkipCutscene : MonoBehaviour
{
    public TimelineAsset skipTimeLine;
    public PlayableDirector director;

    private Button button;
    
    public void Awake()
    {
        button = GetComponent<Button>();
    }
    public void Start()
    { 
        AddSkipScene();
    }
    public void SkipScene()
    {
        director.playableAsset = skipTimeLine;
        director.RebuildGraph();
        director.time = 0.0;
        director.Play();
        button.onClick.RemoveListener(SkipScene);
    }

    public void AddSkipScene()
    {
        if(button != null)
            button.onClick.AddListener(SkipScene);
    }

    public void RemoveSkipScene()
    {
        if(button != null)
            button.onClick.RemoveListener(SkipScene);
    }
}
