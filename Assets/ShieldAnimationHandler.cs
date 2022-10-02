using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAnimationHandler : MonoBehaviour
{
    public void ShieldEvent(int stateOfShield)
    {
        GetComponentInParent<ShieldEnemyScript>().SetShield(stateOfShield > 0 ? true: false);
        GetComponent<Animator>().ResetTrigger("Shield Up");
        GetComponent<Animator>().ResetTrigger("Shield Down");
    }

    public void AttackFinished()
    {
        GetComponentInParent<ShieldEnemyScript>().attacking = false;
        GetComponent<Animator>().ResetTrigger("Attacking");
        GetComponentInParent<ShieldEnemyScript>().capHitter.SetActive(false);
        GetComponentInParent<ShieldEnemyScript>().SetShield(true);
    }
}
