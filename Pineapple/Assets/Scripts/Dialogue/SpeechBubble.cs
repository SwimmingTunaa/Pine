using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    public BubbleConfig smBubble;
    public BubbleConfig mdBubble;
    public BubbleConfig lgBubble;

    private TextMesh _textMesh;
    private SpriteRenderer _spriteRenderer;
    private Dictionary<Enums.BubbleSize, BubbleConfig> _configs;

    private void Awake()
    {
        _configs = new Dictionary<Enums.BubbleSize, BubbleConfig>
        {
            {Enums.BubbleSize.sm, smBubble},
            {Enums.BubbleSize.md, mdBubble},
            {Enums.BubbleSize.lg, lgBubble}
        };

        _textMesh = GetComponent<TextMesh>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        gameObject.SetActive(false);
    }

    public void set(DialogueConfig dialogue)
    {
        BubbleConfig config = _configs[dialogue.bubbleSize];
        print(dialogue.text);
        this.transform.position = dialogue.character.transform.position + config.offset;
        //  _spriteRenderer.sprite = config.sprite;
        _textMesh.text = dialogue.text;
    }
}
