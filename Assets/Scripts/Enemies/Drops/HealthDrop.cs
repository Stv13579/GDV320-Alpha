using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : BaseDropScript //Sebastian
{
    [SerializeField]
    static float healthRestore = 75;

    protected override void PickupEffect()
    {
        base.PickupEffect();
        player.gameObject.GetComponent<PlayerClass>().ChangeHealth(healthRestore, null);
    }

    public void SetHealth(float health)
    {
        healthRestore = health;
    }
}

