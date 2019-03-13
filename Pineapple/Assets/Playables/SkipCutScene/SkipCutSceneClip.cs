using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class SkipCutSceneClip : PlayableAsset, ITimelineClipAsset
{
    public SkipCutSceneMixerBehaviour template = new SkipCutSceneMixerBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SkipCutSceneMixerBehaviour>.Create (graph, template);
        SkipCutSceneMixerBehaviour clone = playable.GetBehaviour ();
        return playable;
    }
}
