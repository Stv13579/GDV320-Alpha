using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeacefulHourglass : Item
{
    
    float addedDamagePerSecond = 5, maxAddedDamage = 50;
    float currentAddedDamage, addTimer;

    void Update()
    {
        if(addTimer >= 1)
        {
            addTimer = 0;
            if(currentAddedDamage < maxAddedDamage)
            {
                currentAddedDamage += addedDamagePerSecond;
            }
        }
        else
        {
            addTimer += Time.deltaTime;
        }
    }

    public override void OnHitTriggers(BaseEnemyClass enemyHit, List<BaseEnemyClass.Types> types)
    {
        base.OnHitTriggers(enemyHit, types);

        enemyHit.TakeDamage(currentAddedDamage, types, 1, false);
        currentAddedDamage = 0;
        addTimer = 0;
    }
}
