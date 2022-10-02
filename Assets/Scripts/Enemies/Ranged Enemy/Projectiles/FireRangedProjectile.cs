using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRangedProjectile : BaseRangedProjectileScript //Sebastian
{
    [SerializeField]
    float fireDuration;
    //Deals damage, sets the player on fire
    protected override void HitEffect(Collider other)
    {
        player.GetComponent<PlayerClass>().ChangeHealth(-damage, origin);
        player.GetComponent<PlayerClass>().OnFire(fireDuration);
        Destroy(this.gameObject);
    }
}
