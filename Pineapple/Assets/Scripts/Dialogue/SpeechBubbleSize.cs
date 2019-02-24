using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SpeechBubbleSize : MonoBehaviour
{
    public Sprite sm;
    public Sprite md;
    public Sprite lg;

    private RectTransform _textMesh;
    private Vector2 _startPos;

    void Awake()
    {
        _textMesh = GetComponentInChildren<RectTransform>();
        _startPos = _textMesh.anchoredPosition;
    }
    public Sprite GetSpeechBubbleSize(DialogueConfig d)
    {
        switch(d.bubbleSize)
        {
            case Enums.BubbleSize.sm:
                moveTextPos(0);
                return sm;
            case Enums.BubbleSize.md:
                moveTextPos(0.6f);
                return md;
            case Enums.BubbleSize.lg:
                moveTextPos(0.6f);
                return lg;
            default:
                moveTextPos(0);
                return md;
        }
    }
// make the text in the middle of the speech bubble, hard coded in the values
    void moveTextPos(float heightToMove)
    {
        _textMesh.anchoredPosition = _startPos + Vector2.up * heightToMove;
    }
}
