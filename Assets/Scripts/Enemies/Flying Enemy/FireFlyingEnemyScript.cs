using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlyingEnemyScript : BaseFlyingEnemyScript //Sebastian
{
    //Find all the enemies around the target enemy, buff their attack, and find a new target
	public override void Effect()
    {
        base.Effect();
        Collider[] objects = Physics.OverlapSphere(transform.position, effectRange);
        foreach(Collider col in objects)
        {
            if(col.gameObject.GetComponent<BaseEnemyClass>())
            {
                if(col.gameObject.GetComponent<BaseFlyingEnemyScript>())
                {
                    continue;
                }

                BaseEnemyClass enemy = col.gameObject.GetComponent<BaseEnemyClass>();

                StatModifier.StartAddModifierTemporary(enemy, enemy.GetDamageStat().multiplicativeModifiers, new StatModifier.Modifier(2.5f, "FireFlyingAttackBuff"), 5.0f);
                if(enemy.buffVFX)
                {
                    enemy.buffVFX.Play();
                }

            }
        }
        FindTarget();
            
    }
}
