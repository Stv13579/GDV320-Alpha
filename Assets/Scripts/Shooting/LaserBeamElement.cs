using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamElement : BaseElementClass
{
    [SerializeField]
    private GameObject laserBeam;

    [SerializeField]
    private PlayerMovement playerMovement;
    protected override void Start()
    {
        base.Start();
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        // same check as the energy shield need to check if player are in theses states
        if(playerHand.GetCurrentAnimatorStateInfo(2).IsName("StartLaser") ||
           playerHand.GetCurrentAnimatorStateInfo(2).IsName("HoldLaser"))
        {
            DeactivateLaser();
        }
        else
        {
            
        }
    }

    public void DeactivateLaser()
    {
        if (!Input.GetKey(KeyCode.Mouse0) || !PayCosts(10 * Time.deltaTime))
        {
            //if left click is up or if player has no mana
            // stop the laser beam
            laserBeam.SetActive(false);
            playerHand.SetTrigger("LaserBeamStopCast");
            audioManager.Stop("Laser Beam");
            laserBeam.GetComponentInChildren<LaserBeam>().isHittingObj = false;
            Multiplier.RemoveMultiplier(playerMovement.movementMultipliers, new Multiplier(0.25f, "Laser"), playerMovement.movementMulti);
        }            
    }
    public override void ElementEffect()
    {
        base.ElementEffect();
        laserBeam.SetActive(true);
        laserBeam.GetComponentInChildren<LaserBeam>().SetVars(damage * (damageMultiplier + elementData.fireDamageMultiplier), attackTypes);
        Multiplier.AddMultiplier(playerMovement.movementMultipliers, new Multiplier(0.25f, "Laser"), playerMovement.movementMulti);
    }
    public override void LiftEffect()
    {
        base.LiftEffect();
        DeactivateLaser();
    }
    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }

    protected override void StartAnims(string animationName, string animationNameAlt = null)
    {
        base.StartAnims(animationName);

        playerHand.ResetTrigger("LaserBeamStopCast");

        playerHand.SetTrigger(animationName);
    }
    protected override bool PayCosts(float modifier = 1)
    {
        //Override of paycosts so that mana is only subtracted at then end, in case the cast is cancelled
        if (playerClass.ManaCheck(manaCost * modifier, manaTypes))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
