using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonInputMove : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerController playerController;
    public float dir = 1f;

    public void OnPointerDown(PointerEventData data)
    {
        playerController._horiMove = dir;
        Debug.Log(playerController._horiMove);
    }

    public void OnPointerUp(PointerEventData data)
    {
        playerController._horiMove = 0;
    }
}
