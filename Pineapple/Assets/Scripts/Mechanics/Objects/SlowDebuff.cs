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

   private GameObject _tomato;
   private PlayerController _playerController;
   private GameManager _gameManager;

   void Start()
   {
      GetComponent<ObjectID>().selfDestroy = true;
      gameObject.SetActive(true);
      _playerController  = CharacterManager.activeCharacter.GetComponent<PlayerController>();
      _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
      _tomato = _gameManager.chaser.gameObject;
   }

   void OnEnable()
   {
      triggerAmount = 1;
   }

   void OnTriggerEnter2D(Collider2D other)
   {
      _playerController  = CharacterManager.activeCharacter.GetComponent<PlayerController>();
      _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
      _tomato = _gameManager.chaser.gameObject;
      if(other.CompareTag("Player") && triggerAmount > 0)
      {  
         Debug.Log("times hit: " + timesHit);
         DoAction(other.gameObject);
         switch(timesHit)
         {
            case 0:
                  _gameManager.chaseVirtualCamera.gameObject.SetActive(true);
                  _gameManager.chaseVirtualCamera.Follow = _playerController.cameraFollowTarget;
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
         Debug.Log("times hit added: " + timesHit );
      }     
   }

   public override void DoAction(GameObject player)
   {
      //TODO: Make it so that it resets before it gets destroyed by panelDestroyer
      base.DoAction(player);
      if(_playerController.speed > 60)
         _playerController.speed -= slowAmount;
      _playerController._anim.SetTrigger("Trip");
      _playerController._anim.SetTrigger("Sad");
      GetComponentInChildren<Animator>().Play("Explode",0);
      //visuals.SetActive(false);
   }
   public override void DisablePickUp()
   {
      if(timesHit <= 1)
      {
         _tomato.GetComponentInChildren<Animator>().Play("Disappear");
         //objects get turn off by animation - see TurnOffGameObjectOnExit
         TomatoController.chasePlayer = false;
      }
      base.DisablePickUp();
      Reset();
   }

   public void Reset()
   {
      _gameManager.chaseVirtualCamera.gameObject.SetActive(false);
      _playerController.speed += slowAmount;
      triggerAmount = 1;
      timesHit = 0;
   }
}
