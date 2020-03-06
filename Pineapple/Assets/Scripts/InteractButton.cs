using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractButton : MonoBehaviour
{
    public PlayerController playerController;

    // DoAction callback methods
    public delegate void DoActionFn();
    private DoActionFn _doActionFn;

    [SerializeField]
    private Image _interactButtonBg; // Needs to be serializable for editor due to mask image existing as well.


    [SerializeField]
    private Sprite _defaultBg;
    [SerializeField]
    private string _defaultText;
    [SerializeField]
    private Color _activateColor;
    [SerializeField]
    private Color _deactivateColor;
    private Color _defaultColor;
    private GameObject _player;
    private Image _interactButtonImg;
    private Interactable _interactableObject;
    public TextMeshProUGUI _interactButtonText;
    private GameObject _setter;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _interactButtonImg = GetComponentInChildren<Image>();
      /*  _defaultColor = _interactButtonImg.color;
        _activateColor.a = _defaultColor.a;
        _deactivateColor.a = _defaultColor.a;*/
        resetBtn(gameObject);
    }

    private bool CheckSetter(GameObject setter)
    {
        if (_setter == null)
        {
            _setter = setter;
            return true;
        }

        return _setter.Equals(setter);
    }

    public void DoAction()
    {   
        if(_doActionFn != null)
        {
            _doActionFn();
            // TODO: We potentially want to clear _doActionFn here, but this means we can only ever doAction once from another script,
            // It's more flexible to trust that any script setting a DoActionFn will also clear it in its callback.
        }
        else if(_interactableObject == null)
        {
            playerController.setJump(1f); // Not convinced we want this, I think we should encourage user to learn the tap jump
        }
        else
        {
            _interactableObject.DoAction(_player);
        }
    }

    public void setDoAction(GameObject setter, DoActionFn doActionFn)
    {
        if (CheckSetter(setter))
        {
            _doActionFn = doActionFn;
        }
    }

    public void clearDoAction(GameObject setter)
    {
        if (CheckSetter(setter))
        {
            _doActionFn = null;
        }
    }

    public void setText(GameObject setter, string txt)
    {
        if (_interactButtonText.gameObject.activeInHierarchy && CheckSetter(setter))
        {
            _interactButtonText.text = txt;
        }
    }

    public void setBgImage(GameObject setter, Sprite img)
    {
        if (CheckSetter(setter))
        {
            if (img != null)
            {
                _interactButtonBg.enabled = true;
                _interactButtonBg.sprite = img;
            }
            else
            {
                _interactButtonBg.enabled = false;
            }
        }
    }

    public void setColor(GameObject setter, Enums.InteractColor color)
    {
        if (CheckSetter(setter))
        {
            switch (color)
            {
                case Enums.InteractColor.deactivate:
                    _interactButtonImg.color = _deactivateColor;
                    break;
                case Enums.InteractColor.activate:
                    _interactButtonImg.color = _activateColor;
                    break;
                default:
                    _interactButtonImg.color = _defaultColor;
                    break;
            }
        }
    }

    public void setInteractable(GameObject setter, Interactable _interactable)
    {
        if (CheckSetter(setter))
        {
            _interactableObject = _interactable;
        }
    }

    public void resetBtn(GameObject setter)
    {
        if (CheckSetter(setter))
        {
            _setter = null;
            set(null, _defaultText, _defaultBg, Enums.InteractColor.none, null);
            _doActionFn = null;
        }
    }

    public void forceReset()
    {
        _setter = null;
        resetBtn(null);
    }

    public void set(GameObject setter, string txt, Sprite img, Enums.InteractColor color, Interactable _interactable)
    {
        if (CheckSetter(setter))
        {
            setBgImage(setter, img);
            setText(setter, txt);
            setColor(setter, color);
            setInteractable(setter, _interactable);
        }
    }
}
