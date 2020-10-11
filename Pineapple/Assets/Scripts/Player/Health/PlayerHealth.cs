using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHealth : HealthGeneric 
{

	public GameObject deathEffectHairCut;
	public bool invincible;
	public Vector2 impulseForce;
	public TakeScreenShot takeScreenShot;
	[Header("Revive")]
	public GameObject reviveEffect;
	[Header("Shield")]
	public GameObject hitEffect;
	public AudioClip shieldHitSound;
	public bool shieldActive;
	public List<GameObject> healthBars = new List<GameObject>();

	private PlayerController playerController;
	public StartChildPosition _deathEffectStartPos;
	void Awake()
	{
		playerController = GetComponent<PlayerController>();
	}

	void Start()
	{
		_deathEffectStartPos = CharacterManager.activeVisual.deathEffect.GetComponent<StartChildPosition>();
		_deathEffectStartPos.localStartPos = _deathEffectStartPos.transform.localPosition;
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
		_deathEffectStartPos.gameObject.SetActive(true);
		_deathEffectStartPos.transform.parent = null;

		if(!CharacterManager.activeCharacter.GetComponent<CharacterController2D>().isFlying)
		{	
			foreach(Rigidbody2D r in _deathEffectStartPos.GetComponentsInChildren<Rigidbody2D>())
			{
				r.AddForce(new Vector2(impulseForce.x + Random.Range(-2,2), impulseForce.y + Random.Range(-2,2)), ForceMode2D.Impulse);
			}
		}

		//player is dead
		dead = true;
		MixLevels.Instance.FadeBGMtoGOM(true, true);
		gameObject.SetActive(false);
	}

	public IEnumerator Revive(float reviveXPos)
	{
		//player the revive animation
        _deathEffectStartPos.ReturnToDefault(reviveXPos);
        //wait till sliced parts return to default pos and Gameover Menu animation
        yield return new WaitForSecondsRT(1f);
        //play the revive effect
        reviveEffect.SetActive(true);
		//unparent it because the Parent is inactive
		reviveEffect.transform.parent = null;
		reviveEffect.transform.position = _deathEffectStartPos.transform.position;
        //wait for effect to fade
        yield return new WaitForSecondsRT(1.5f);
		//revive the player
		dead = false;

		_deathEffectStartPos.transform.parent = transform;
		_deathEffectStartPos.transform.localPosition = _deathEffectStartPos.localStartPos;
		_deathEffectStartPos.gameObject.SetActive(false);
		
		playerController.pausePlayer = false;
		playerController._anim.SetTrigger("Happy");
		MixLevels.Instance.FadeBGMtoGOM(true, false);
		gameObject.SetActive(true);
		transform.position = _deathEffectStartPos.transform.position;
		health = 1;
	}

	public Transform FindFurthestBodyPart()
	{
		if(_deathEffectStartPos.gameObject)
		{
			Transform g = null;
			foreach(GameObject child in _deathEffectStartPos.childObjects)
			{
				if(g == null)
					g = child.transform;
				if(child.transform.position.x > g.transform.position.x)
					g = child.transform;
			}
			return g;
		}
		else
			return CharacterManager.activeCharacter.transform;
	}

	public bool BodyPartsStopMoving()
	{
		List<bool> allStopped = new List<bool>();
		foreach(GameObject r in _deathEffectStartPos.childObjects)
			{
				allStopped.Add(r.GetComponent<Rigidbody2D>().velocity == Vector2.zero);
			}
		return !allStopped.Contains(false) ? true : false;
	}
}
