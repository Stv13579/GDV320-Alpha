using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalFlyingEnemy : BaseFlyingEnemyScript
{
    //Overrid of find target to pick an area around the player instead of an enemy
    protected override void FindTarget()
    {
        target = player;
        targetPos = player.transform.position + new Vector3(0, 10, 0);

        RaycastHit hit;
        if (Physics.SphereCast(this.transform.position, 0.5f, (this.transform.position - targetPos).normalized, out hit, Vector3.Distance(this.transform.position, targetPos), moveDetect))
        {
            targetPos = hit.point - (this.transform.position - targetPos).normalized * 2;
        }
        BaseEnemyClass[] enemies = FindObjectsOfType<BaseEnemyClass>();



        List<BaseEnemyClass> validEnemies = new List<BaseEnemyClass>();
        foreach (BaseEnemyClass enemy in enemies)
        {
            //Get a list of all non-flying enemies that the enemy can reach

            if (!enemy.gameObject.GetComponent<BaseFlyingEnemyScript>())
            {
                if (enemy.gameObject != target)
                {
                    target = enemy.gameObject;
                }
                targetPos = target.transform.position + new Vector3(0, 10, 0);
                RaycastHit hit1;
                if (!Physics.SphereCast(this.transform.position, 0.5f, (this.transform.position - targetPos).normalized, out hit1, Vector3.Distance(this.transform.position, targetPos), moveDetect))
                {
                    validEnemies.Add(enemy);
                }
            }
        }
        if(validEnemies.Count <= 0)
        {
            currentHealth = 0;
            Death();
        }
    }

    protected override void Effect()
    {
        base.Effect();
        playerClass.StopCoroutine(playerClass.Vulnerable(new PlayerClass.defenseMultiSource()));
        playerClass.StartCoroutine(playerClass.Vulnerable(new PlayerClass.defenseMultiSource(0.5f, "Flying Enemy")));
    }
}
