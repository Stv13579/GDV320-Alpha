using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : BaseDropScript
{
    public static float healthRestore = 10;
    protected override void PickupEffect()
    {
        base.PickupEffect();
        player.gameObject.GetComponent<PlayerClass>().ChangeHealth(healthRestore);
    }
}

