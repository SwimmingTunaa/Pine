using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoController : MonoBehaviour
{

    public static bool chasePlayer;

    [HideInInspector] public float startSpeed;
    public float speed;
    public float jumpStrength;
    public LayerMask whatCanTakeDamage;
    public LayerMask whatIsGround;
    public bool immobile = false;
    public GameObject SlashEffect;
    public Transform effectSpawnPoint;
  
    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;
    [Range(0, .3f)] private float m_MovementSmoothing = .05f;	
    private Animator _anim;
    [HideInInspector] public float _horiMove;
    private RaycastHit2D _hit;
    private float _halfHeight;
    private float _halfWidth;
    Camera _cam;
    private bool _jumping;

    void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
        startSpeed = speed;        
    }

    void Start()
    {
        _cam = Camera.main;
        _halfHeight = _cam.orthographicSize;
        _halfWidth  = _cam.aspect * _halfHeight; 
    }

    void FixedUpdate()
    {
      
            if(!chasePlayer)
                Move();
            else if(chasePlayer)
                ChasePlayer();
                //Jump();
                MoveUpCollider();

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(GetComponent<Collider2D>().IsTouchingLayers(whatCanTakeDamage))
        {
            _anim.SetTrigger("Attack");
        }
    }

    void Move()
    {
        float move = speed * Time.deltaTime;
        Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
        // And then smoothing it out and applying it to the character
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
    }

    void MoveUpCollider()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up, transform.right, 1f, whatIsGround);
        if(hit)
        {
//            print("Move up collider");
            Vector3 newYPos = new Vector3 (0, (hit.collider.bounds.extents.y + 1f), 0); 
            transform.position = transform.position + newYPos;
        }
    }

    void Jump()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up, transform.right, 6f, whatIsGround);
        if(hit.normal.x <= -1 && !_jumping)
        {
            m_Rigidbody2D.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
            _jumping = true;
        }else
            if(hit.normal.x == -1 && m_Rigidbody2D.velocity.x <= 0)
            {
                m_Rigidbody2D.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
                _jumping = true;
            }
            
        if(m_Rigidbody2D.velocity.y == 0) _jumping = false;
        else if(m_Rigidbody2D.velocity.y < 0) m_Rigidbody2D.gravityScale = 2;
    }

    public void SpawnChaser(float xOffset)
    {
        gameObject.SetActive(true);

        _cam = Camera.main;
        _halfHeight = _cam.orthographicSize;
        _halfWidth  = _cam.aspect * _halfHeight; 
        _hit = Physics2D.Raycast(new Vector2(_cam.transform.position.x - _halfWidth, _cam.transform.position.y), -transform.up, 5, whatIsGround);
        //Debug.Log(_hit.collider != null ? _hit.collider.name + " " + _hit.point : "Null");
        if(_hit.collider != null)
        {
            Vector3 newPos = new Vector3 (_cam.transform.position.x - _halfWidth + xOffset, _hit.point.y, transform.position.z);
            transform.position = newPos;
        }
    }
    
    public void ChasePlayer()
    {
        /*_cam = Camera.main;
        _halfHeight = _cam.orthographicSize;
        _halfWidth  = _cam.aspect * _halfHeight; 
        var previousPos = transform.position;
        var newPos = new Vector2(_cam.transform.position.x - _halfWidth + 1 , transform.position.y);
        transform.position = newPos;*/

        speed = CharacterManager.activeCharacter.GetComponent<PlayerController>().speed;
        Move();
    }

    private void setAnimations()
    {
        _anim.SetFloat("HoriMove", Mathf.Abs(m_Rigidbody2D.velocity.x));
    }
}
