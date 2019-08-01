using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoController : MonoBehaviour
{
    [HideInInspector] public float startSpeed;
    public float speed;
    public bool immobile = false;
    public GameObject SlashEffect;
    public Transform effectSpawnPoint;
    public GameObject killEffect;

    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;
    [Range(0, .3f)] private float m_MovementSmoothing = .05f;	
    private Animator _anim;
    [HideInInspector] public float _horiMove;

    void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
        startSpeed = speed;
    }

    void Update()
    {
       // setAnimations();
    }

    void FixedUpdate()
    {
        Move();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(Kill(other.gameObject)); 
    }

    void Move()
    {
        float move = speed * Time.deltaTime;
        Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
        // And then smoothing it out and applying it to the character
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
    }

    IEnumerator Kill(GameObject target)
    {
        if(target.tag == "Player")
        {
            _anim.SetTrigger("Attack");
            yield return new WaitForSeconds(0.3f);
            SlashEffect.SetActive(false);
            SlashEffect.SetActive(true);
            target.GetComponent<HealthGeneric>().TakeDamage(1f);
        }else
        if(target.GetComponent<ObjectID>() != null)
        {
            if(target.GetComponent<ObjectID>().objectType == ObjType.Obstacle)
            {
                target.GetComponent<ObjectID>().Disable();
                Instantiate(killEffect, target.transform.position, killEffect.transform.rotation);
                _anim.SetTrigger("Attack");
                yield return new WaitForSeconds(0.2f);
                SlashEffect.SetActive(false);
                SlashEffect.SetActive(true);
            } 
        }
    }

    private void setAnimations()
    {
        _anim.SetFloat("HoriMove", Mathf.Abs(m_Rigidbody2D.velocity.x));
    }
}
