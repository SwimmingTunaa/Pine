using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileArc : MonoBehaviour
{   
    public float speed = 1f;
    [MinMaxSlider(-12f ,-3f)] public Vector2 xMinMax;
    [MinMaxSlider(3.0f ,15.0f)] public Vector2 yMinMax;
    bool move = false;

    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Awake()
    {
         _rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        _rb.velocity = Vector2.zero;
        move = true;
    }

    void FixedUpdate()
    {
        if(!move) return;
        Move();
    }
    
    void Move()
    {
        _rb.velocity = -transform.right * speed;
    }
}
