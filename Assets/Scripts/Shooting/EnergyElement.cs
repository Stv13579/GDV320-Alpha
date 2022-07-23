using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyElement : BaseElementClass
{
    [SerializeField]
    private GameObject chargeVFX;

    public GameObject test;

    [SerializeField]
    private PlayerLook lookScript;
    private void Start()
    {
        base.Start();
        heldCast = true;
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void ElementEffect()
    {
        base.ElementEffect();
        Vector3 camLook = lookScript.GetCamera().transform.forward;
        camLook = new Vector3(camLook.x, 0.0f, camLook.z).normalized;
        GameObject temptest = Instantiate(test, shootingTranform.position + (camLook * 3), Quaternion.identity);

        playerHand.SetTrigger("StopEnergy");
        audioManager.Stop("Energy Element");
        Destroy(playerClass.gameObject.GetComponent<Shooting>().GetRightOrbPos().GetChild(1).gameObject);

    }

    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }

    protected override void StartAnims(string animationName)
    {
        base.StartAnims(animationName);
        playerHand.SetTrigger(animationName);

        audioManager.Play("Energy Element");
        Instantiate(chargeVFX, playerClass.gameObject.GetComponent<Shooting>().GetRightOrbPos());

    }
}
