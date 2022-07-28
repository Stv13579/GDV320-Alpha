using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRangedProjectile : BaseRangedProjectileScript
{
    protected override void HitEffect()
    {
        player.GetComponent<PlayerClass>().ChangeHealth(-damage);
        player.GetComponent<PlayerMovement>().StopCoroutine(player.GetComponent<PlayerMovement>().Slowness(new PlayerMovement.movementMultiSource()));
        player.GetComponent<PlayerMovement>().StartCoroutine(player.GetComponent<PlayerMovement>().Slowness(new PlayerMovement.movementMultiSource(0.5f, "Water Ranged")));
        Destroy(this.gameObject);


    }
}
