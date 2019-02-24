using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchUtility : MonoBehaviour
{
    static public Enums.TouchState state = Enums.TouchState.none;
    static public bool touched = false;

    private float _touchTime = 0;

    // Update is called once per frame
    void Update()
    {
        state = Enums.TouchState.none;

        if (Input.touchCount > 0)
        {
            touched = true;
            _touchTime += Time.deltaTime;

            Touch touch = Input.GetTouch(0);

            if (touch.position.x > Screen.width / 2)
            {
                state = Enums.TouchState.pressedRight;
            } else
            {
                state = Enums.TouchState.pressedLeft;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                if (_touchTime <= Constants.MAX_TAP_TIME)
                {
                    state = Enums.TouchState.tapped;
                }
                _touchTime = 0;
            }
        }
    }
}
