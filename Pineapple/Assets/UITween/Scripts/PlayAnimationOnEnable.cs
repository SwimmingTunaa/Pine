using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationOnEnable : MonoBehaviour
{
    public EasyTween EasyTweenStart;

    void OnEnable()
    {
        EasyTweenStart?.TriggerOpenClose();
    }
}
