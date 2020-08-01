using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGeneric : MonoBehaviour {

	public float health = 2;
	public bool dead = false;
	[HideInInspector] public float startHealth;
	
	void Awake () {
		 startHealth = health;
	}
	virtual public void TakeDamage(float damage)
    {	
		if(health > 0 && !dead)
			health -= damage;
    }

	virtual public void Dead()
	{
		dead = true;
		gameObject.transform.parent.gameObject.SetActive(false);
	}
}
