using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteParralax : MonoBehaviour
{
    
    public GameObject cam;
    public float depth;
    
    private float length, startPos;
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;    
    }

    // Update is called once per frame
    void Update()
    {
        float temp = (cam.transform.position.x * (1- depth));
        float dist  = (cam.transform.position.x * depth);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if(temp > startPos + length) startPos += length;
        else if(temp < startPos - length) startPos -= length;
    }
}
