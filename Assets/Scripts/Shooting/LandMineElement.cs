using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineElement : BaseElementClass
{
    [SerializeField]
    private GameObject landMineProjectile;

    [SerializeField]
    private float damage;

    public float damageMultiplier = 1;

    [SerializeField]
    private float explosiveRadius;

    [SerializeField]
    private float lifeTimer;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    public override void ElementEffect()
    {
        base.ElementEffect();
        Vector3 camLook = Camera.main.transform.forward;
        camLook = new Vector3(camLook.x, 0.0f, camLook.z).normalized;
        GameObject newLandMine = Instantiate(landMineProjectile, shootingTranform.position + camLook, Quaternion.identity);
        newLandMine.GetComponent<LandMineProj>().SetVars(damage * damageMultiplier, lifeTimer, explosiveRadius, attackTypes);
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
