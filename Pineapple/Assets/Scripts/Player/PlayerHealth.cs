using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthGeneric {

	public GameObject deathEffect;
	public GameObject deathEffectHairCut;

	override public void TakeDamage(float damage)
	{
		base.TakeDamage(damage);
		if(health <= 0 && !dead)
		{
			//player is dead
			dead = true;
			Instantiate(GetComponent<PlayerController>()._haircut ? deathEffectHairCut : deathEffect, transform.position + Vector3.up * 2.5f, transform.rotation);
			gameObject.SetActive(false);
		}
	}

	public void AddHealth(float amount)
	{
		health += amount;
	}
}
