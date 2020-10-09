using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cameraFollowTarget;
    public static bool jumpPressed;
    public bool pausePlayer;
    [Header("Running inputs")]
    [HideInInspector] public float startSpeed;
    public float speed;
    public float maxSpeed; 
    public bool jumpable = true;

    [Header("Flying Inputs")]
    public float flySpeed;
    private bool fly;

    [Header("Hair")]
    public GameObject slicedHair;
    public GameObject hairMask;
    public AudioClip hairSlicedAudio;

    [Header("Hover Board")]
    public GameObject hoverBoard;
    public ParticleSystem flame, flameTwo;
    public AudioClip hoverBoardSoundStart;
    public AudioClip hoverBoardSoundEnd;
    public AudioClip hoverBoardSoundLoop;
    private AudioSource boardAudioSource;

    public Animator _anim;
    [HideInInspector] public float _horiMove;
    private CharacterController2D _characterController;
    private Rigidbody2D _rigidBody;

    void Awake()
    {
        _characterController = GetComponent<CharacterController2D>();
        
        _rigidBody = GetComponent<Rigidbody2D>();
        startSpeed = speed;
    }
    
    void Start()
    {
        _anim = CharacterManager.activeVisual.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if(!pausePlayer)
        {
            setAnimations();
            _characterController.Move(speed * Time.deltaTime);

            if(fly) _characterController.Fly(flySpeed);
            //_jump = false;
        }
    }
    public void OnPointerPressed(bool triggered)
    {
        //triggered by press jump button event
        if(triggered && !pausePlayer && !_characterController.isFlying)
        {
            setJump(1f); 
        }
        if(_characterController.isFlying)
        {
            if(!boardAudioSource)
                boardAudioSource = hoverBoard.GetComponent<AudioSource>();
            fly = triggered;     
            var emmision = flame.emission;      
            if(triggered)
            {
                boardAudioSource.PlayOneShot(hoverBoardSoundStart);
                boardAudioSource.clip = hoverBoardSoundLoop;
                boardAudioSource.Play();
                flame.Play();
                flameTwo.Play();
                emmision = flame.emission;
                emmision.enabled = true;
                emmision = flameTwo.emission;
                emmision.enabled = true;
            } 
            else
            {
                emmision = flame.emission;
                emmision.enabled = false;
                emmision = flameTwo.emission;
                emmision.enabled = false;
                boardAudioSource.clip = null;
                boardAudioSource.PlayOneShot(hoverBoardSoundEnd);
            } 
        }
        jumpPressed = triggered;
    }

    public void setJump(float multiplier)
    {
        if (jumpable)
        {
            //_jump = true;
            _characterController.Jump(true, multiplier);
            _anim.SetBool(!_characterController.canDoubleJump ? "DoubleJump" : "Jump", true);
        }
    }

    private void setAnimations()
    {
        _anim.SetFloat("HoriMove", Mathf.Abs(_horiMove));
        _anim.SetFloat("yVelocity", _rigidBody.velocity.y);
        _anim.SetBool("Grounded", _characterController.m_Grounded);
        _anim.SetFloat("Speed", speed);
        if(_rigidBody.velocity.y < -2f)
        {
            _anim.SetBool("DoubleJump", false);
        }
    }

    public void DestroyHair()
    {
        if(!hairMask.activeInHierarchy)
        {
            _anim.SetTrigger("Sad");
            hairMask.SetActive(true);
            DialogueSequence dialogue = GetComponent<DialogueSequence>();
            dialogue.dialogues[0].text = "MY HAIR!";
            dialogue.dialogues[0].diallogueInterval = 2;
            dialogue.StartDialogue(gameObject);
            GetComponent<AudioSource>().PlayOneShot(hairSlicedAudio);
            StatsManager.Instance.AddToAStat(1,"HaircutsTaken");
        }
    }

    public IEnumerator EquipHoverBoard()
    {
        _rigidBody.velocity = Vector2.zero;
        yield return new WaitForSeconds(.2f);
        _characterController.isFlying = true;
        Time.timeScale = 0;
        _anim.SetBool("Fierce", true);
        GameManager.Instance.vfxVirtualCamera.gameObject.SetActive(true);
        hoverBoard.SetActive(true);
        yield return new WaitForSecondsRT(0.7f);
        _anim.SetBool("Fly", true);
        _anim.SetBool("Fierce", false);
        GameManager.Instance.vfxVirtualCamera.gameObject.SetActive(false);
        yield return new WaitForSecondsRT(.8f);
        Time.timeScale = 1;
        _characterController.AddForce(Vector2.up * 1000f);
    }

    public void UnequipHoverBoard()
    {
        _rigidBody.velocity = Vector2.zero;
        _anim.SetBool("Fly", false);
        hoverBoard.SetActive(false);
    }
}
