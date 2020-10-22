using UnityEngine;
using System.Collections;
 
// Makes objects float up & down while gently spinning.
public class Floater : MonoBehaviour {
    // User Inputs
    //public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;
 
    // Position Storage Variables
    Vector3 posOffset = new Vector3 ();
    float startY;
 
    // Use this for initialization
    void Start () {
        // Store the starting position & rotation of the object
        startY = transform.localPosition.y;
    }
     
    // Update is called once per frame
    void Update () {
        // Spin object around Y-Axis
        //transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
 
        // Float up/down with a Sin()
        posOffset = new Vector3(transform.localPosition.x, startY);
        posOffset.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;
 
        transform.localPosition = posOffset;
    }
}