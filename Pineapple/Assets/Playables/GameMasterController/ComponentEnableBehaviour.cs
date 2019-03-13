using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class ComponentEnableBehaviour : PlayableBehaviour
{
    public bool enable;

    public override void OnPlayableCreate (Playable playable)
    {
        
    }
}
