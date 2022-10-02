using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRangedProjectile : BaseRangedProjectileScript //Sebastian
{
    //Deals damage, slows the player down temporarily
    protected override void HitEffect(Collider other)
    {
        player.GetComponent<PlayerClass>().ChangeHealth(-damage, origin);
        PlayerMovement playerMove = player.GetComponent<PlayerMovement>();
        StatModifier.StartAddModifierTemporary(playerMove, playerMove.GetSpeedStat().multiplicativeModifiers, new StatModifier.Modifier(0.5f, "Water Ranged"), 10.0f);
        Destroy(this.gameObject);


    }
}
