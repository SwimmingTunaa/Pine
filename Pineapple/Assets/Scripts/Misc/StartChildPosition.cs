using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartChildPosition : MonoBehaviour
    {
    public bool returnToDefault;
    public float transitionSpeed;
    public List<GameObject> childObjects = new List<GameObject>();
    public Vector2 localStartPos;

    private List<Vector2> startPos = new List<Vector2>();
    private List<Quaternion> startRot = new List<Quaternion>();
    float t;

    void Start()
    {
        foreach(GameObject g in childObjects)
        {
            startPos.Add(g.transform.localPosition);
            startRot.Add(g.transform.localRotation);
        }
    }

    void OnEnable()
    {
        t = 0;
        foreach(GameObject g in childObjects)
        {
            g.GetComponent<Rigidbody2D>().isKinematic = false;
            g.GetComponent<Collider2D>().enabled = true;
        }
    }

    void OnDisable()
    {
        returnToDefault = false;
    }

    void Update()
    {
        if(returnToDefault)
            MoveObjects(transitionSpeed);
    }

    public void MoveObjects(float speed)
    {
        //time it takes to move back
        t += Time.unscaledDeltaTime/speed%1;
        for (int i = 0; i < childObjects.Count; i++)
        {
            childObjects[i].transform.localPosition = Vector2.Lerp(childObjects[i].transform.localPosition, startPos[i], t);
            childObjects[i].transform.localRotation = Quaternion.Lerp(childObjects[i].transform.localRotation, startRot[i], t);
        }
    }

    public void ReturnToDefault(float extraDistance)
    {
        returnToDefault = true;
        int i = 0;
        foreach(GameObject g in childObjects)
        {
            g.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            g.GetComponent<Rigidbody2D>().isKinematic = true;
            g.GetComponent<Collider2D>().enabled = false;
            i++;
        }
    }
}
