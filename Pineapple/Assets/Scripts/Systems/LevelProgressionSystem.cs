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
    public GameObject holePanelToAdd;
    public float speedIncreaseAmount;
    public float speedCheckpointDistance;
    
    public int difficultyLvl = 0;
    [SerializeField] public List<LevelInfo> levelInfos = new List<LevelInfo>();

    public int roundsCompleted;  
    private float _currentCheckpoint;
    private int startCounter = 0;
    private int _sItemForcedSpawnCounter;
    private MasterSpawner _masterSpawner;
    private float _timer;
    private float _interval;

    void Awake()
    {
        Instance = this;
        if(PlayerPrefs.GetInt("FirstTimeStart") == 0) startingPool.Initialise();
    } 
    void Start()
    {
        _masterSpawner = MasterSpawner.Instance;
        _currentCheckpoint = PlayerPrefs.GetInt("FirstTimeStart") < 1 ? speedCheckpointDistance + 400f: speedCheckpointDistance;
//        Debug.Log(_currentCheckpoint + " current checkpoint");
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
                if(Timer(_interval) && _interval > 0)
                {
                    Time.timeScale = 1;
                    _interval= 0;
                    if(startCounter >= 5)
                    {
                        MasterSpawner.Instance.enabled = true;
                        PlayerPrefs.SetInt("FirstTimeStart", 1);
                    }
                }
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
                    if(!MasterSpawner.Instance.activeRegion.panels.spawnedObjectPool.Contains(holePanelToAdd))
                    {
                        //add hole panel so that player can fall down into new area
                        for (int i = 0; i < 3; i++)
                        {
                            GameObject tempGO = Instantiate(holePanelToAdd);
                            tempGO.SetActive(false);
                            tempGO.transform.parent = PoolManager.instance.transform;
                            MasterSpawner.Instance.activeRegion.panels.spawnedObjectPool.Add(tempGO);
                        }
                    }
                }
                break;
            case 1:
                if(CheckDistanceAndLevel(levelInfos[2].checkpoint))
                {
                    SetDifficulty(2);
                    ObstacleSpawner.Instance.UpdateLevelConfig(MasterSpawner.Instance.activeRegion.obstaclePoolsLevelInstances[1]);
                    StartCoroutine(_masterSpawner.projectileSpawner.RandomSpawnType());
                }
                break;
            case 2:
                if(CheckDistanceAndLevel(levelInfos[3].checkpoint))
                {
                    SetDifficulty(3);
                    ObstacleSpawner.Instance.UpdateLevelConfig(MasterSpawner.Instance.activeRegion.obstaclePoolsLevelInstances[2]);
                    _masterSpawner.projectileSpawner.spawnAmount = new Vector2(2,4);
                  
                    
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
                    _masterSpawner.projectileSpawner.spawnAmount = new Vector2(3,4);
                    _masterSpawner.projectileSpawner.warningTimer = 1f;
                    _masterSpawner.projectileSpawner.disableOSpawnerTimer = 2.5f;
                
                }
                break;
            case 4:
                if(CheckDistanceAndLevel(levelInfos[4].checkpoint))
                {
                    levelInfos[4].checkpoint += Random.Range(200f, 300f);
                    _masterSpawner.minSpawnAmount += 1;
                    _masterSpawner.maxSpawnAmount += 2;
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
        _masterSpawner.minDistance = levelInfo.minSpawnDistance;
        _masterSpawner.maxDistance = levelInfo.maxSpawnDistance;
        RewardSpawner.instance.ChangeRewardPoolSpawnChances(levelInfo.stickerChance, levelInfo.itemChance, levelInfo.specialItemChance);
        _masterSpawner.ChangeSpawnerTypeChance(levelInfo.projectileChance, levelInfo.obstacleChance);
        ObstacleSpawner.Instance.changePoolSpawnChance(levelInfo.ObstTopChance, levelInfo.ObstMidChance, levelInfo.ObstBotChance);
    }

    bool CheckDistanceAndLevel(float distanceToCheck)
    {
        if(PlayerPrefs.GetInt("FirstTimeStart") > 0)
            return Statics.DistanceTraveled  >=  distanceToCheck;
        else
            return Statics.DistanceTraveled  >=  (distanceToCheck + 400f); // added the distance it took to do the tutorial

    }

    void FirstStart()
    {
        _masterSpawner.enabled = false;
        switch(startCounter)
        {
            //jumpover small obstacle
            case 0:
                if(Statics.DistanceTraveled  >= 50f)
                {
                    //_currentCheckpoint = Statics.DistanceTraveled + 20f;
                    PlayDialogue("Tap to jump", Enums.BubbleSize.md, 3f);
                }
            break;
            case 1://jump over tall book shelf
                if(Statics.DistanceTraveled  >= 150f)
                {
                    PlayDialogue("Hold and press to jump higher", Enums.BubbleSize.md, 3f);
                }
            break;
            case 2: // jump over long bookshelf
                if(Statics.DistanceTraveled  >= 250f)
                {
                    PlayDialogue("Tap again after jumping to double jump", Enums.BubbleSize.lg, 3f);
                }
            break;
            case 3://stickers
                if(Statics.DistanceTraveled  >= 320f)
                {
                    PlayDialogue("Collect Stickers to spend in the Market Place and earn points", Enums.BubbleSize.lg, 3f);
                }
            break;
            case 4://seedlings
                if(Statics.DistanceTraveled  >= 400f)
                {
                    PlayDialogue("Collect Seedlings to gain points", Enums.BubbleSize.md, 3f);
                }
            break;
            default:
                Debug.Log("Tutorial is Done");
            break;
        }
      
         //spawn big obstacle
          
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

    void PlayDialogue(string text, Enums.BubbleSize bubbleSize, float interval)
    {
        dialogueSequence.EndDialogue();
        DialogueConfig dialogue = new DialogueConfig();
        dialogue.character = CharacterManager.activeCharacter;
        dialogue.bubbleSize = bubbleSize;
        dialogue.diallogueInterval = interval;
        dialogue.text = text;
        
        dialogueSequence.dialogues[0] = dialogue;
        dialogueSequence.StartDialogue(0);
        Time.timeScale = .5f;
        _interval = 1f;
        SpawnSpecificObjectAtFloor(startCounter);
        startCounter++;
    }

    void SpawnSpecificObjectAtFloor(int objectIndex = 0)
    {
            ObstaclePoolConfig tempConfig = new ObstaclePoolConfig(); 
            startingPool.spawnedObjectPool[objectIndex].SetActive(true);
            tempConfig.bot = startingPool;
            startingPool.spawnedObjectPool[objectIndex].transform.position = ObstacleSpawner.Instance
                .GetFloorSpawnPoint(startingPool.spawnedObjectPool[objectIndex].GetComponent<Collider2D>(), -8f);      
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
