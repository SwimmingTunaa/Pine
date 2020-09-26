using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    public float speed;
    public float maxDistance = 2;
    public float wanderTime = 2;

    private float _timer;
    private Vector3 targetPos;

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
    void FixedUpdate()
    {
        if (transform.position == targetPos)
            targetPos = NewPosition(transform.position.x + Random.Range(-maxDistance + 2, maxDistance));
            else if (Timer(wanderTime))
                targetPos = NewPosition(transform.position.x + Random.Range(-maxDistance + 2, maxDistance));
        Move();
    }

    public void OnTriggerExit2D(Collider2D other)
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
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed); 
    }

    bool Timer(float wanderTime)
    {
       _timer += Time.deltaTime;
        if(_timer > wanderTime)
        {
            _timer = 0f;
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
         _listener.OnTriggerExit2D(other);
     }
 }
