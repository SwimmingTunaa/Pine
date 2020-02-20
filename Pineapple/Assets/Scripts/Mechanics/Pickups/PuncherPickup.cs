using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuncherPickup : PickUpsBase
{
    [Header("Puncher")]
    public GameObject puncherObj;
    public GameObject outfit;
    public GameObject visual;

    [Header("VFX")]
    public GameObject mainCam;
    public GameObject vfxCamera;
    private PlayerController pController;
    public GameObject transformVFX;
    public AudioClip transformSFX;

    private PlayerHealth _health;
    private Animator _anim;
    private bool _sfxActive = false;
    private bool _effectActive = false;

    void OnEnable()
    {
        puncherObj.SetActive(false);
        visual.SetActive(true);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && triggerAmount >= 1)
        {
            _health = other.gameObject.GetComponent<PlayerHealth>();
            DoAction(other.gameObject);
        }
    }

    public override void Update()
    {
        if(_effectActive && (_timerActive && effectDuration > 0 && Timer(effectDuration) ||_health != null && _health.health <= 1))
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
        StartCoroutine(ActivateEffect(_health.gameObject, false));
    }

    IEnumerator ActivateEffect(GameObject player, bool enableEffect)
    {
        if(_sfxActive)
        {
            mainCam.SetActive(false);
            vfxCamera.SetActive(true);
            //Pause player and everything else
            Time.timeScale = 0;
            //Zoom into players body
            WaitForSecondsRT wait = new WaitForSecondsRT(0.5f);
            while(_sfxActive)
            {
                //shows face change
                _anim = player.GetComponentInChildren<Animator>();
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
                mainCam.SetActive(true);
                vfxCamera.SetActive(false);
                Time.timeScale = 1;

                puncherObj.SetActive(enableEffect);
                outfit.SetActive(enableEffect);
                outfit.transform.parent = player.GetComponent<PlayerController>().playerItemSlots.bodySlot;
                outfit.transform.position = player.GetComponent<PlayerController>().playerItemSlots.bodySlot.position;
                if(!enableEffect)
                {
                    if(_health.health > 1)
                        _health.health--;
                    _effectActive = false;
                    GetComponent<ObjectID>().Disable();
                }
                  
                _sfxActive = false;
            }
        }
    }
}
