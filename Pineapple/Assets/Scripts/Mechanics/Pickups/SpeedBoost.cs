using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class SpeedBoost : PickUpsBase
{
    public float distanceToTravel;
    public float boostSpeed = 20;
    public float minHeightThreshold = 3;
    public LayerMask groundCheck;
    public GameObject visuals;
    public float transitionSpeed;
    public GameObject transformVFX;
    
    [Header("Audio")]
    public AudioClip cloudPopClip;
    public AudioClip rumbleClip;
    private AudioSource audioSource;

    private CinemachineVirtualCamera boostCamera;
    private Vector3 _targetPos;
    private GameObject _player;
    private bool _active;
    private bool _boost;
    private Vector2 _currentPos;
    private Animator _anim;
    private Outfits outfits;
    void Start()
    {
        boostCamera = GameManager.Instance.boostCamera;
        audioSource = GetComponent<AudioSource>();
        outfits = CharacterManager.activeVisual; 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && triggerAmount > 0)
        {
            DoAction(other.gameObject);
        }
    }

    public override void Update()
    {
        if(_active)
        {
            if(_player.transform.position.y <= _targetPos.y && !_boost)
            {
                MovePlayerTowardsPos(_targetPos, transitionSpeed);
            }
        
            if(_boost)
            {
                MovePlayerTowardsPos(_targetPos, boostSpeed);
                if(_player.transform.position.x >= _targetPos.x)
                    {
                        _boost = false;
                        _active = false;
                        Activate(_player, false);
                        _anim.SetBool("Boost", false);
                        GameManager.Instance.ToggleVFXCamera(false);
                        boostCamera.gameObject.SetActive(false);
                        CameraShake.Instance.shakeActive = false;
                        transformVFX.SetActive(true);
                        transformVFX.transform.position = _player.transform.position;
                        outfits.toggleOutfit(outfits.boostOutFit, false);
                        GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(cloudPopClip);
                        audioSource.clip = null;
                        DisablePickUp();
                    }
            }              
        }
    }

    public override void DoAction(GameObject player)
    {
        base.DoAction(player);
        visuals.SetActive(false);
        player.GetComponent<CharacterController2D>().m_Grounded = false;
        Activate(player, true);
        StartCoroutine(Boost(player));        
    }

    IEnumerator Boost(GameObject player)
    {
        //check players distance from ground and make sure its above a minimum height
        RaycastHit2D hit2D = Physics2D.Raycast(player.transform.position, -player.transform.up, 8f, groundCheck);
        _active = true;
        //move player to the minimum height
        if(Vector2.Distance(player.transform.position, hit2D.point) < minHeightThreshold)
        {
            //set players currentPos
            _currentPos = new Vector3(player.transform.position.x, hit2D.point.y + minHeightThreshold); 
            _targetPos = _currentPos; 
        }  
        //pause
        Time.timeScale = 0; 
        //TODO: make effects
        _anim = player.GetComponentInChildren<Animator>();
        _anim.SetTrigger("ItemAcquired");
        _anim.SetBool("Boost", true);
        //wait for any anims
        GameManager.Instance.ToggleVFXCamera(true);
        yield return new WaitForSecondsRT(1);
        //play the sfx
        transformVFX.SetActive(true);
        transformVFX.transform.position = player.transform.position;
        transformVFX.transform.parent = null;
        audioSource.PlayOneShot(cloudPopClip);
        //put on the outfit
        outfits.toggleOutfit(outfits.boostOutFit, true);
        //change the camera to the boost camera
        GameManager.Instance.vfxVirtualCamera.GetComponent<Animator>().SetBool("Boost",true);
        CameraShake.Instance.ShakeCamera(2, 1, 5);
        yield return new WaitForSecondsRT(1f);
        boostCamera.gameObject.SetActive(true);
        CameraShake.Instance.ShakeCamera(10, 0.2f, 5);
        //set pos to yeet to
        _targetPos =  player.transform.position + (Vector3.right * distanceToTravel);
        //unpause
        Time.timeScale = 1;
        _boost = true;
        //play flying audio loop
        audioSource.clip = rumbleClip;
        audioSource.Play();
    }

    void Activate(GameObject player, bool toggle)
    {
        if(!_player) _player = player;
        //turn off the master spawner
        MasterSpawner.Instance.gameObject.SetActive(!toggle);
        //turn off player collision
        player.GetComponent<Collider2D>().enabled = !toggle;
        //turn on kinematic
        player.GetComponent<Rigidbody2D>().isKinematic = toggle;
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        if(toggle) player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        else player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        //turn off player controller
        player.GetComponent<PlayerController>().pausePlayer = toggle;
        player.GetComponent<CharacterController2D>().disableGravityMultiplier = toggle;
    }

    void MovePlayerTowardsPos(Vector3 targetPos,float speed)
    {
        _player.transform.position = Vector3.MoveTowards(_player.transform.position, targetPos, speed * Time.unscaledDeltaTime);
    }
}
