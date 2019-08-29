using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SlowDebuff : PickUpsBase
{
   [Header("Slow Debuff")]
   public float slowAmount;

   private GameObject visuals;
   private PlayerController _playerController;

   void Start()
   {
      gameObject.SetActive(true);
      _playerController  = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
      visuals = gameObject.GetComponentInChildren<SpriteRenderer>().gameObject;
   }

   void OnEnable()
   {
      triggerAmount = 1;
   }

   IEnumerator OnTriggerEnter2D(Collider2D other)
   {
      if(other.CompareTag("Player") && triggerAmount > 0)
      {  
         DoAction(other.gameObject);
         yield return new WaitForSeconds(effectDuration);
         Reset();
      }
   }

   public override void DoAction(GameObject player)
   {
      base.DoAction(player);
      if(_playerController.speed > 15)
         _playerController.speed -= slowAmount;
      _playerController._anim.SetTrigger("Trip");
      GetComponentInChildren<Animator>().Play("Explode",0);
      //visuals.SetActive(false);
   }

   void Reset()
   {        
      _playerController.speed = _playerController.startSpeed;
      triggerAmount = 1;
      visuals.SetActive(true);
      gameObject.SetActive(false);
   }
}
