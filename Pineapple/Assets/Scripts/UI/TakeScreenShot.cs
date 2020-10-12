using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class TakeScreenShot : MonoBehaviour
{
    public Camera screenshotCamera;
    public IEnumerator ScreenShot()
	{
        yield return new WaitForEndOfFrame();
        screenshotCamera.Render();
	}
}
