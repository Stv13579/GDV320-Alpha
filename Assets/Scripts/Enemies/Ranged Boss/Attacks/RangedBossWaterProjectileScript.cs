using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBossWaterProjectileScript : BaseRangedProjectileScript
{
    protected override void HitEffect()
    {
        player.GetComponent<PlayerClass>().ChangeHealth(-damage);
        Destroy(this.gameObject);
    }
}
