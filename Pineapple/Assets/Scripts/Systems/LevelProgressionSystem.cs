using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressionSystem : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("First Time Starting Game")]
    public ObstaclePool startingPool;

    [Header("Normal Game Mode")]
    public float speedIncreaseAmount;
    public float speedCheckpointDistance;
    public MasterSpawner masterSpawner;
    private float _currentCheckpoint;
    private int _difficultyLvl = 1;
    private int startCounter = 0;

    void Awake()
    {
        _currentCheckpoint = PlayerPrefs.GetInt("FirstTimeStart")<1 ? 40f: speedCheckpointDistance;
    }

    void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("FirstTimeStart"));
    }

    // Update is called once per frame
    void Update()
    {
            Debug.Log(_currentCheckpoint);
            if(GameManager._distanceTraveled > 50f && PlayerPrefs.GetInt("FirstTimeStart") == 1)
            {
                DifficultyIncrease();
                return;
            }
            else
                if(PlayerPrefs.GetInt("FirstTimeStart") == 0)
                {
                    FirstStart();
                } 
    }

    void DifficultyIncrease()
    {
        //increases players speed overtime
        //TODO: make objects spawn more frequently as level progress, via minDis and maxDis
        if(GameManager._distanceTraveled > _currentCheckpoint && GameManager._player.speed < GameManager._player.MaxSpeed)
        {
            _currentCheckpoint = GameManager._distanceTraveled + speedCheckpointDistance;
            GameManager._player.speed += speedIncreaseAmount;
            //Debug.Log("Speed increased. <color=red>The new speed is: </color>"  + _player.speed +  " || <color=blue>The next checkpoint is: </color>"  + _currentCheckpoint+"m");
        }
        if(GameManager._distanceTraveled >= 300 && _difficultyLvl == 1)
        {
            _difficultyLvl +=1;
            masterSpawner.obstacleSpawner.currentLevelConfig = masterSpawner.obstacleSpawner.levelConfig.leveltwo;
            StartCoroutine(masterSpawner.projectileSpawner.RandomSpawnType());
            masterSpawner.obstacleSpawner.changePoolSpawnChance(0.35f,0.20f,0.45f);
            masterSpawner.ChangeSpawnerTypeChance(0.2f, 0.45f, 0.35f);
            Debug.Log(_difficultyLvl);
        }
        else
        if(GameManager._distanceTraveled >= 800 && _difficultyLvl == 2)
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

    void FirstStart()
    {
        //spawn small jumpable obstacle
        if(GameManager._distanceTraveled >= _currentCheckpoint && startCounter < 3)
        {
            masterSpawner.enabled = false;
            _currentCheckpoint = GameManager._distanceTraveled + 40f;
            Debug.Log("Spawned");
            SpawnSpecificObjectAtFloor(startCounter);
            startCounter++;
        }
        else
            if(GameManager._distanceTraveled >= 80f && startCounter == 3)
            {
                SpawnSpecificObjectAtFloor(startCounter);
                masterSpawner.enabled = true;
                PlayerPrefs.SetInt("FirstTimeStart", 1);
                startCounter++;
            }
    }

    void SpawnSpecificObjectAtFloor(int objectIndex = 0)
    {
            ObstaclePoolConfig tempConfig = new ObstaclePoolConfig(); 
            startingPool.spawnedObjectPool[objectIndex].SetActive(true);
            tempConfig.bot = startingPool;
            startingPool.spawnedObjectPool[objectIndex].transform.position = masterSpawner.obstacleSpawner
                .GetFloorSpawnPoint(tempConfig, startingPool.spawnedObjectPool[objectIndex].GetComponent<Collider2D>());
            
    }


}
