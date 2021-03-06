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
    float damage = 1;
    public float damageMultiplier = 1;

    [SerializeField]
    float projectileSpeed = 5;

    [SerializeField]
    public float projectileLifetime = 10;

    protected override void Update()
    {
        base.Update();


    }

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
            Debug.Log(hit.collider.gameObject.name);
        }
        newWaterProj.GetComponent<WaterProjectile>().SetVars(projectileSpeed, damage * damageMultiplier, projectileLifetime, attackTypes);
    }

    public override void ActivateVFX()
    {
        base.ActivateVFX();


    }

    protected override void StartAnims(string animationName)
    {
        base.StartAnims(animationName);

        playerHand.SetTrigger(animationName);

    }
}
