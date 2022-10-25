using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalGrenadeElement : BaseElementClass
{
    [SerializeField]
    GameObject CrystalGrenade;

    [SerializeField]
    float projectileSpeed;

    [SerializeField]
    float gravity;

    [SerializeField]
    float timeToExplode;

    [SerializeField]
    float explosionRange;

    [SerializeField]
    float explosionDamage;

    protected override void Start()
    {
        base.Start();
        activatedVFX.SetActive(false);
    }
    protected override void Update()
    {
        base.Update();
        //if this element is turned on turn on indicator
        if (shootingScript.GetComboElements()[shootingScript.GetLeftElementIndex()].comboElements[shootingScript.GetRightElementIndex()] == this
            && shootingScript.GetInComboMode() == true)
        {
            activatedVFX.SetActive(true);
        }
        else
        {
            activatedVFX.SetActive(false);
        }
    }
    // gets called in the animation event triggers
    public override void ElementEffect()
    {
        base.ElementEffect();
        GameObject newCrystalGrenade = Instantiate(CrystalGrenade, shootingTranform.position, Camera.main.transform.rotation);
        newCrystalGrenade.GetComponent<CrystalGrenadeProj>().SetVars(projectileSpeed, damage * (damageMultiplier + elementData.crystalDamageMultiplier), timeToExplode, explosionRange, explosionDamage, attackTypes);
    }
    // gets called in the animation event triggers
    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }
    protected override void StartAnims(string animationName, string animationNameAlt = null)
    {
        base.StartAnims(animationName);

        playerHand.SetTrigger(animationName);
    }
}
