using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game UI")]
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI stickersText;

    [Header("Level Progression")]
    public float speedIncreaseAmount;
    public float checkpointDistance;
    public MasterSpawner masterSpawner;
    private float _currentCheckpoint;
    private int _difficultyLvl = 1;

    [Header("Chaser")]
    public GameObject chaser;
    public float warningDistance;
    public float velocityThreshold = 5f; // the threshold for when the tomato spawns;
    public CinemachineVirtualCamera followVirtualCamera, stopVirutalCameara;
    public GameObject tomatoWarningBubble;

    [Header("Gameover")]
    public TextMeshProUGUI finalScore;
    public GameObject gameoverUIBody;
    public TextMeshProUGUI totalStickers;
    public TextMeshProUGUI gameoverStickerText;

    private float _distanceFromChaser;
    [HideInInspector] public PlayerController _player;
    private Rigidbody2D _playerRb;
    private Vector2 _playerCachedPos;
    public static float _distanceTraveled;
    private float _timer;
    private bool _gameover;

    [Header("Sticker")]
    [HideInInspector] public int stickerCollected;
  


    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _playerCachedPos = _player.transform.position;
        _playerRb = _player.GetComponent<Rigidbody2D>();
        _currentCheckpoint = checkpointDistance;
        Time.timeScale = 1;
        //PlayerPrefs.DeleteAll();
    }

    void FixedUpdate()
    {
        _distanceTraveled = Mathf.RoundToInt(Vector2.Distance(_playerCachedPos, _player.transform.position));
        distanceText.text = _distanceTraveled.ToString()+"m";
        stickersText.text = stickerCollected.ToString();
        if(!Statics.paused)
        {
            SpawnChaser();
            StartCoroutine(Gameover());
        }
    }

    void Update()
    {
        if(_distanceTraveled > 50f)
            DifficultyIncrease();
    }

    public void pauseGame(bool enable)
    {
        //Time.timeScale = 0; // TODO: Not sure if this is correct. Something about timescale pausing all scripts and everything?
        float tempCurrentSpeed = _player.speed;
        DisablePlayerInput(enable);
        Statics.paused = enable;
    }

    public void DisablePlayerInput(bool enabled)
    {
        _player.enabled = !enabled;
        _player.jumpable = !enabled;
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

    IEnumerator Gameover()
    {
        if(_player.GetComponent<HealthGeneric>().dead && !_gameover)
        {
            _gameover = true;
            yield return new WaitForSeconds(2f);
            pauseGame(true);
            gameoverUIBody.SetActive(true);
            finalScore.text = distanceText.text;
            distanceText.gameObject.SetActive(false);
            //set Stickers collected in this round
            gameoverStickerText.text = stickerCollected.ToString();
            //the total amount of stickers the player has
            int allStickers = PlayerPrefs.GetInt("TotalStickers") + stickerCollected;
            PlayerPrefs.SetInt("TotalStickers", allStickers);
            totalStickers.text = PlayerPrefs.GetInt("TotalStickers").ToString();
            //update stats part
            if(stickerCollected > PlayerPrefs.GetInt("StickersCollected"))
                PlayerPrefs.SetInt("StickersCollected", stickerCollected);
            if(_distanceTraveled > PlayerPrefs.GetInt("HighScore"))
                PlayerPrefs.SetInt("HighScore", (int)_distanceTraveled);
        }
    }

    void DifficultyIncrease()
    {
        //increases players speed overtime
        //TODO: make objects spawn more frequently as level progress, via minDis and maxDis
        if(_distanceTraveled > _currentCheckpoint && _player.speed < _player.MaxSpeed)
        {
            _currentCheckpoint = _distanceTraveled + checkpointDistance;
            _player.speed += speedIncreaseAmount;
            //Debug.Log("Speed increased. <color=red>The new speed is: </color>"  + _player.speed +  " || <color=blue>The next checkpoint is: </color>"  + _currentCheckpoint+"m");
        }
        if(_distanceTraveled >= 300 && _difficultyLvl == 1)
        {
            _difficultyLvl +=1;
            masterSpawner.obstacleSpawner.currentLevelConfig = masterSpawner.obstacleSpawner.levelConfig.leveltwo;
            StartCoroutine(masterSpawner.projectileSpawner.RandomSpawnType());
            masterSpawner.obstacleSpawner.changePoolSpawnChance(0.35f,0.20f,0.45f);
            masterSpawner.ChangeSpawnerTypeChance(0.2f, 0.45f, 0.35f);
            Debug.Log(_difficultyLvl);
        }
        else
        if(_distanceTraveled >= 800 && _difficultyLvl == 2)
        {
            _difficultyLvl +=1;
            masterSpawner.minDistance = 25f;
            masterSpawner.maxDistance = 35f;
            //TODO: different spawn pool
            masterSpawner.obstacleSpawner.currentLevelConfig = masterSpawner.obstacleSpawner.levelConfig.levelthree;
            masterSpawner.obstacleSpawner.changePoolSpawnChance(0.9f,0,0.1f);
            masterSpawner.ChangeSpawnerTypeChance(0.1f, 0.7f, 0.2f);
            Debug.Log(_difficultyLvl);
           
        }
        //panel cahnge?
    }

    void SpawnChaser()
    {
        //check to see if player speed is below a threshold and wait x seconds then turn on the warning bubble
        if(_playerRb.velocity.x < velocityThreshold && !tomatoWarningBubble.activeInHierarchy)
        {        
            if(_distanceTraveled > 20f && Timer(1f))
            {
                Debug.Log("Player Stopped");
                //reset player's speed
                _player.speed  = _player.startSpeed;
                tomatoWarningBubble.SetActive(true);
                _timer = 0;
            } 
                followVirtualCamera.gameObject.SetActive(false);
                stopVirutalCameara.gameObject.SetActive(true);        
        }
        else if(tomatoWarningBubble.activeInHierarchy  && !chaser.activeInHierarchy)
        {
            //after the another x seconds has passed while warning bubble was on then turn on the tomato
            if(Timer(0.25f))
            {
                chaser.SetActive(true);
                //spawn the tomato to a certain distnce behind the palyer
                Vector2 spawnPos = new Vector2(_player.transform.position.x - 15f, chaser.transform.position.y);
                chaser.transform.position = spawnPos; 
                chaser.GetComponent<TomatoController>().speed = _player.speed + 1f;
                _timer = 0;
            }
        }

        _distanceFromChaser = Vector2.Distance(_player.transform.position, chaser.transform.position);
        //if tomato is within x dist then turn off warning otherwise if larger than x dist AND it has been x seconds, turn off both warning and tomato
        if(_distanceFromChaser < 15f && chaser.activeInHierarchy)
        {
            tomatoWarningBubble.SetActive(false);
        }
        else 
            if(_distanceFromChaser >= 20f && chaser.activeInHierarchy)
            {
                if(!tomatoWarningBubble.activeInHierarchy)
                    tomatoWarningBubble.SetActive(true);
                if(Timer(1f))
                {   
                    tomatoWarningBubble.SetActive(false);
                    chaser.SetActive(false);
                    _timer = 0;
                }
            }
        if(_playerRb.velocity.x > velocityThreshold)
        {
            stopVirutalCameara.gameObject.SetActive(false);
            followVirtualCamera.gameObject.SetActive(true);
        }       
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
}
