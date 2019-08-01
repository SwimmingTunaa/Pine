using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileArc : MonoBehaviour
{   
    
    [MinMaxSlider(-12f ,-3f)] public Vector2 xMinMax;
    [MinMaxSlider(3.0f ,15.0f)] public Vector2 yMinMax;

   
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Awake()
    {
         _rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        Move();
    }
    
    void Move()
    {
        _rb.velocity = Vector2.zero;
        _rb.AddForce(new Vector2(Random.Range(xMinMax.x, xMinMax.y), Random.Range(yMinMax.x, yMinMax.y)), ForceMode2D.Impulse);
        _rb.AddTorque(100f);
    }
}
