using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyElement : BaseElementClass
{
    [SerializeField]
    private GameObject chargeVFX;

    [SerializeField]
    GameObject energyShield;

    [SerializeField]
    private bool usingEnergyShield;


    protected override void Start()
    {
        base.Start();
        //heldCast = true;
    }

    protected override void Update()
    {
        base.Update();
        if (!Input.GetKey(KeyCode.Mouse1))
        {
            DeactivateEnergyShield();
        }
        if (playerHand.GetCurrentAnimatorStateInfo(0).IsName("Hold"))
        {
            if (Input.GetKeyUp(KeyCode.Mouse1) || !PayCosts(Time.deltaTime))
            {
                DeactivateEnergyShield();
            }
        }
    }

    public void DeactivateEnergyShield()
    {
            usingEnergyShield = false;
            energyShield.SetActive(false);

            playerHand.SetTrigger("StopEnergy");
            audioManager.Stop("Energy Element");
            Destroy(playerClass.gameObject.GetComponent<Shooting>().GetRightOrbPos().GetChild(1).gameObject);
    }

    public override void ElementEffect()
    {
        base.ElementEffect();
        usingEnergyShield = true;
        energyShield.SetActive(true);
    }

    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }

    protected override void StartAnims(string animationName)
    {
        base.StartAnims(animationName);

        playerHand.ResetTrigger("StopEnergy");
        playerHand.SetTrigger(animationName);

        audioManager.Play("Energy Element");
        Instantiate(chargeVFX, playerClass.gameObject.GetComponent<Shooting>().GetRightOrbPos());

    }
}
