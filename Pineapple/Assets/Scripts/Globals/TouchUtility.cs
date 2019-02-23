using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchUtility : MonoBehaviour
{
    static public bool tapped = false;
    static public bool pressedRight = false;
    static public bool pressedLeft = false;
    static public bool touched = false;

    private float _touchTime = 0;

    // Update is called once per frame
    void Update()
    {
        tapped = false;

        if (Input.touchCount > 0)
        {
            touched = true;
            _touchTime += Time.deltaTime;

            Touch touch = Input.GetTouch(0);

            if (touch.position.x > Screen.width / 2)
            {
                pressedRight = true;
            } else
            {
                pressedLeft = true;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                if (_touchTime <= Constants.MAX_TAP_TIME)
                {
                    tapped = true;
                }
                _touchTime = 0;
                pressedRight = false;
                pressedLeft = false;
            }
        }
    }
}
