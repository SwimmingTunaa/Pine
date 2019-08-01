using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderingSortingLayer : MonoBehaviour
{
    public int sortLayer = 0;
    public string sortinglayerName;

    void Awake()
    {
        Renderer renderer = this.gameObject.GetComponent<Renderer>();
        renderer.sortingOrder = sortLayer;
        renderer.sortingLayerName = sortinglayerName;
    }

}
