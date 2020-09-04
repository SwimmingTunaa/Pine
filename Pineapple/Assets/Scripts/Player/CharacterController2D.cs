using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	public float m_JumpForce = 400f;	// Amount of force added when the player jumps.
	public float doubleJumpForce;					
	public AudioClip jumpSound;	
	public bool disableGravityMultiplier;
	public bool canDoubleJump;
	public bool m_Grounded;		// Whether or not the player is grounded.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private float m_fallMulitplier = 2.5f;
	[SerializeField] private float m_lowJumpMulitplier = 2.5f;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;
	
	[Header("Flying Inputs")]
    public bool isFlying;							// A position marking where to check if the player is grounded.

	private Vector2 _GroundedBoxSize = new Vector2(0.7f, 0.15f); // size of the overlap box to determine if grounded
	private Rigidbody2D m_Rigidbody2D;
	private Vector3 m_Velocity = Vector3.zero;
	private Vector3 m_FlyVelocity = Vector3.zero;
	private AudioSource _audioSource;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		_audioSource = GetComponent<AudioSource>();
	}

	private void FixedUpdate()
	{
		if(m_Grounded && !isFlying) canDoubleJump = true;
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapBoxAll(m_GroundCheck.position, _GroundedBoxSize, 0, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;;
			}
		}
		//make the player fall faster and jump higher if button is held
		if(!disableGravityMultiplier)
		{
			//if player is falling make the fall faster
			if(m_Rigidbody2D.velocity.y < 0 && canDoubleJump && !isFlying)
				m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (m_fallMulitplier - 1) * Time.fixedDeltaTime;
			//if player is jumping make them jump higher
			else if(m_Rigidbody2D.velocity.y > 0 && !PlayerController.jumpPressed || !canDoubleJump)
				m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (m_lowJumpMulitplier - 1) * Time.fixedDeltaTime;
		}
	}

	private void OnDrawGizmosSelected() 
	{
    	Gizmos.color = Color.red;
    	Gizmos.DrawWireCube(m_GroundCheck.position, _GroundedBoxSize);
 	}

	public void Move(float move)
	{
		
		//only control the player if grounded or airControl is turned on
	
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
	}

	public void Fly(float ySpeed)
	{
		Vector3 targetYVelocity = new Vector2(m_Rigidbody2D.velocity.x, (ySpeed * Time.fixedDeltaTime) * 10f);
		m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetYVelocity,ref m_FlyVelocity, m_MovementSmoothing);
	}

	public void AddForce(Vector2 direction)
	{
		m_Rigidbody2D.AddForce(direction);
	}

	public void Jump(bool jump, float multiplier = 1)
	{
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			_audioSource.PlayOneShot(jumpSound);
			_audioSource.pitch = 1f;
			m_Grounded = false;
			float newJumpForce = m_JumpForce * multiplier;
			m_Rigidbody2D.AddForce(new Vector2(0f, newJumpForce), ForceMode2D.Impulse);
		}
		else if(canDoubleJump)
		{
			m_Grounded = false;
			canDoubleJump = false;
			float newJumpForce = doubleJumpForce * multiplier;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_Rigidbody2D.velocity.y <= -8f ? newJumpForce * 2f : newJumpForce), ForceMode2D.Impulse);
			_audioSource.PlayOneShot(jumpSound);
			_audioSource.pitch = 1.2f;
		}
	}
}
