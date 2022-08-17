using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalRangedProjectile : BaseRangedProjectileScript
{
    protected override void HitEffect(Collider other)
    {
        player.GetComponent<PlayerClass>().ChangeHealth(-damage);
        Destroy(this.gameObject);
    }
}
