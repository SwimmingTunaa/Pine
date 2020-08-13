using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    public HealthGeneric health;
    public float spiderLifeDuration;
    public float speed;
    public float rotationSpeed;
    public LayerMask canWalkOn;

    private float _startSpeed;
    private float _animationSpeedMultiplier;
    private float _speedMultiplier;
    private Rigidbody2D _rigidbody;
    private Animator _anim;
    private Vector2 _hitNormal = Vector2.zero;
    private Vector2 _curNormal = Vector2.up;
    private RaycastHit2D _hit;
    private bool _canMove = true;
    private float highestPoint;
    private bool _jumping;
    private ObjectID _objectID;
    private float _timer;
    
    Vector2 m_Velocity = Vector3.zero;

    void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _objectID = GetComponent<ObjectID>();
        _startSpeed = speed;
    } 

    void Start()
    {
        Initialise();
    }

    void OnEnable()
    {
        Initialise();
    }

    void Initialise()
    {
        if(CharacterManager.activeCharacter)
            _speedMultiplier = CharacterManager.activeCharacter.GetComponent<PlayerController>().speed/ 60;

        speed = _startSpeed * _speedMultiplier;
        _animationSpeedMultiplier = speed / 10;
        _anim.SetFloat("RunSpeedMultiplier", _animationSpeedMultiplier);
        _timer = 0;
    }

    void LateUpdate()
    {
        if(_canMove)
        {
            Move();
            //checks in front for obstacles
            _hit = Physics2D.Raycast(transform.position + Vector3.up, transform.right, 0.5f, canWalkOn);
            Debug.DrawRay(transform.position + Vector3.up, transform.right * 1.5f, Color.magenta);
            if(_hit)
            {
                //stop movement and stops the raycasting so that it can rotate without interuptions
                _canMove = false;
                _hitNormal =  _hit.normal;
                //change rotation and continue movement;    
            }
            
            //check below for floor
            _hit = Physics2D.Raycast(transform.position, -transform.up,0.5f, canWalkOn);
            Debug.DrawRay(transform.position, -transform.up * 0.5f, Color.red);
            if(_hit)
                {
                    if(!_rigidbody.isKinematic)
                    {
                        Jump(false);
                    }
                    highestPoint = _hit.point.y;
                }
            else
            //if there is no floor then check around the corner for the next floor/obstacle
                if(!_hit && !_jumping)
                {
                    Vector2 checkBottom = transform.position + -transform.up;
                    _hit = Physics2D.Raycast(checkBottom, -transform.right, 0.5f, canWalkOn);
                    if(_hit)
                    {
                        //check for the highest hit point and if the next one is lower make the spider jump;
                        if(_hit.point.y < highestPoint)
                        {
                            Jump(true);
                            return;
                        }
                        _canMove = false;
                        _hitNormal =  _hit.normal;
                    }
                } 
        }
        else
            if(!_canMove)
                ChangeRotation(); //rotate the object        

        if(Timer(spiderLifeDuration)) gameObject.SetActive(false);
    }

    
   public bool Timer(float interval)
   {
      _timer += Time.deltaTime;
        if(_timer > interval)
        {
            _timer = 0f;
            return true;
        }
        return false;
   }

    void Move()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
    
    void ChangeRotation()
    {
        //smoothly rotate between the two normal
        _rigidbody.isKinematic = true;
        _curNormal = Vector2.Lerp(_curNormal, _hitNormal, rotationSpeed * Time.deltaTime);
        Quaternion newRot = Quaternion.FromToRotation(Vector2.up, _curNormal);
        transform.rotation = newRot;
        //check to see if rotation is complete and then set the final rotation and allow movement again
        float check = (_curNormal - _hitNormal).sqrMagnitude;
        if(check < 0.001f)
        {
            newRot = Quaternion.FromToRotation(Vector2.up, _hitNormal);
            transform.rotation = newRot;
            //transform.position = _hit.point;
            _canMove = true;
        }
    }

    void Jump(bool toggle)
    {
        _rigidbody.isKinematic = !toggle;
        _rigidbody.simulated = toggle;
        _jumping = toggle;
        _anim.SetBool("Jump", toggle);
    }
}
