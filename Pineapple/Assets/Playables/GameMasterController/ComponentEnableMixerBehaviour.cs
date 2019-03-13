using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
[Serializable]
public class ComponentEnableMixerBehaviour : PlayableBehaviour
{
    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    [SerializeField]
    private bool enable;
    private Behaviour trackBinding;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        trackBinding = playerData as Behaviour;
        if (!trackBinding)
            return;

        trackBinding.enabled = enable;
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (trackBinding == null)
            return;
        trackBinding.enabled = !trackBinding.enabled;
        base.OnBehaviourPause(playable,info);
    }
}
