using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class SkipCutSceneMixerBehaviour : PlayableBehaviour
{
    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    [SerializeField]
    public bool removeListener;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        SkipCutscene trackBinding = playerData as SkipCutscene;

        if (!trackBinding)
            return;
            
        if(removeListener)
            trackBinding.RemoveSkipScene();
        else
            trackBinding.AddSkipScene();
    }
}
