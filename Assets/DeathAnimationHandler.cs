using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimationHandler : MonoBehaviour
{
    public void Death()
    {
        this.transform.parent.gameObject.GetComponent<BaseEnemyClass>().Death();
    }
}
