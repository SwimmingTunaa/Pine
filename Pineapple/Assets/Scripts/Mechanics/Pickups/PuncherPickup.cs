using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PuncherPickup : PickUpsBase
{
    [Header("Puncher")]
    public GameObject puncherObj;
    public GameObject visual;

    [Header("VFX")]
    public GameObject mainCam;
    public GameObject vfxCamera;
    private PlayerController pController;
    public GameObject transformVFX;
    public AudioClip transformSFX;
    
    private GameObject outfit;
    private PlayerHealth _health;
    private Animator _anim;
    private bool _sfxActive = false;
    private bool _effectActive = false;
    private bool _disableEffect;

    void OnEnable()
    {
        puncherObj.SetActive(false);
        visual.SetActive(true);
        _disableEffect = false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(triggerAmount >= 1 && (other.CompareTag("Player") || other.CompareTag("Hair")))
        {
            triggerAmount--;
            _health = other.gameObject.GetComponentInParent<PlayerHealth>();
            DoAction(_health.gameObject);
        }
    }

    public override void Update()
    {
        if(_effectActive && (_timerActive && item.Instance.effectDuration > 0 && Timer(item.Instance.effectDuration) ||_health != null && _health.health <= 1))
        {
            DisablePickUp();
            Debug.Log("Disable Glove");
        }        
    }

    public override void DoAction(GameObject player)
    {
        _health.AddHealth(1);
        _effectActive = true;
        visual.SetActive(false);
        _sfxActive = true;
        Debug.Log("Enable Glove");
        StartCoroutine(ActivateEffect(player, true));
        base.DoAction(player);
        transform.parent = player.GetComponent<PlayerController>().playerItemSlots.bodySlot;
        transform.position = player.transform.position;
    }
    
    public override void DisablePickUp()
    {
        _sfxActive = true;
        if(!_disableEffect)
        {
            _disableEffect = true;
            StartCoroutine(ActivateEffect(_health.gameObject, false));
        }
        triggerAmount = 1;
    }

    IEnumerator ActivateEffect(GameObject player, bool enableEffect)
    {
        if(_sfxActive)
        {
            GameManager.Instance.vfxVirtualCamera.Follow = CharacterManager.activeCharacter.transform;
            GameManager.Instance.followVirtualCamera.gameObject.SetActive(false);
            GameManager.Instance.vfxVirtualCamera.gameObject.SetActive(true);
            //Pause player and everything else
            Time.timeScale = 0;
            //Zoom into players body
            WaitForSecondsRT wait = new WaitForSecondsRT(0.5f);
            while(_sfxActive)
            {
                _anim = CharacterManager.activeCharacter.GetComponentInChildren<Animator>();
                outfit = CharacterManager.activeCharacter.GetComponent<Outfits>().outfit.puncherOutfit;
                //shows face change
                _anim.SetBool("Fierce", enableEffect);
                yield return wait.NewTime(0.2f);
                //play the transform animation
                _anim.SetTrigger("ItemAcquired");
                Instantiate(transformVFX,player.transform.position,player.transform.rotation);
                //wait till middle of smoke effect to play sound
                _anim.GetComponentInParent<AudioSource>().PlayOneShot(transformSFX);
                //wait for VFX to die out
                yield return wait.NewTime(1.4f);
                //adds speed to player
                if(pController == null)
                    pController =  player.GetComponent<PlayerController>();
                pController.speed = enableEffect ? pController.speed += 10: pController.speed -=10;
                GameManager.Instance.followVirtualCamera.gameObject.SetActive(false);
                GameManager.Instance.vfxVirtualCamera.gameObject.SetActive(false);
                Time.timeScale = 1;

                puncherObj.SetActive(enableEffect);
                outfit.SetActive(enableEffect);
                //outfit.transform.parent = player.GetComponent<PlayerController>().playerItemSlots.bodySlot;
                //outfit.transform.position = player.GetComponent<PlayerController>().playerItemSlots.bodySlot.position;
                if(!enableEffect)
                {
                    if(_health.health > 1)
                        _health.health = 1;
                    _effectActive = false;
                    GetComponent<ObjectID>().Disable();
                }
                _sfxActive = false;
            }
        }
    }
}
