using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blacksmith1 : Quest
{
    List<GameObject> slainEnemies;

    //Added to death triggers
    public void DeathTypeCheck(GameObject enemy)
    {
        BaseEnemyClass eType = enemy.GetComponent<BaseEnemyClass>();

        if(!slainEnemies.Exists(enemyType => enemyType.GetComponent<BaseEnemyClass>() == eType))
        {
            slainEnemies.Add(enemy);
        }

        //If there are 12 enemy types in slain enemy, finish the quest
        if (slainEnemies.Count == 12)
        {
            FinishQuest();
        }
    }

    public override void SpawnEventBehaviour(GameObject enemySpawning)
    {
        base.SpawnEventBehaviour(enemySpawning);

        enemySpawning.GetComponent<BaseEnemyClass>().GetDeathTriggers().Add(DeathTypeCheck);
    }

}
