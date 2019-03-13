using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class SkipCutSceneBehaviour : PlayableBehaviour
{
    public bool removeListener;

    public override void OnPlayableCreate (Playable playable)
    {
        
    }
}
