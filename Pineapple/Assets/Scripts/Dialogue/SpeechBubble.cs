using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBubble : MonoBehaviour
{
    public BubbleConfig smBubble;
    public BubbleConfig mdBubble;
    public BubbleConfig lgBubble;

    private Dictionary<Enums.BubbleSize, BubbleConfig> _configs;

    private RectTransform _textRect;
    private TextMeshPro _textMesh;

    private SpriteRenderer _spriteRenderer;
    private GameObject _attachedCharacter;
    private BubbleConfig _currentConfig;

    private void Awake()
    {
        _configs = new Dictionary<Enums.BubbleSize, BubbleConfig>
        {
            {Enums.BubbleSize.sm, smBubble},
            {Enums.BubbleSize.md, mdBubble},
            {Enums.BubbleSize.lg, lgBubble}
        };

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _textRect = GetComponentInChildren<RectTransform>();
        _textMesh = GetComponentInChildren<TextMeshPro>(); 
        _attachedCharacter = CharacterManager.activeCharacter;       
    }

    private void Update()
    {
        //dialogue follow player hack
        if (gameObject.activeInHierarchy && _attachedCharacter != null)
        {
            gameObject.transform.position = _attachedCharacter.transform.position + _currentConfig.bubbleOffset;
        }
    }

    public void set(DialogueConfig dialogue)
    {
        // Get the bubbleConfig of the dialogue bubble size
        _currentConfig = _configs[dialogue.bubbleSize];

        // Store a reference to the attached character's gO
        _attachedCharacter = dialogue.character;

        // Set the bubble text 
        _textMesh.text = dialogue.text;
        // Set the bubble sprite
        _spriteRenderer.sprite = _currentConfig.sprite;
        // Set the bubble relative to the character talking
        if(_attachedCharacter != null)
            this.transform.position = _attachedCharacter.transform.position + _currentConfig.bubbleOffset;

        // Set the text relative to bubble size.
        _textRect.localPosition = _currentConfig.textOffset;
    }
}
