using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(1f, 0.1603289f, 0f)]
[TrackClipType(typeof(SkipCutSceneClip))]
[TrackBindingType(typeof(SkipCutscene))]
public class SkipCutSceneTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<SkipCutSceneMixerBehaviour>.Create (graph, inputCount);
    }
}
