using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerRevive : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerPrefs.SetInt("canRevive", 1);       
    }
}
