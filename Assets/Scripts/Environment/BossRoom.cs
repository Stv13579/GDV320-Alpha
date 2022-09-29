﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : Room
{
    [SerializeField]
    BossList bosses;

    [SerializeField]
    Transform spawnPosition;

    [SerializeField]
    GameObject spawner;

    bool bossSpawned = false;
    bool bossDead = false;

    GameObject currentBoss;
    string bossName;

    [SerializeField]
    GameObject portalObject;

    [SerializeField]
    Transform portalSpawnPosition;

    // audio manager
    AudioManager audioManager;

    RunManager runManager;
    public BossList GetList() { return bosses; }
    void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        runManager = FindObjectOfType<RunManager>();
    }
    public void Update()
    {
        base.Update();

        // need to fix boss audio with saim audio
        //if (audioManager)
        //{
        //if (runManager)
        //{
        //    audioManager.FadeOutAndPlayMusic($"Level {runManager.GetSceneIndex() - 1} Non Combat", $"Level {runManager.GetSceneIndex() - 1} Boss");
        //}
        //}

        if (bossSpawned)
        {
            if (FindObjectsOfType<BaseEnemyClass>().Length == 0 && !bossDead && bossSpawned)
            {
                //Spawn the portal
                Instantiate(portalObject, portalSpawnPosition.position, Quaternion.identity);
                UnlockDoors();
                
                bossDead = true;

                Destroy(GameObject.Find("Boss Healthbar(Clone)"));
                
                //if the boss dies set this to true
                if (audioManager)
                {
                    audioManager.SetCurrentStateToFadeOutAudio2();
                }
            }
        }

        if (roomTrigger.GetComponent<RoomTrigger>().triggered && !bossSpawned)
        {
            bossSpawned = true;
            //Spawn the boss once
            currentBoss = Instantiate(bosses.GetBoss(), spawnPosition.position, Quaternion.identity);
            currentBoss.GetComponent<BaseEnemyClass>().SetSpawner(spawner);
            spawner.GetComponent<SAIM>().spawnedEnemies.Add(currentBoss.GetComponent<BaseEnemyClass>());
            GameObject.Find("Quest Manager").GetComponent<QuestManager>().SpawnUpdate(currentBoss, "Boss");
            //Lock the doors
            LockDoors();

            // when the boss spawns set this to true
            if (audioManager)
            {
                audioManager.SetCurrentStateToFadeOutAudio1();
            }
        }



    }




}
