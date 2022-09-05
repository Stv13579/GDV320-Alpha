using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFlyingEnemyScript : BaseFlyingEnemyScript //Sebastian
{
    [SerializeField]
    float healthRestore = 2.0f;
    //Find all the enemies around the target enemy, heal them, and find a new target

	public override void Effect()
    {

        base.Effect();
        Debug.Log("Effect");
        Collider[] objects = Physics.OverlapSphere(target.transform.position, 5.0f);
        foreach (Collider col in objects)
        {
            if (col.gameObject.GetComponent<BaseEnemyClass>())
            {
                col.gameObject.GetComponent<BaseEnemyClass>().RestoreHealth(healthRestore);
            }
        }
        FindTarget();

    }
}
