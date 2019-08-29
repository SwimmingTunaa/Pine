﻿using System.Collections;
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
    private float _currentCheckpoint;
    private int _difficultyLvl = 0;
    private int startCounter = 0;

    void Start()
    {
        _currentCheckpoint = PlayerPrefs.GetInt("FirstTimeStart")<1 ? 40f: speedCheckpointDistance;
        Debug.Log(PlayerPrefs.GetInt("FirstTimeStart"));
    }

    // Update is called once per frame
    void Update()
    {
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
            Debug.Log("Checkpoint: " + _currentCheckpoint);
            GameManager._player.speed += speedIncreaseAmount;
            //Debug.Log("Speed increased. <color=red>The new speed is: </color>"  + _player.speed +  " || <color=blue>The next checkpoint is: </color>"  + _currentCheckpoint+"m");
        }
        if(GameManager._distanceTraveled >= 100f && _difficultyLvl == 0)
        {
            _difficultyLvl = 1;
            masterSpawner.minDistance = 20f;
            masterSpawner.maxDistance = 28f;
            masterSpawner.obstacleSpawner.changePoolSpawnChance(0.2f,0.2f,0.6f);
        }
        else
        if(GameManager._distanceTraveled >= 300 && _difficultyLvl == 1)
        {
            _difficultyLvl +=1;
            masterSpawner.obstacleSpawner.currentLevelConfig = masterSpawner.obstacleSpawner.levelConfig.leveltwo;
            StartCoroutine(masterSpawner.projectileSpawner.RandomSpawnType());
            masterSpawner.obstacleSpawner.changePoolSpawnChance(0.35f,0.20f,0.45f);
            masterSpawner.ChangeSpawnerTypeChance(0.3f, 0.7f);
            masterSpawner.minDistance = 18f;
            masterSpawner.maxDistance = 22f;
            Debug.Log("Lvl " + _difficultyLvl);
        }
        else
        if(GameManager._distanceTraveled >= 700 && _difficultyLvl == 2)
        {
            _difficultyLvl +=1;
            masterSpawner.minDistance = 15f;
            masterSpawner.maxDistance = 25f;
            //TODO: different spawn pool
            masterSpawner.RewardSpawner.ChangeRewardPoolSpawnChances(0.7f, 0.3f);
            masterSpawner.obstacleSpawner.currentLevelConfig = masterSpawner.obstacleSpawner.levelConfig.levelthree;
            masterSpawner.obstacleSpawner.changePoolSpawnChance(0.5f,0.1f,0.4f);
            masterSpawner.ChangeSpawnerTypeChance(0.2f, 0.8f);
            Debug.Log( "Lvl " + _difficultyLvl);
           
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
        else //spawn big obstacle
            if(GameManager._distanceTraveled >= 120f && startCounter == 3)
            {
                DialogueConfig dialogue = new DialogueConfig();
                dialogue.character = GameManager._player.gameObject;
                dialogue.bubbleSize = Enums.BubbleSize.md;
                dialogue.diallogueInterval = 5f;
                dialogue.text = "Press and hold to jump higher";

                dialogueSequence.dialogues[0] = dialogue;
                dialogueSequence.setSpeechbubble(dialogueSequence.dialogues[0]);
                dialogueSequence.stopPlayerMoving = false;
                dialogueSequence.startDialogue();
            
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
