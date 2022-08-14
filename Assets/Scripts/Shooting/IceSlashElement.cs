﻿using System.Collections;
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
        base.ElementEffect();
        GameObject iceSlashTemp = Instantiate(iceSlash, shootingTranform.position, Camera.main.transform.rotation);
        iceSlashTemp.GetComponent<IceSlashProj>().SetVars(projectileSpeed, damage * (damageMultiplier + elementData.waterDamageMultiplier), lifeTimer, attackTypes);
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
        }
        else
        {
            playerHand.SetTrigger(animationNameAlt);
        }
    }
}
