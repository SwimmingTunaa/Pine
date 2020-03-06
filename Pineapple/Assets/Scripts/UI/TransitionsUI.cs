using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class TransitionsUI : MonoBehaviour
{
    public CinemachineBrain cinemachine;
    public void DelayTransitionEnable(GameObject obj)
    {
        StartCoroutine(Delay(obj, true));
    }
    public void DelayTransitionDisable(GameObject obj)
    {
        StartCoroutine(Delay(obj, false));
    }
   
   public void CameraBlendFinishEnable(GameObject obj)
   {
      StartCoroutine(CameraBlendFinish(obj, true));
   }

   public void CameraBlendFinishDisable(GameObject obj)
   {
       StartCoroutine(CameraBlendFinish(obj,false));
   }

   IEnumerator Delay(GameObject obj, bool active)
   {
       yield return new WaitForSeconds(0.5f);
       obj.SetActive(active);
   }
    IEnumerator CameraBlendFinish(GameObject obj, bool active)
    {
        yield return null;
        while(cinemachine.IsBlending)
        yield return null;
        obj.SetActive(active);
    }
}
