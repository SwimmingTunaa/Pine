using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    public bool moveOneDirection;
    public float xPosToMove;
    public float speed;
    public float maxDistance = 2;
    public float wanderTime = 2;

    private float _timer;
    private Vector3 targetPos;
    private bool _move = true;

   void Awake()
    {
        // Check if Colider is in another GameObject
        Collider2D collider = GetComponentInChildren<Collider2D>();
        if (collider.gameObject != gameObject)
        {
            ColliderBridge cb = collider.gameObject.AddComponent<ColliderBridge>();
            cb.Initialize(this);
        }
    }
   
    // Start is called before the first frame update
    void Start()
    {
        targetPos = NewPosition(transform.position.x + Random.Range(-maxDistance + 2, maxDistance));
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (transform.position == targetPos)
        {
            if(!moveOneDirection)
                targetPos = NewPosition(transform.position.x + Random.Range(-maxDistance + 2, maxDistance));
            else if (moveOneDirection)
                targetPos = NewPosition(transform.position.x + xPosToMove);
        }
            else if (Timer(wanderTime))
            {
                if(!moveOneDirection)
                    targetPos = NewPosition(transform.position.x + Random.Range(-maxDistance + 2, maxDistance));
                else  if (moveOneDirection) 
                    targetPos = NewPosition(transform.position.x + xPosToMove);
            }
    }

    public void ExitCheck(Collider2D other)
    {
        if(other.gameObject.layer == 12)//ground check
        {   
            if(targetPos.x - transform.position.x > 0)
            {
                NewPosition(transform.position.x -3);
            }
            else
                NewPosition(transform.position.x +3);
        }
    }

    Vector3 NewPosition(float xVal)
    { 
        Vector3 newPos = new Vector3(xVal, transform.position.y, 0f);
        if (newPos.x - transform.position.x > 0 ) 
            transform.rotation = Quaternion.Euler(0,0,0);
            else
            transform.rotation = Quaternion.Euler(0,180,0);
        return newPos;
    }

    void Move()
    {
        if(_move)
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
    }
    
    public void ToggleMove(bool toggle)
    {
        _move = toggle;
    }

    bool Timer(float wanderTime)
    {
       _timer += Time.deltaTime;
        if(_timer > wanderTime)
        {
            //_timer = 0f;
            return true;
        }
        return false;
    }
}

 public class ColliderBridge : MonoBehaviour
 {
     Wander _listener;
     public void Initialize(Wander l)
     {
         _listener = l;
     }
     void OnTriggerExit2D(Collider2D other)
     {
         _listener.ExitCheck(other);
     }
 }
