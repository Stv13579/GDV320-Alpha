using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalGrenadeElement : BaseElementClass
{
    [SerializeField]
    private GameObject CrystalGrenade;

    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    private float gravity;

    [SerializeField]
    private float timeToExplode;

    [SerializeField]
    private float explosionRange;

    [SerializeField]
    private float explosionDamage;
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void ElementEffect()
    {
        base.ElementEffect();
        GameObject newCrystalGrenade = Instantiate(CrystalGrenade, shootingTranform.position, Camera.main.transform.rotation);
        newCrystalGrenade.GetComponent<CrystalGrenadeProj>().SetVars(projectileSpeed, damage * (damageMultiplier + elementData.crystalDamageMultiplier), timeToExplode, gravity, explosionRange, explosionDamage, attackTypes);
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
