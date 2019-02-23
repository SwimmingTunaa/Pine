using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderingSortingLayer : MonoBehaviour
{
    public int sortingLayer = 0;
    public int sortingLayerID = 0;

    void Awake()
    {
        Renderer renderer = this.gameObject.GetComponent<Renderer>();
        renderer.sortingOrder = sortingLayer;
        renderer.sortingLayerID = sortingLayerID;
    }

}
