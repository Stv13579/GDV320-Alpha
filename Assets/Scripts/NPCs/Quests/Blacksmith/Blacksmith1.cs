using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blacksmith1 : Quest
{
    List<GameObject> slainEnemies = new List<GameObject>();

    //Added to death triggers
    public void DeathTypeCheck(GameObject enemy)
    {

        bool slain = false;

        foreach(GameObject collectedEnemy in slainEnemies)
        {
            if(collectedEnemy.name == enemy.name)
            {
                slain = true;
                break;
            }
        }

        if(slain == false)
        {
            slainEnemies.Add(enemy);
        }

        //If there are 12 enemy types in slain enemy, finish the quest
        if (slainEnemies.Count == 12)
        {
            FinishQuest();
        }
    }

    public override void SpawnEventBehaviour(GameObject enemySpawning, string spawnOrigin)
    {
        base.SpawnEventBehaviour(enemySpawning, spawnOrigin);

        if(spawnOrigin != "Regular")
        {
            return;
        }

        enemySpawning.GetComponent<BaseEnemyClass>().GetDeathTriggers().Add(DeathTypeCheck);
    }

}
