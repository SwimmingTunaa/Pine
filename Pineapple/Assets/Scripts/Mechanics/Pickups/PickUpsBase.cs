using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpsBase : MonoBehaviour
{
    [Header("Base")]
    public int triggerAmount = 1;
    public bool debuff;
    public PickUpObject item;
    private SpriteRenderer _visual;
    private float _timer;
    private GameObject deathEffect;
    [HideInInspector] public bool _timerActive;

    void Awake()
    {
        item.Init();
        
    }
    void Start()
    {
        _visual = GetComponentInChildren<SpriteRenderer>();
    }
    void OnEnable()
    {
        triggerAmount = 1;
        if(_visual)
            _visual.enabled = true;
    }

    public virtual void DoAction(GameObject player)
    {
        triggerAmount--;
        if(!debuff) RewardSpawner.instance.itemCurrentlyActive = true;
        _timerActive = true;
        //Only plays these if they are not null
        if(item.pickUpSound)
            GetComponent<AudioSource>().PlayOneShot(item.pickUpSound);
        if(!deathEffect)
        {
            if(item.deathFX)
                deathEffect = Instantiate(item.deathFX, transform.position, item.deathFX.transform.rotation);
        }
        else
        {
            deathEffect.SetActive(true);
            deathEffect.transform.position = transform.position;
        }
        //_visual.enabled = false;
    }

    public virtual bool Timer(float interval)
    {
        _timer += Time.deltaTime;
        if(_timer > interval)
        {
            _timer = 0f;
            _timerActive = false;
            return true;
        }
        return false;
    }
    
    public virtual void DisablePickUp()
    {
        if(GetComponent<ObjectID>() != null)
            GetComponent<ObjectID>().Disable();
        _timerActive = false;
        triggerAmount = 1;
        //allow items to be spawned again
        if(!debuff) RewardSpawner.instance.itemCurrentlyActive = false;
    }

    public virtual void Update()
    {
        if(_timerActive && item.effectDuration > 0 && Timer(item.Instance.effectDuration))
            DisablePickUp();
    }
}
