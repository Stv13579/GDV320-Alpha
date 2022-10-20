using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterElement : BaseElementClass
{
    //A bouncy ball of water

    //Click to fire (if we have the mana/not in delay)
    //Instantiate the projectile in the right direction etc.
    [SerializeField]
    GameObject waterProj;

    [SerializeField]
    float projectileSpeed = 5;

    [SerializeField]
    float projectileLifetime = 10;

    //Fires the projectile, passing damage, speed, etc
    public override void ElementEffect()
    {
        base.ElementEffect();
        //
        GameObject newWaterProj = Instantiate(waterProj, shootingTranform.position, Camera.main.transform.rotation);
        playerClass.OnFire(0);
        Physics.IgnoreCollision(newWaterProj.GetComponent<Collider>(), this.gameObject.GetComponent<Collider>());
        RaycastHit hit;
        Physics.Raycast(this.gameObject.transform.position, Camera.main.transform.forward, out hit, 100, shootingIgnore);
        if (hit.collider)
        {
            newWaterProj.transform.LookAt(hit.point);
        }
        newWaterProj.GetComponent<WaterProjectile>().SetVars(projectileSpeed, damage * (damageMultiplier * elementData.waterDamageMultiplier), projectileLifetime, attackTypes);
    }

    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }

    protected override void StartAnims(string animationName, string animationNameAlt = null)
    {
        base.StartAnims(animationName, animationNameAlt);
        randomAnimationToPlay = Random.Range(0, 2);
        if (randomAnimationToPlay == 0)
        {
            playerHand.SetTrigger(animationName);
            if (audioManager)
            {
                audioManager.StopSFX(shootingSoundFX);
                audioManager.PlaySFX(shootingSoundFX);
            }
        }
        else
        {
            playerHand.SetTrigger(animationNameAlt);
            if (audioManager)
            {
                audioManager.StopSFX(otherShootingSoundFX);
                audioManager.PlaySFX(otherShootingSoundFX);
            }
        }
    }
}
