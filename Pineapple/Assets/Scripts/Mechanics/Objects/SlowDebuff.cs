using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(AudioSource))]
public class SlowDebuff : PickUpsBase
{
   public static int timesHit = 0;
   [Header("Slow Debuff")]
   public float slowAmount;
   public string animationName;
   public ParticleSystem additionalEffect;
   public GameObject visuals;

   private GameObject _tomato;
   private PlayerController _playerController;
   private static float DEBUFFTIMER;

   void Start()
   {

      GetComponent<ObjectID>().selfDestroy = true;
      gameObject.SetActive(true);
      _tomato = GameManager.Instance.chaser.gameObject;
      _playerController  = CharacterManager.activeCharacter.GetComponent<PlayerController>();
   }

   void OnEnable()
   {
      triggerAmount = 1;
   }
  
   void OnTriggerEnter2D(Collider2D other)
   {
      _tomato = GameManager.Instance.chaser.gameObject;
      _playerController  = CharacterManager.activeCharacter.GetComponent<PlayerController>();
      if(other.CompareTag("Player") && triggerAmount > 0 && !other.GetComponent<PlayerHealth>().shieldActive)
      {  
         DoAction(other.gameObject);
         switch(timesHit)
         {
            case 0:
                  GameManager.Instance.chaseVirtualCamera.gameObject.SetActive(true);
                  GameManager.Instance.chaseVirtualCamera.Follow = _playerController.cameraFollowTarget;
                  _tomato.SetActive(true);
                  _tomato.GetComponentInChildren<Animator>().Play("Appear");
                  TomatoController.chasePlayer = true;
               break;
            case 1:
                  TomatoController.chasePlayer = false;
                  _tomato.GetComponent<TomatoController>().speed = _playerController.speed + 5f;
               break;
            case 2:
                  _tomato.GetComponent<TomatoController>().speed = _playerController.speed + 15f;
               break;
         }
         timesHit ++;
      } 
      else if(other.CompareTag("Player"))
         DisablePickUp();    
   }

   public override bool Timer(float interval)
   {
      DEBUFFTIMER += Time.deltaTime;
        if(DEBUFFTIMER > interval)
        {
            DEBUFFTIMER = 0f;
            return true;
        }
        return false;
   }

   public override void DoAction(GameObject player)
   {
      DEBUFFTIMER = 0;
      base.DoAction(player);
      if(_playerController.speed > 60)
         _playerController.speed -= slowAmount;
      _playerController._anim.SetTrigger("Trip");
      _playerController._anim.SetTrigger("Sad");
      if(animationName.Length > 0)
         GetComponentInChildren<Animator>().Play(animationName,0);
      else 
      {
         visuals.SetActive(false);
         if(additionalEffect) additionalEffect.gameObject.SetActive(true);
      }
   }
   public override void DisablePickUp()
   {
      if(timesHit <= 1)
      {
         _tomato.GetComponentInChildren<Animator>().Play("Dissapear");
         //objects get turn off by animation - see TurnOffGameObjectOnExit
         TomatoController.chasePlayer = false;
      }
      base.DisablePickUp();
      Reset();
   }

   public void Reset()
   {
      GameManager.Instance.chaseVirtualCamera.gameObject.SetActive(false);
      _playerController.speed += slowAmount;
      triggerAmount = 1;
      timesHit = 0;
   }
}
