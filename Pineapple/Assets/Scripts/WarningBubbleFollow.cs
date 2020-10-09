using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WarningBubbleFollow : MonoBehaviour
{
    private Camera _camera;
    void Awake()
    {
        DontDestroyOnLoad(this);
        _camera = Camera.main;
        // SceneManager.MoveGameObjectToScene(gameObject,)
    }
}
