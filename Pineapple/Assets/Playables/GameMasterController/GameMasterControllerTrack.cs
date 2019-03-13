using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0f, 0.5f, 1f)]
[TrackClipType(typeof(GameMasterControllerClip))]
[TrackBindingType(typeof(Behaviour))]
public class GameMasterControllerTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<ComponentEnableMixerBehaviour>.Create (graph, inputCount);
    }
}
