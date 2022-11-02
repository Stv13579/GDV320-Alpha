using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaterFlyingEnemyScript : BaseFlyingEnemyScript //Sebastian
{
    [SerializeField]
    float healthRestore = 25.0f;
    //Find all the enemies around the target enemy, heal them, and find a new target

	public override void Effect()
    {

        base.Effect();
        Debug.Log("Effect");
        Collider[] objects = Physics.OverlapSphere(transform.position, effectRange);
        foreach (Collider col in objects)
        {
            if (col.gameObject.GetComponent<BaseEnemyClass>())
            {
                if (col.gameObject.GetComponent<BaseFlyingEnemyScript>())
                {
                    continue;
                }

                col.gameObject.GetComponent<BaseEnemyClass>().RestoreHealth(healthRestore);
                if(col.gameObject.GetComponent<BaseEnemyClass>().healVFX)
                {
                    col.gameObject.GetComponent<BaseEnemyClass>().healVFX.Play();
                }
                
            }
        }
        FindTarget();

    }
}
