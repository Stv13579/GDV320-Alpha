using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSlashElement : BaseElementClass
{
    [SerializeField]
    private GameObject iceSlash;

    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    private float lifeTimer;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void ElementEffect()
    {
        GameObject iceSlashTemp = Instantiate(iceSlash, shootingTranform.position, Camera.main.transform.rotation);
        iceSlashTemp.GetComponent<IceSlashProj>().SetVars(projectileSpeed, damage * (damageMultiplier + elementData.waterDamageMultiplier), lifeTimer, attackTypes);
        base.ElementEffect();
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
