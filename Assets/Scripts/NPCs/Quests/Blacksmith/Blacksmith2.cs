using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blacksmith2 : Quest
{
    List<string> slainEnemies = new List<string>();

    //Added to death triggers
    public void DeathTypeCheck(GameObject enemy)
    {

        bool slain = false;

        foreach (string collectedEnemy in slainEnemies)
        {
            if (collectedEnemy == enemy.name)
            {
                slain = true;
                break;
            }
        }

        if (slain == false)
        {
            slainEnemies.Add(enemy.name);
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

        if (spawnOrigin != "Boss")
        {
            return;
        }

        enemySpawning.GetComponent<BaseEnemyClass>().GetDeathTriggers().Add(DeathTypeCheck);
    }
    
}
