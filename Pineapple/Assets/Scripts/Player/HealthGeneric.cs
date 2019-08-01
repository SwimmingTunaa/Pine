using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGeneric : MonoBehaviour {

	public float health = 2;
	public float healthRecoverTime;
	public bool dead = false;
	[HideInInspector] public float startHealth;
	
	void Start () {
		 startHealth = health;
	}
	virtual public void TakeDamage(float damage)
    {	
			if(health > 0)
						health -= damage;
    }
}
