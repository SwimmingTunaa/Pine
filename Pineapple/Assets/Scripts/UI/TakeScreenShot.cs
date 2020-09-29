using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class TakeScreenShot : MonoBehaviour
{
    public Image screenshotUI;
    public CinemachineVirtualCamera screenshotCamera;
    
    public IEnumerator ScreenShot()
	{
        screenshotCamera.Follow = CharacterManager.activeCharacter.transform;
        screenshotCamera.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        Texture2D _screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        _screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        _screenshotTexture.Apply();
        Sprite tempsprite = Sprite.Create(_screenshotTexture, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0,0));
		screenshotUI.sprite = tempsprite;
        screenshotUI.SetNativeSize();
        screenshotUI.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        screenshotUI.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        screenshotUI.rectTransform.anchoredPosition = Vector2.zero;
        screenshotCamera.gameObject.SetActive(false);
	}
}
