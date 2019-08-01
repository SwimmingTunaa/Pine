using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomAnimation : MonoBehaviour
{   
    public string stateName;
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        
        anim.Play(stateName, -1, Random.Range(0.0f,1.0f));
    }

    void OnEnable()
    {
        anim.Play(stateName, -1, Random.Range(0.0f,1.0f));
    }
}
