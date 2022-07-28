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

    }

    protected override void Effect()
    {
        base.Effect();
        playerClass.StopCoroutine(playerClass.Vulnerable(new PlayerClass.defenseMultiSource()));
        playerClass.StartCoroutine(playerClass.Vulnerable(new PlayerClass.defenseMultiSource(0.5f, "Flying Enemy")));
    }
}
