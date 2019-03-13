using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class GameMasterControllerClip : PlayableAsset, ITimelineClipAsset
{
    public ComponentEnableMixerBehaviour template = new ComponentEnableMixerBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ComponentEnableMixerBehaviour>.Create (graph, template);
        ComponentEnableMixerBehaviour clone = playable.GetBehaviour ();
        return playable;
    }
}
