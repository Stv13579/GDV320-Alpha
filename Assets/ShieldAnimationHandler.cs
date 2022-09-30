using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAnimationHandler : MonoBehaviour
{
    public void ShieldEvent(int stateOfShield)
    {
        GetComponentInParent<ShieldEnemyScript>().SetShield(stateOfShield > 0 ? true: false);
    }
}
