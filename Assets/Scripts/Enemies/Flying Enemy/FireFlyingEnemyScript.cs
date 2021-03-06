using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlyingEnemyScript : BaseFlyingEnemyScript
{
    protected override void Effect()
    {
        //Find all the enemies around the target enemy, buff their attack, and find a new target
        base.Effect();
        Debug.Log("Effect");
        Collider[] objects = Physics.OverlapSphere(target.transform.position, 5.0f);
        foreach(Collider col in objects)
        {
            if(col.gameObject.GetComponent<BaseEnemyClass>())
            {
                col.gameObject.GetComponent<BaseEnemyClass>().StartCoroutine(AttackBuff());
            }
        }
        FindTarget();
            
    }
}
