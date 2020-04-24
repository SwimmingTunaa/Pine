using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLineRenderer : MonoBehaviour
{
    public void Reset(LineRenderer lr)
    {
        lr.positionCount = 0;
    }
}

