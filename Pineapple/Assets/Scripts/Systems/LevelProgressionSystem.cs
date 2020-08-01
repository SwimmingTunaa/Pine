    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressionSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public static LevelProgressionSystem Instance;
    [Header("First Time Starting Game")]
    public ObstaclePool startingPool;
    public DialogueSequence dialogueSequence;

    [Header("Normal Game Mode")]
    public float speedIncreaseAmount;
    public float speedCheckpointDistance;
    
    public int difficultyLvl = 0;
    [SerializeField] public List<LevelInfo> levelInfos = new List<LevelInfo>();

    private int currentDifficulty;
    public int roundsCompleted;  
    private float _currentCheckpoint;
    private int startCounter = 0;
    private int _sItemForcedSpawnCounter;
    private MasterSpawner masterSpawner;

    void Awake() => Instance = this;

    void Start()
    {
        masterSpawner = MasterSpawner.Instance;
        _currentCheckpoint = PlayerPrefs.GetInt("FirstTimeStart")<1 ? 40f: speedCheckpointDistance;
        Debug.Log(_currentCheckpoint + " current checkpoint");
        //Debug.Log(PlayerPrefs.GetInt("FirstTimeStart"));
    }

    // Update is called once per frame
    void Update()
    {
        if(Statics.DistanceTraveled > 50f && PlayerPrefs.GetInt("FirstTimeStart") == 1)
        {
            PlayerSpeedIncrease();
            DifficultyIncrease();
            return;
        }
        else
            if(PlayerPrefs.GetInt("FirstTimeStart") == 0)
            {
                FirstStart();
            } 
    }

    void PlayerSpeedIncrease()
    {
        //increases players speed overtime
        if(Statics.DistanceTraveled  > _currentCheckpoint && GameManager._player.speed < GameManager._player.maxSpeed)
        {
            _currentCheckpoint = Statics.DistanceTraveled + speedCheckpointDistance;
            //Debug.Log("Checkpoint: " + _currentCheckpoint);
            GameManager._player.speed += speedIncreaseAmount;
            //Debug.Log("Speed increased. <color=red>The new speed is: </color>"  + GameManager._player.speed +  " || <color=blue>The next checkpoint is: </color>"  + _currentCheckpoint+"m");
        }
    }

    void DifficultyIncrease()
    {
        switch (difficultyLvl)
        {
            case 0: 
                if(CheckDistanceAndLevel(levelInfos[1].checkpoint))
                {
                    SetDifficulty(1);
                }
                break;
            case 1:
                if(CheckDistanceAndLevel(levelInfos[2].checkpoint))
                {
                    SetDifficulty(2);
                    ObstacleSpawner.Instance.UpdateLevelConfig(MasterSpawner.Instance.activeRegion.obstaclePoolsLevelInstances[1]);
                    StartCoroutine(masterSpawner.projectileSpawner.RandomSpawnType());
                    
                }
                break;
            case 2:
                if(CheckDistanceAndLevel(levelInfos[3].checkpoint))
                {
                    SetDifficulty(3);
                    ObstacleSpawner.Instance.UpdateLevelConfig(MasterSpawner.Instance.activeRegion.obstaclePoolsLevelInstances[2]);
                    masterSpawner.projectileSpawner.spawnAmount = new Vector2(2,4);
                }
                break;
            case 3:
                if(CheckDistanceAndLevel(levelInfos[4].checkpoint))
                {
                    SetDifficulty(4);
                    //force spawn the special item only every 2 rounds
                    if(_sItemForcedSpawnCounter <= 1)
                        _sItemForcedSpawnCounter++;
                        else if(_sItemForcedSpawnCounter >= 2)
                        {
                            RewardSpawner.instance.ChangeRewardPoolSpawnChances(0, 0,1f);
                            _sItemForcedSpawnCounter = 0; 
                        }
                    ObstacleSpawner.Instance.UpdateLevelConfig(MasterSpawner.Instance.activeRegion.obstaclePoolsLevelInstances[3]);
                    //Changing Projectile spawner settings
                    masterSpawner.projectileSpawner.spawnAmount = new Vector2(3,4);
                    masterSpawner.projectileSpawner.warningTimer = 1f;
                    masterSpawner.projectileSpawner.disableOSpawnerTimer = 2.5f;
                }
                break;
            case 4:
                if(CheckDistanceAndLevel(levelInfos[4].checkpoint))
                {
                    levelInfos[4].checkpoint += Random.Range(200f, 300f);
                    masterSpawner.minSpawnAmount += 1;
                    masterSpawner.maxSpawnAmount += 2;
                    roundsCompleted ++;
                }
                break;
            default:
               break;
        }
    }

    public void SetDifficulty(int lvl)
    {
        difficultyLvl = lvl;
        //Debug.Log("Lvl " + difficultyLvl);
        LevelInfo levelInfo = levelInfos[difficultyLvl];
        masterSpawner.minDistance = levelInfo.minSpawnDistance;
        masterSpawner.maxDistance = levelInfo.maxSpawnDistance;
        RewardSpawner.instance.ChangeRewardPoolSpawnChances(levelInfo.stickerChance, levelInfo.itemChance, levelInfo.specialItemChance);
        masterSpawner.ChangeSpawnerTypeChance(levelInfo.projectileChance, levelInfo.obstacleChance);
        ObstacleSpawner.Instance.changePoolSpawnChance(levelInfo.ObstTopChance, levelInfo.ObstMidChance, levelInfo.ObstBotChance);
    }

    bool CheckDistanceAndLevel(float distanceToCheck)
    {
        return Statics.DistanceTraveled  >= distanceToCheck;
    }

    void FirstStart()
    {
        //spawn small jumpable obstacle
        if(Statics.DistanceTraveled  >= _currentCheckpoint && startCounter < 3)
        {
            masterSpawner.enabled = false;
            _currentCheckpoint = Statics.DistanceTraveled  + 30f;
            Debug.Log("Spawned");
            SpawnSpecificObjectAtFloor(startCounter);
            startCounter++;
        }
        else //spawn big obstacle
            if(Statics.DistanceTraveled  >= 90f && startCounter == 3)
            {
                DialogueConfig dialogue = new DialogueConfig();
                dialogue.character = GameManager._player.gameObject;
                dialogue.bubbleSize = Enums.BubbleSize.md;
                dialogue.diallogueInterval = 5f;
                dialogue.text = "Press and hold to jump higher";

                dialogueSequence.dialogues[0] = dialogue;
                dialogueSequence.SetSpeechbubble(dialogueSequence.dialogues[0]);
                dialogueSequence.stopPlayerMoving = false;
                dialogueSequence.StartDialogue(CharacterManager.activeCharacter);
            
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
            startingPool.spawnedObjectPool[objectIndex].transform.position = ObstacleSpawner.Instance
                .GetFloorSpawnPoint(startingPool.spawnedObjectPool[objectIndex].GetComponent<Collider2D>());      
    }

[System.Serializable]
public class LevelInfo 
{
    public float checkpoint;
    public float minSpawnDistance, maxSpawnDistance;

    [Header("Spawner Type Chance")]
    public float projectileChance;
    public float obstacleChance;

    [Header("Obstacle location Spawn Chance")]
    public float ObstTopChance;
    public float ObstMidChance;
    public float ObstBotChance;

    [Header("Reward Type Spawn Chance")]
    public float stickerChance;
    public float itemChance;
    public float specialItemChance;
}
}
