using System.Collections;
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

    public bool GetBossSpawned() { return bossSpawned; }
    void Awake()
    {
	    audioManager = AudioManager.GetAudioManager();
	    runManager = RunManager.GetRunManager();
    }
	public void Update()
    {

        if (bossSpawned)
        {
            if (audioManager)
            {
                if (runManager)
                {
                    audioManager.FadeOutAndPlayMusic($"Level {runManager.GetSceneIndex() - 1} Non Combat", $"Level {runManager.GetSceneIndex() - 1} Boss");
                }
            }
        }

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
                    audioManager.SetCurrentStateToFadeOutAudio1();
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
	        QuestManager.GetQuestManager().SpawnUpdate(currentBoss, "Boss");
            //Lock the doors
            LockDoors();

            if (audioManager)
            {
                audioManager.SetCurrentStateToFadeOutAudio2();
            }
        }



    }




}
