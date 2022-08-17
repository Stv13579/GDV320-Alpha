using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBossWaterProjectileScript : BaseRangedProjectileScript
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void HitEffect(Collider other)
    {
        player.GetComponent<PlayerClass>().ChangeHealth(-damage);
        Destroy(this.gameObject);
    }
}
