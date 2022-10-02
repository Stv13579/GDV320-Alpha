using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBossWaterProjectileScript : BaseRangedProjectileScript //Sebastian
{
    protected override void Start()
    {
        base.Start();
    }
    //Damage the player
    protected override void HitEffect(Collider other)
    {
        player.GetComponent<PlayerClass>().ChangeHealth(-damage, FindObjectOfType<RangedBossScript>().gameObject);
        Destroy(this.gameObject);
    }
}
