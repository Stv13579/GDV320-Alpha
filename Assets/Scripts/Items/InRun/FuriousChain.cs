using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuriousChain : Item
{
    [SerializeField]
    float baseDamage;
	float currentDamage = 5;

    //Add the hit trigger to the list of delegates. this allows the damage to scale on sunsequent hits 
    public override void OnHitTriggers(BaseEnemyClass enemyHit, List<BaseEnemyClass.Types> types)
    {
        base.OnHitTriggers(enemyHit, types);
        enemyHit.GetHitTriggers().Add(HitTrigger);
    }

    //Add to an enemy, the more added, the more damage it will deal
    void HitTrigger(BaseEnemyClass enemy, List<BaseEnemyClass.Types> types)
    {
	    enemy.TakeDamage(currentDamage, types, 1, false);
    }
}
