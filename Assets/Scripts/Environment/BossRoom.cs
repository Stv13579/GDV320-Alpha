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

    public BossList GetList() { return bosses; }

    public void Update()
    {
        base.Update();
        
        if(bossSpawned)
        {
            if (FindObjectsOfType<BaseEnemyClass>().Length == 0 && !bossDead && bossSpawned)
            {
                //Spawn the portal
                Instantiate(portalObject, portalSpawnPosition.position, Quaternion.identity);
                UnlockDoors();
                
                bossDead = true;

                Destroy(GameObject.Find("Boss Healthbar(Clone)"));
            }
        }

        if (roomTrigger.GetComponent<RoomTrigger>().triggered && !bossSpawned)
        {
            bossSpawned = true;
            //Spawn the boss once
            currentBoss = Instantiate(bosses.GetBoss(), spawnPosition.position, Quaternion.identity);
            currentBoss.GetComponent<BaseEnemyClass>().SetSpawner(spawner);
            GameObject.Find("Quest Manager").GetComponent<QuestManager>().SpawnUpdate(currentBoss, "Boss");
            //Lock the doors
            LockDoors();
        }



    }




}
