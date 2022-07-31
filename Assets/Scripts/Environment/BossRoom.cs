using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : Room
{
    [SerializeField]
    GameObject boss;

    [SerializeField]
    Transform spawnPosition;


    bool bossSpawned = false;
    bool bossDead = false;

    GameObject currentBoss;

    [SerializeField]
    GameObject portalObject;

    [SerializeField]
    Transform portalSpawnPosition;

    private void Update()
    {
        if(bossDead)
        {
            return;
        }

        if(!currentBoss && !bossDead && bossSpawned)
        {
            //Spawn the portal
            Instantiate(portalObject, portalSpawnPosition.position, Quaternion.identity);
            UnlockDoors();
            bossDead = true;
        }

        if (roomTrigger.GetComponent<RoomTrigger>().triggered && !bossSpawned)
        {
            bossSpawned = true;
            //Spawn the boss once
            currentBoss = Instantiate(boss, spawnPosition.position, Quaternion.identity);
            //Lock the doors
            LockDoors();
        }
    }




}
