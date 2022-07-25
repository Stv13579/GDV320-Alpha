using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyElement : BaseElementClass
{
    [SerializeField]
    private GameObject chargeVFX;

    public GameObject energyShield;

    // might change to this method depending if current way works
    private enum shieldState
    {
        shieldUp,
        parrying,
        shieldDown
    }
    private shieldState shieldStateChange;
    private float parryTimer;

    [SerializeField]
    private bool usingEnergyShield;


    protected override void Start()
    {
        base.Start();
        
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

    protected override bool PayCosts(float modifier = 1)
    {
        //Override of paycosts so that mana is only subtracted at then end, in case the cast is cancelled
        if (playerClass.ManaCheck(manaCost * modifier, manaTypes))
        {
            playerClass.ChangeMana(-manaCost * modifier, manaTypes);
            return true;
        }
        else
        {
            playerHand.SetTrigger("NoMana");
            // this section needs fixing will fix later
            Destroy(playerClass.gameObject.GetComponent<Shooting>().GetRightOrbPos().GetChild(1).gameObject);
            return false;
        }
    }
}
