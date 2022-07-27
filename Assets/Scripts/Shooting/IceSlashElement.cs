using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSlashElement : BaseElementClass
{
    [SerializeField]
    private GameObject iceSlash;

    [SerializeField]
    private float damage;
    public float damageMultiplier = 1;

    [SerializeField]
    private float projectileSpeed;


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void ElementEffect()
    {
        base.ElementEffect();
        GameObject iceSlashTemp = Instantiate(iceSlash, shootingTranform.position, Camera.main.transform.rotation);

    }

    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }

    protected override void StartAnims(string animationName)
    {
        base.StartAnims(animationName);

        playerHand.SetTrigger(animationName);
        playerHandL.SetTrigger(animationName);
    }
}
