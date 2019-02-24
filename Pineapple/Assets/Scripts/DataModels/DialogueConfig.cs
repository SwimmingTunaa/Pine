using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public struct DialogueConfig 
{
    public GameObject character;
    [TextArea]
    public string text;
    public float diallogueInterval;
    public Enums.BubbleSize bubbleSize;
}
