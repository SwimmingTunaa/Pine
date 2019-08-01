using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjectState : MonoBehaviour
{
    public GameObject[] objsToReset;
    
    private Vector3[] _startPos;
    private Quaternion[] _startRot;

    
    void Awake()
    {
        _startPos = new Vector3[objsToReset.Length];
        _startRot = new Quaternion[objsToReset.Length];

        for(int i = 0; i < objsToReset.Length; i ++)
        {
            _startPos[i] = objsToReset[i].transform.localPosition;
            _startRot[i] = objsToReset[i].transform.localRotation;
        }
    }

    void OnEnable()
    {
        for(int i = 0; i < objsToReset.Length; i++)
        {
           // Debug.Log(objsToReset[i].transform.position + " = " +  _startPos[i]);
            objsToReset[i].transform.localPosition = _startPos[i];
            objsToReset[i].transform.localRotation = _startRot[i];
        }
    }
}
