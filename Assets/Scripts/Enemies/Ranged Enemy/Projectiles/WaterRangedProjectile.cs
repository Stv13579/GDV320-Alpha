using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRangedProjectile : BaseRangedProjectileScript
{
    protected override void HitEffect(Collider other)
    {
        player.GetComponent<PlayerClass>().ChangeHealth(-damage);
        player.GetComponent<PlayerMovement>().StopCoroutine(player.GetComponent<PlayerMovement>().Slowness(new Multiplier(1, "WaterSlow")));
        player.GetComponent<PlayerMovement>().StartCoroutine(player.GetComponent<PlayerMovement>().Slowness(new Multiplier(0.5f, "Water Ranged")));
        Destroy(this.gameObject);


    }
}
