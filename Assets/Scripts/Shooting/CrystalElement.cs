using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalElement : BaseElementClass
{
    [SerializeField]
    private GameObject crystalProjectile;

    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    private AnimationCurve damageCurve;

    [SerializeField]
    private float lifeTimer;

    [SerializeField]
    private float damageLimit;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void ElementEffect()
    {
        base.ElementEffect();
        // for loop to instaniate it 5 times
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 2; j++)
            {

                GameObject newCrystalPro = Instantiate(crystalProjectile, shootingTranform.position, Camera.main.transform.rotation);
                //changes the angle of where they are being fired to
                newCrystalPro.transform.RotateAround(shootingTranform.position, Camera.main.transform.up, 3.0f * i - 5.0f);
                newCrystalPro.transform.RotateAround(shootingTranform.position, Camera.main.transform.right, 3.0f * j - 5.0f);
                // setting the varibles from CrystalProj script
                newCrystalPro.GetComponent<CrystalProj>().SetVars(projectileSpeed, damage * (damageMultiplier + elementData.crystalDamageMultiplier), damageCurve, lifeTimer, attackTypes, damageLimit);
            }
        }
        //RaycastHit hit;
        //Physics.Raycast(this.gameObject.transform.position, Camera.main.transform.forward, out hit, 100, shootingIgnore);
        //if (hit.collider)
        //    {
        //    crystalProjectile.transform.LookAt(hit.point);
        //}
    }

    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }

    protected override void StartAnims(string animationName, string animationNameAlt = null)
    {
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
