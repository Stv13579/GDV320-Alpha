using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRangedProjectile : BaseRangedProjectileScript
{
    public float fireDuration;
    protected override void HitEffect()
    {
        player.GetComponent<PlayerClass>().ChangeHealth(-damage);
        player.GetComponent<PlayerClass>().OnFire(fireDuration);
        Destroy(this.gameObject);
    }
}
