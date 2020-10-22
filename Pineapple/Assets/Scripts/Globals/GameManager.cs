using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Analytics;
using UnityEngine.UDP;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public const float PLAYERSTARTSPEED = 60;
    public const float TOMATOSTARTSPEED = 55;
    public StatsManager stats;
    public Animator screenFader;

    [Header("Game UI")]
    public FloatVariable distanceVariable;
    public IntVariable stickersCollectedVar;
    public SpeechBubble speachBubble;

    [Header("Chaser")]
    public TomatoController chaser;
    public float warningDistance;
    public float velocityThreshold = 5f; // the threshold for when the tomato spawns;
    public GameObject tomatoWarningBubble;

    [Header("Cameras")]
    public FollowPlayer cameraFollower;
    public CinemachineVirtualCamera followVirtualCamera;
    public CinemachineVirtualCamera stopVirutalCameara;
    public CinemachineVirtualCamera chaseVirtualCamera;
    public CinemachineVirtualCamera vfxVirtualCamera;
    public CinemachineVirtualCamera boostCamera;

    [Header("Gameover")]
    public GameObject gameoverUIBody;
    
    [Header("Retry")]
    public Button playButton;
    public MainMenu startMenu;
    public Animator loadScreen;

    private Camera _camera;
    private float _halfHeight;
    private float _halfWidth;

    private float _distanceFromChaser;
    public static PlayerController _player;
    private Rigidbody2D _playerRb;
    private Vector2 _playerCachedPos;
    private GameObject _trackedPosition;
    private float _timer;
    private PlayerHealth _playerHealth;
    private bool _gameover;
    private static Animator _playerAnim;

    void Awake()
    {
        if (Instance != null) 
                Destroy(gameObject);
            else
                Instance = this;
        //reset statics
        ResetStatics();
        _camera = Camera.main;
        
    // Instantiate the listener
    IInitListener listener = new InitListener();
    StoreService.Initialize(listener);
    }

    IEnumerator ScreenFade()
    {
        screenFader.Play("CrossFadeEnd");
        yield return new WaitForSeconds(screenFader.GetCurrentAnimatorClipInfo(0).Length);
    }

    void ResetStatics()
    {
        Statics.DistanceTraveled = 0;
        Statics.currentDifficultyLevel = 0;
        Statics.paused = true;
        Time.timeScale = 1;
        SelfDamage.floorIsOpened = false;
        SelfDamage.health = 2;
    }

    public void InitialisePlayer()
    {
        //Debug.Log("GM: " + CharacterManager.activeCharacter.name);
        _player = CharacterManager.activeCharacter.GetComponent<PlayerController>();
        _player.speed = PLAYERSTARTSPEED;
        chaser.speed = TOMATOSTARTSPEED;
        _playerHealth = CharacterManager.activeCharacter.GetComponent<PlayerHealth>();
        _playerAnim = CharacterManager.activeCharacter.GetComponentInChildren<Animator>();
        _trackedPosition = _player.gameObject;
        _playerCachedPos = _player.transform.position;
        _playerRb = _player.GetComponent<Rigidbody2D>();
        //followVirtualCamera.Follow = _player.cameraFollowTarget;
        //stopVirutalCameara.Follow = _player.cameraFollowTarget;
        //chaseVirtualCamera.Follow = _player.cameraFollowTarget;
    }

    void Update()
    {
        Statics.DistanceTraveled = Mathf.RoundToInt(Vector2.Distance(_playerCachedPos, _trackedPosition.transform.position));
        distanceVariable.RuntimeValue = Statics.DistanceTraveled;
        //distanceText.text = Statics.DistanceTraveled .ToString()+"m";
        stickersCollectedVar.RuntimeValue = stats.stickerCollectedThisRound;
        if(!Statics.paused)
        {
            if(!_playerHealth.dead) 
            {
                SpawnChaser();
            }
            else if(_playerHealth.dead)
            {
                if(!_gameover)
                {
                    //change the camera to follow the furtherst part if not flying
                    Vector3 newCamPos = new Vector3 (_playerHealth.FindFurthestBodyPart().position.x, 
                    followVirtualCamera.Follow.transform.position.y, followVirtualCamera.Follow.transform.position.z);

                    followVirtualCamera.Follow.position = newCamPos;
                    //change what the tracked object for distance is
                    _trackedPosition = followVirtualCamera.Follow.gameObject;
                    //this gets called in the update loop
                    if(_playerHealth.BodyPartsStopMoving() || Timer(5f))
                    {
                        ShowGameOverScreen();
                    }else
                    if(CharacterManager.activeCharacter.GetComponent<CharacterController2D>().isFlying)
                    {
                        //this gets called once
                        _gameover = true;
                        StartCoroutine(Gameover());  
                    }
                }
            }
        }
    }

    public static void DisablePlayerInput(bool enabled)
    {
        _player.enabled = !enabled;
        _player.jumpable = !enabled;
        _playerAnim.SetBool("Sit", enabled);
    }

    public void ToggleVFXCamera(bool toggle)
    {
        vfxVirtualCamera.Follow = CharacterManager.activeCharacter.transform;
        followVirtualCamera.gameObject.SetActive(!toggle);
        vfxVirtualCamera.Follow = CharacterManager.activeCharacter.transform;
        vfxVirtualCamera.gameObject.SetActive(toggle);
    }

    bool Timer(float interval)
    {
        _timer += Time.deltaTime;
        if(_timer > interval)
        {
            _timer = 0f;
            return true;
        }
        return false;
    }

    void SpawnChaser()
    {
        //check to see if player speed is below a threshold and wait x seconds then turn on the warning bubble
        if(_playerRb.velocity.x < velocityThreshold && !tomatoWarningBubble.activeInHierarchy)
        {        
            //set the tomato waring spawn pos
            _halfHeight = _camera.orthographicSize;
            _halfWidth  = _camera.aspect * _halfHeight; 
            Vector3 warningPosition = new Vector3(_camera.transform.position.x - _halfWidth, _camera.transform.position.y, transform.position.z);

            if(Statics.DistanceTraveled  > 20f && Timer(1f))
            {
                //spawn Warning
                tomatoWarningBubble.SetActive(true);
                tomatoWarningBubble.transform.position = warningPosition;
                _timer = 0;
            } 
            if(!_playerHealth.dead)
            {
                followVirtualCamera.gameObject.SetActive(false);
                stopVirutalCameara.gameObject.SetActive(true);   
                TomatoController.chasePlayer = false;
            }     
        }
        else if(tomatoWarningBubble.activeInHierarchy && !chaser.gameObject.activeInHierarchy)
        {
            //after the another x seconds has passed while warning bubble was on then turn on the tomato
            if(Timer(0.25f))
            {
                chaser.gameObject.SetActive(true);
                //spawn the tomato to a certain distnce behind the palyer
                chaser.SpawnChaser(-2f);
                chaser.GetComponent<TomatoController>().speed = _player.speed + 2f;
                _timer = 0;
            }
        }

        _distanceFromChaser = Vector2.Distance(_player.transform.position, chaser.transform.position);
        //if tomato is within x dist then turn off warning otherwise if larger than x dist AND it has been x seconds, turn off both warning and tomato
        if(_distanceFromChaser < 15f && chaser.gameObject.activeInHierarchy)
        {
            tomatoWarningBubble.SetActive(false);
        }
        else 
            if(_distanceFromChaser >= 20f && chaser.gameObject.activeInHierarchy)
            {
                if(!tomatoWarningBubble.activeInHierarchy)
                    tomatoWarningBubble.SetActive(true);
                if(Timer(1f))
                {   
                    tomatoWarningBubble.SetActive(false);
                    chaser.gameObject.SetActive(false);
                    _timer = 0;
                }
            }
        if(_playerRb.velocity.x > velocityThreshold)
        {
            stopVirutalCameara.gameObject.SetActive(false);
            followVirtualCamera.gameObject.SetActive(true);
        }       
    }

    IEnumerator Gameover()
    {
        yield return new WaitForSeconds(1f);
        ShowGameOverScreen();
    }

    void ShowGameOverScreen()
    {
        _gameover = true;
        if(PlayerPrefs.GetInt("Preservatives") == 1)
            StartPlayerRevive();
        else
        {
            PauseGame(true);
            StatsManager.Instance.getTotalScore();
            gameoverUIBody.SetActive(true);
            AdManager.instance.ShowOptInAdButton();
            Time.timeScale = 0;  
            //Track Analytics
            //track Total Stickers collcected
            Analytics.CustomEvent("Stickers Collected", new Dictionary<string, object>{
                                                        {"Stickers Collect", PlayerPrefs.GetInt("StickersCollected")},
                                                        {"Play Time", Time.timeSinceLevelLoad},
                                                                                        } );
            //Track playTime session
            //track session play count
        }
        _timer = 0;            
    }

    public IEnumerator PlayerRevive()
    {
        //clear all obstacles and rewards
        PlayerPrefs.SetInt("Preservatives",0);
        MasterSpawner.Instance.ClearAllActiveObjects();
        //Play the UI close animation
        if(gameoverUIBody.activeSelf)
            gameoverUIBody.GetComponent<Animator>().Play("End",0);
        //turn the chaser off
        chaser.gameObject.SetActive(false);
        //revive Player
        yield return (StartCoroutine(_playerHealth.Revive(_playerCachedPos.x + Statics.DistanceTraveled)));
        //Close UI
        gameoverUIBody.SetActive(false);
        //change the camera follow target to the player again
        _trackedPosition = _player.gameObject;
        //lower the difficulty level
        LevelProgressionSystem.Instance.SetDifficulty(LevelProgressionSystem.Instance.difficultyLvl - 1 > 0 ? LevelProgressionSystem.Instance.difficultyLvl - 1 : 1);
        Time.timeScale = 1;
        //unpause
        _gameover = false;
        PauseGame(false);
    }
    public void StartPlayerRevive()
    {
        StartCoroutine(PlayerRevive());
    }

    public IEnumerator LoadLevel(int levelIndex)
    {   
        loadScreen.gameObject.SetActive(true);
        loadScreen.updateMode = AnimatorUpdateMode.UnscaledTime;
        loadScreen.Play("ScreenFadeIn", 0);
        LevelManager.MoveObjectsToNewScene();
        yield return new WaitForSecondsRT(.5f);
        SceneManager.LoadScene(0);
        //check if the pressed play again
        Statics.playerRestartedGame = true;
    }
    
    public static void PauseGame(bool enable)
    {
        float tempCurrentSpeed = _player.speed;
        DisablePlayerInput(enable);
        Statics.paused = enable;
    }

    public void PauseGame()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        PauseGame(Time.timeScale == 0 ? true : false);
    }

    public void Retry(int isRetry)
    {
        PlayerPrefs.SetInt("Retry", isRetry);
        //System.GC.Collect();
        //the total amount of stickers the player has
        stats.AddStickersToTotalOwnedAmount();
        //update stats part
        stats.UpdateMostStickersEverCollected();
        stats.UpdateFurthestDistanceTravelled();  

        StartCoroutine(LoadLevel(0));
    }
}
