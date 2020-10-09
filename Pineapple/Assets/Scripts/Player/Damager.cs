using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public float DamageAmount = 1;
    public LayerMask layerMask;
    protected Collider2D myCollider;

    public virtual void Awake()
    {
        myCollider = gameObject.GetComponent<Collider2D>();
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(myCollider.IsTouchingLayers(layerMask))
        {
            if(other.GetComponent<HealthGeneric>())
                other.GetComponent<HealthGeneric>().TakeDamage(DamageAmount);
        }
    }
}
