using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpsBase : MonoBehaviour
{
    [Header("Base")]
    public int triggerAmount = 1;
    public AudioClip pickUpSound;
    public GameObject deathFX;
    public float effectDuration;
    private float _initialDuration;
    private SpriteRenderer _visual;
    private float _timer;
    private bool _timerActive;

    void Awake()
    {
        _initialDuration = effectDuration;
        _visual = GetComponentInChildren<SpriteRenderer>();
    }

    void OnEnable()
    {
        effectDuration = _initialDuration;
        triggerAmount = 1;
        _visual.enabled = true;
    }

    public virtual void DoAction(GameObject player)
    {
        triggerAmount--;
        _timerActive = true;
        //Only plays these if they are not null
        if(pickUpSound)
            GetComponent<AudioSource>().PlayOneShot(pickUpSound);
        if(deathFX)
            Instantiate(deathFX,transform.position,transform.rotation);
        //_visual.enabled = false;
    }

    public bool Timer(float interval)
    {
        _timer += Time.deltaTime;
        if(_timer > interval)
        {
            _timer = 0f;
            return true;
        }
        return false;
    }

    void Update()
    {
        if(_timerActive && effectDuration > 0 && Timer(effectDuration))
        {
            GetComponent<ObjectID>().Disable();
            Debug.Log("Item Disabled");
        }
    }
}
