using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public StatsManager stats;
    [Header("Game UI")]
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI stickersText; 

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
    public TextMeshProUGUI gameOverFinalScore;
    public TextMeshProUGUI gameOverTotalScore;

    private float _distanceFromChaser;
    public static PlayerController _player;
    private Rigidbody2D _playerRb;
    private Vector2 _playerCachedPos;
    private float _timer;
    private bool _gameover;

    void Awake()
    {
        //reset statics
        Statics.DistanceTraveled = 0;
        Statics.currentDifficultyLevel = 0;
        Statics.paused = true;
        ////////////////////////////
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _playerCachedPos = _player.transform.position;
        _playerRb = _player.GetComponent<Rigidbody2D>();
        Time.timeScale = 1;
        //PlayerPrefs.DeleteAll();
    }

    void FixedUpdate()
    {
        Statics.DistanceTraveled = Mathf.RoundToInt(Vector2.Distance(_playerCachedPos, _player.transform.position));
        distanceText.text = Statics.DistanceTraveled .ToString()+"m";
        stickersText.text = stats.stickerCollected.ToString();
        if(!Statics.paused)
        {
            SpawnChaser();
            StartCoroutine(Gameover());
        }
    }

    public static void pauseGame(bool enable)
    {
        //Time.timeScale = 0; // TODO: Not sure if this is correct. Something about timescale pausing all scripts and everything?
        float tempCurrentSpeed = _player.speed;
        DisablePlayerInput(enable);
        Statics.paused = enable;
    }

    public static void DisablePlayerInput(bool enabled)
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
            gameoverStickerText.text = stats.stickerCollected.ToString() + " x 10";
            //the total amount of stickers the player has
            stats.AddStickersToTotalOwnedAmount();
            totalStickers.text = PlayerPrefs.GetInt("TotalStickers").ToString();
            gameOverFinalScore.text = stats.currentScore.ToString();
            gameOverTotalScore.text = (stats.currentScore + (stats.stickerCollected * 10) + Statics.DistanceTraveled).ToString();
            //update stats part
            stats.AddStickersToTotalEverCollected();
            stats.UpdateFurthestDistanceTravelled();
        }
    }

    void SpawnChaser()
    {
        //check to see if player speed is below a threshold and wait x seconds then turn on the warning bubble
        if(_playerRb.velocity.x < velocityThreshold && !tomatoWarningBubble.activeInHierarchy)
        {        
            if(Statics.DistanceTraveled  > 20f && Timer(1f))
            {
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
        //check if the pressed play again
        Statics.playerRestartedGame = true;
    }
}
