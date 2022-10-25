using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRangedProjectile : BaseRangedProjectileScript //Sebastian
{
    PlayerClass playerClass;
    protected override void Awake()
    {
        base.Awake();
        playerClass = FindObjectOfType<PlayerClass>();
    }
    //Deals damage, slows the player down temporarily
    protected override void HitEffect(Collider other)
    {
        player.GetComponent<PlayerClass>().ChangeHealth(-damage, origin);
        PlayerMovement playerMove = player.GetComponent<PlayerMovement>();
        StatModifier.StartAddModifierTemporary(playerMove, playerMove.GetSpeedStat().multiplicativeModifiers, new StatModifier.Modifier(0.5f, "Water Ranged"), 5.0f);
        playerClass.SetSlowed(true);
        Destroy(this.gameObject);
    }
}