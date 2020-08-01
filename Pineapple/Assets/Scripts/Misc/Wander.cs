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

    // Start is called before the first frame update
    void Start()
    {
        targetPos = NewPosition();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position == targetPos)
            targetPos = NewPosition();
            else if (Timer(wanderTime))
                targetPos = NewPosition();
        Move();
    }

    Vector3 NewPosition()
    { 
        Vector3 newPos = new Vector3(transform.position.x + Random.Range(-maxDistance + 2, maxDistance), transform.position.y, 0f);
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
