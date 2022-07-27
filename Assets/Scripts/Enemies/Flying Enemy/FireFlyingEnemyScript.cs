using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlyingEnemyScript : BaseFlyingEnemyScript
{
    protected override void Effect()
    {
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
