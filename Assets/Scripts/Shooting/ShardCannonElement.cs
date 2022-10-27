using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardCannonElement : BaseElementClass
{
    //A high speed attack with a big delay between shots and a high mana cost.
    //A single high damage accurate crystal projectile.
    //Can cause collateral damage to targets beyond the first (pierces through).

    //Click to fire (if we have the mana/not in delay)
    //Instantiate the projectile in the right direction etc.
    [SerializeField]
    GameObject shardProj;

    [SerializeField]
    float projectileSpeed;

    [SerializeField]
    float upgradedCoolDownTimer;

    [SerializeField]
    AnimationState shardCannonAnims;

    [SerializeField]
    float animsSpeed;

    [SerializeField]
    float upgradedManaCost;

    protected override void Update()
    {
        base.Update();
    }

    //Fires the shard, passing damage, speed etc
    public override void ElementEffect()
    {
        base.ElementEffect();
        Quaternion rot = Camera.main.transform.rotation;
        rot = rot * Quaternion.Euler(90, 0, 0);
        GameObject newShard = Instantiate(shardProj, shootingTranform.position, rot);
        newShard.GetComponent<ShardProjectile>().SetVars(projectileSpeed, damage * (damageMultiplier + elementData.crystalDamageMultiplier), attackTypes);
    }

    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }

    protected override void StartAnims(string animationName, string animationNameAlt = null)
    {
        base.StartAnims(animationName);

        playerHand.SetTrigger(animationName);
    }
    public override void Upgrade()
    {
        base.Upgrade();
        cooldownTimer = upgradedCoolDownTimer;
        manaCost = upgradedManaCost;
        playerHand.SetFloat("ShardCannonAnimsSpeed", animsSpeed);
    }
}
