using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PuncherPickup : PickUpsBase
{
    [Header("Puncher")]
    public GameObject puncherObj;
    public GameObject visual;
    public float speedIncrease;

    [Header("VFX")]
    private PlayerController pController;
    public GameObject transformVFX;
    public AudioClip transformSFX;

    private Outfits outfit;
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
            _health =   CharacterManager.activeCharacter.GetComponent<PlayerHealth>();
            DoAction(_health.gameObject);
        }
    }

    public override void Update()
    {
        print(item.Instance.effectDuration);
        if(_effectActive && (_timerActive && item.Instance.effectDuration > 0 && Timer(item.Instance.effectDuration) ||_health != null && _health.health <= 1))
        {
            DisablePickUp();
        }        
    }

    public override void DoAction(GameObject player)
    {
        _health.AddHealth(1);
        _effectActive = true;
        visual.SetActive(false);
        _sfxActive = true;
        StartCoroutine(ActivateEffect(player, true));
        base.DoAction(player);
    }
    
    public override void DisablePickUp()
    {
        _sfxActive = true;
        if(!_disableEffect)
        {
            StartCoroutine(ActivateEffect(_health.gameObject, false));
            _disableEffect = true;
            _health.Invulnerable(1.5f);
            MasterSpawner.Instance.ChangeMinMax(0,0, true);
        }
        triggerAmount = 1;
    }

    IEnumerator ActivateEffect(GameObject player, bool enableEffect)
    {
        if(_sfxActive)
        {
            GameManager.Instance.ToggleVFXCamera(true);
            //Pause player and everything else
            Time.timeScale = 0;
            //Zoom into players body
            WaitForSecondsRT wait = new WaitForSecondsRT(0.5f);
            while(_sfxActive)
            {
                _anim = CharacterManager.activeCharacter.GetComponentInChildren<Animator>();
                outfit = CharacterManager.activeCharacter.GetComponentInChildren<Outfits>();
                //shows face change
                _anim.SetBool("Fierce", enableEffect);
                yield return wait.NewTime(0.2f);
                CameraShake.Instance.ShakeCamera(.7f, .6f, 5);
                //play the transform animation
                _anim.SetTrigger("ItemAcquired");
                //wait till middle of smoke effect to play sound
                _anim.GetComponentInParent<AudioSource>().PlayOneShot(transformSFX);
                //wait for sound to pop at right moment
                 yield return wait.NewTime(0.65f);
                //create the effect
                transformVFX.SetActive(true);
                transformVFX.transform.position = player.transform.position;
                transformVFX.transform.parent = null;
                 //wait for VFX to die out
                yield return wait.NewTime(0.65f);
                
                //adds speed to player
                if(pController == null)
                    pController =  player.GetComponent<PlayerController>();
                if(enabled)
                    pController.speed += speedIncrease;
                else
                    pController.speed -= speedIncrease;
                //pController.speed = enableEffect ? pController.speed += speedIncrease: pController.speed -=speedIncrease;
                GameManager.Instance.ToggleVFXCamera(false);
                Time.timeScale = 1;
                
                //move the glove puncher to the player
                puncherObj.SetActive(enableEffect);
                puncherObj.transform.position = outfit.glovePunchSlot.position;
                puncherObj.transform.parent = outfit.glovePunchSlot;
                outfit.puncherOutFit.SetActive(enableEffect);
                //outfit.transform.parent = player.GetComponent<PlayerController>().playerItemSlots.bodySlot;
                //outfit.transform.position = player.GetComponent<PlayerController>().playerItemSlots.bodySlot.position;
                if(!enableEffect)
                {
                    if(_health.health > 1)
                        _health.health = 1;
                    _effectActive = false;
                    GetComponent<ObjectID>().Disable();
                }
                //make it so that it spawns obstacles for player to punch
                MasterSpawner.Instance.ChangeMinMax(8f,10f, false);
                _sfxActive = false;
            }
        }
    }
}
