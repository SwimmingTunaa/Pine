using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressionSystem : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("First Time Starting Game")]
    public ObstaclePool startingPool;
    public DialogueSequence dialogueSequence;

    [Header("Normal Game Mode")]
    public float speedIncreaseAmount;
    public float speedCheckpointDistance;
    public MasterSpawner masterSpawner;
    public int difficultyLvl = 0;
    public float lvl1Checkpoint;
    public float lvl2Checkpoint;
    public float lvl3Checkpoint;
    public float lvl4Checkpoint;

    public int roundsCompleted;  
    private float _currentCheckpoint;
    private int startCounter = 0;
    private int _sItemForcedSpawnCounter;

    void Start()
    {
        _currentCheckpoint = PlayerPrefs.GetInt("FirstTimeStart")<1 ? 40f: speedCheckpointDistance;
        Debug.Log(_currentCheckpoint + " current checkpoint");
        //Debug.Log(PlayerPrefs.GetInt("FirstTimeStart"));
    }

    // Update is called once per frame
    void Update()
    {
        if(Statics.DistanceTraveled > 50f && PlayerPrefs.GetInt("FirstTimeStart") == 1)
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
        if(Statics.DistanceTraveled  > _currentCheckpoint && GameManager._player.speed < GameManager._player.maxSpeed)
        {
            _currentCheckpoint = Statics.DistanceTraveled + speedCheckpointDistance;
//            Debug.Log("Checkpoint: " + _currentCheckpoint);
            GameManager._player.speed += speedIncreaseAmount;
            //Debug.Log("Speed increased. <color=red>The new speed is: </color>"  + GameManager._player.speed +  " || <color=blue>The next checkpoint is: </color>"  + _currentCheckpoint+"m");
        }
        //Debug.Log("Lvl " + difficultyLvl);
        //if(difficultyLvl == 3) return;
        
        if(CheckDistanceAndLevel(lvl1Checkpoint, 0))
        {
            SetDifficulty(1, 20f, 25f);
            masterSpawner.obstacleSpawner.changePoolSpawnChance(0.2f,0.2f,0.6f);
//            Debug.Log("Lvl " + difficultyLvl);
        }
        else
        if(CheckDistanceAndLevel(lvl2Checkpoint, 1))
        {
            SetDifficulty(2, 15f, 20f);
            masterSpawner.obstacleSpawner.UpdateLevelConfig(masterSpawner.obstacleSpawner.levelConfig.leveltwo);
            StartCoroutine(masterSpawner.projectileSpawner.RandomSpawnType());
            masterSpawner.obstacleSpawner.changePoolSpawnChance(0.35f,0.20f,0.45f);
            masterSpawner.ChangeSpawnerTypeChance(0.3f, 0.7f);
            Debug.Log("Lvl " + difficultyLvl);
        }
        else
        if(CheckDistanceAndLevel(lvl3Checkpoint, 2))
        {
            SetDifficulty(3, 12f, 18f);
            //TODO: different spawn pool
            masterSpawner.RewardSpawner.ChangeRewardPoolSpawnChances(0.7f, 0.2f,0.1f);
           masterSpawner.obstacleSpawner.UpdateLevelConfig(masterSpawner.obstacleSpawner.levelConfig.levelthree);
            masterSpawner.obstacleSpawner.changePoolSpawnChance(0.5f,0.25f,0.25f);
            masterSpawner.ChangeSpawnerTypeChance(0.2f, 0.8f);
            masterSpawner.projectileSpawner.spawnAmount = new Vector2(2,4);
            Debug.Log( "Lvl " + difficultyLvl);

        }else
        if(CheckDistanceAndLevel(lvl4Checkpoint, 2))
        {
            SetDifficulty(4, 10f, 14f);
            //TODO: different spawn pool
            masterSpawner.RewardSpawner.ChangeRewardPoolSpawnChances(0.6f, 0.2f,0.2f);
            //force spawn the special item only every 2 rounds
            if(_sItemForcedSpawnCounter < 1)
                _sItemForcedSpawnCounter++;
                else if(_sItemForcedSpawnCounter >= 2)
                {
                   masterSpawner.RewardSpawner.ChangeRewardPoolSpawnChances(0, 0,1f); 
                }
            masterSpawner.obstacleSpawner.UpdateLevelConfig(masterSpawner.obstacleSpawner.levelConfig.levelthree);
            masterSpawner.obstacleSpawner.changePoolSpawnChance(0.8f,0f,0.2f);
            masterSpawner.ChangeSpawnerTypeChance(0.1f, 0.9f);
            //Changing Projectile spawner settings
            masterSpawner.projectileSpawner.spawnAmount = new Vector2(2,5);
            masterSpawner.projectileSpawner.warningTimer = 1f;
            masterSpawner.projectileSpawner.disableOSpawnerTimer = 1.5f;
            //////////////////////////////////////
            masterSpawner.minSpawnAmount += 1;
            masterSpawner.maxSpawnAmount += 2;
            Debug.Log( "Lvl " + difficultyLvl);
            roundsCompleted ++;
        }
        //panel cahnge?
    }

    public void SetDifficulty(int lvl, float minSpawnDistance, float maxSpawnDistance)
    {
        difficultyLvl = lvl;
        masterSpawner.minDistance = minSpawnDistance;
        masterSpawner.maxDistance = maxSpawnDistance;
    }
 
    public void SetNewCheckpoints(float lvl1, float lvl2, float lvl3, float lvl4)
    {
        lvl1Checkpoint = Statics.DistanceTraveled  + lvl1;
        lvl2Checkpoint = lvl1Checkpoint + lvl2;
        lvl3Checkpoint = lvl2Checkpoint + lvl3;
        lvl4Checkpoint = lvl3Checkpoint + lvl4;
    }

    bool CheckDistanceAndLevel(float distanceToCheck, int lvlToCheck)
    {
        return Statics.DistanceTraveled  >= distanceToCheck && difficultyLvl == lvlToCheck;
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
            startingPool.spawnedObjectPool[objectIndex].transform.position = masterSpawner.obstacleSpawner
                .GetFloorSpawnPoint(tempConfig, startingPool.spawnedObjectPool[objectIndex].GetComponent<Collider2D>());      
    }
}
