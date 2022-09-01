﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidCloudElement : BaseElementClass
{
    //Creates a cloud of acid, which deals damage over time to enemies within the cloud

    [SerializeField]
    private GameObject cloudProj;

    [SerializeField]
    private float cloudSize;

    [SerializeField]
    private float cloudDuration;

    [SerializeField]
    private float damageTicker;

    protected override void Update()
    {
        base.Update();
    }
    public override void ElementEffect()
    {
        base.ElementEffect();
        //Instantiate an acid cloud object in the direction the player is looking
        Vector3 camLook = Camera.main.transform.forward;
        camLook = new Vector3(camLook.x, 0.0f, camLook.z).normalized;
        GameObject cloud = Instantiate(cloudProj, shootingTranform.position + (camLook * 3), Quaternion.identity);
        cloud.transform.GetChild(1).gameObject.GetComponent<AcidCloud>().SetVars(damage * (damageMultiplier + elementData.fireDamageMultiplier) , cloudSize, cloudDuration, attackTypes, damageTicker);
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
}
