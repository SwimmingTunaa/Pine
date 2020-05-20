using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puncher : MonoBehaviour
{
    public float punchSpeed;
    public GameObject spring;
    public GameObject glove;
    public float maxDistance;
    public GameObject killEffect;
    public AudioClip killSoundEffect;

    private SpriteRenderer _springBounds;
    private bool _activateSpring;
    private GameObject _targetGameObject;
    private Vector3 _startScale;
    private float _xScale;
    private float _springXBound;
    private bool _retract;
    private Vector3 _springReverseScale;
    private Animator _anim;
    void Start()
    {
        _springBounds = spring.GetComponent<SpriteRenderer>();
        _xScale = spring.transform.localScale.x;
        _startScale = spring.transform.localScale;
        _anim = GetComponentInParent<Animator>();
    }
   void OnTriggerStay2D(Collider2D other)
   {
        _activateSpring = true;
        _targetGameObject = other.gameObject;
   }

    void Update()
    {
        //make the glove stay at the end of the spring
        _springXBound = spring.transform.localPosition.x + _springBounds.sprite.bounds.size.x * spring.transform.localScale.x;
        Vector3 newGlowPos = new Vector3(_springXBound,spring.transform.localPosition.x,spring.transform.localPosition.x);
        glove.transform.localPosition =  newGlowPos;

        if(_activateSpring) 
            StretchSpring(_targetGameObject);
    }

   void StretchSpring(GameObject target)
   {
        //stop the spring from extending once the destination is reached
       if(!_retract && glove.transform.position.x >= target.transform.transform.position.x)
       {
            _retract = true;
            Instantiate(killEffect, target.transform.position, killEffect.transform.rotation);
            Vibration.Vibrate(333);
            CameraShake.ShakeCamera(0.1f);
            if(target.GetComponent<ObjectID>() != null)
                target.GetComponent<ObjectID>().Disable();
            else
                target.SetActive(false);
            _anim.GetComponentInParent<AudioSource>().PlayOneShot(killSoundEffect);
       }
       //retract the spring
        if(_startScale != _springReverseScale && _retract)
        {
            _springReverseScale =  new Vector3(_xScale -= Time.deltaTime * (punchSpeed * 1.3f),spring.transform.localScale.y,spring.transform.localScale.z);
            if(spring.transform.localScale.x >= _startScale.x)
                spring.transform.localScale = _springReverseScale;
                else
                {
                    //re initiate everything
                    _retract = false;
                    _activateSpring = false;
                    spring.transform.localScale = _startScale;
                    _anim.enabled = true;
                }
            return;
        }
       else if (!_retract)
       {
           //make the glove look at the target
            this.gameObject.transform.right = new Vector3(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y, 0);
            //make the spring extend
            Vector3 springScale =  new Vector3(_xScale += Time.deltaTime * punchSpeed,spring.transform.localScale.y,spring.transform.localScale.z);
            spring.transform.localScale = springScale;
            //stop animator from playing animation
            _anim.enabled = false;
       }
   }
}
