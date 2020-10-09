using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHealth : HealthGeneric {

	public GameObject deathEffectHairCut;
	public bool invincible;
	public Vector2 impulseForce;
	public TakeScreenShot takeScreenShot;
	[Header("Shield")]
	public GameObject hitEffect;
	public AudioClip shieldHitSound;
	public bool shieldActive;
	public List<GameObject> healthBars = new List<GameObject>();

	private GameObject _deathEffect;
	private PlayerController playerController;
	
	void Awake()
	{
		playerController = GetComponent<PlayerController>();
		
	}

	void Start()
	{
		_deathEffect = CharacterManager.activeVisual.deathEffect;
	}

	override public void TakeDamage(float damage)
	{
		if(!invincible)
		{
			base.TakeDamage(damage);
			if(shieldActive)
				DamageShield();
			if(health == 1)
			{
				Invulnerable(1.5f);
			}
		}
		//check if player is dead after deducting health
		if(health <= 0 && !dead) StartCoroutine(killPlayer());
	}

	public void AddHealth(float amount)
	{
		health += amount;
	}
	
	public void DamageShield()
	{
		if(hitEffect) hitEffect.SetActive(true);
		if(shieldHitSound) GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(shieldHitSound);
		for (int i = healthBars.Count - 1; i > 0; i--)
		{
			if(healthBars[i].activeInHierarchy)
			{
				healthBars[i].SetActive(false); 
				break;
			}
			if(i ==0)
				shieldActive = false;
		}
	}

	public void AddShield(float amount, List<GameObject> shieldHealthBars)
	{
		health += amount;
		healthBars = shieldHealthBars;
		shieldActive = true;
	}

	public void Invulnerable(float duration)
	{
		invincible = true;
		playerController._anim.SetBool("Invincible", true);
		Invoke("resetInvulnerablilty", duration);
	}
	void resetInvulnerablilty()
	{
		invincible = false;
		playerController._anim.SetBool("Invincible", false);
	}

	IEnumerator killPlayer()
	{
		playerController._anim.SetTrigger("Sad");
		playerController.pausePlayer = true;
		if(takeScreenShot)
			yield return (StartCoroutine(takeScreenShot.ScreenShot()));
		//the sliced body
		_deathEffect.SetActive(true);
		_deathEffect.transform.parent = null;
		if(CharacterManager.activeCharacter.GetComponent<CharacterController2D>().isFlying == false)
		{	
			foreach(Rigidbody2D r in _deathEffect.GetComponentsInChildren<Rigidbody2D>())
			{
				r.AddForce(new Vector2(impulseForce.x + Random.Range(-2,2), impulseForce.y + Random.Range(-2,2)), ForceMode2D.Impulse);
			}
		}
		//player is dead
		dead = true;
		MixLevels.Instance.FadeBGMtoGOM(true);
		gameObject.SetActive(false);
	}

	public Transform FindFurthestBodyPart()
	{
		if(_deathEffect)
		{
			Transform g = null;
			foreach(Transform child in _deathEffect.transform)
			{
				if(g == null)
					g = child;
				if(child.position.x > g.transform.position.x)
					g = child;
			}
			return g;
		}
		else
			return CharacterManager.activeCharacter.transform;
	}

	public bool BodyPartsStopMoving()
	{
		List<bool> allStopped = new List<bool>();
		foreach(Rigidbody2D r in _deathEffect.GetComponentsInChildren<Rigidbody2D>())
			allStopped.Add(r.velocity == Vector2.zero);
		return !allStopped.Contains(false) ? true : false;
	}
}
