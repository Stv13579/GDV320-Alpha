using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamElement : BaseElementClass
{
    [SerializeField]
    private GameObject laserBeam;

    [SerializeField]
    private PlayerMovement playerMovement;

    private bool useLaserBeam = false;
    protected override void Start()
    {
        base.Start();
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if(useLaserBeam)
        {
            PayCosts(Time.deltaTime);
        }
        // same check as the energy shield need to make this nicer.
        if(!Input.GetKey(KeyCode.Mouse0) && playerHand.GetCurrentAnimatorStateInfo(2).IsName("StartLaser"))
        {
            DeactivateLaser();
        }
    }

    public void DeactivateLaser()
    {
        //if left click is up or if player has no mana
        // stop the laser beam
        laserBeam.SetActive(false);
        useLaserBeam = false;       
        playerHand.SetTrigger("LaserBeamStopCast");
        audioManager.Stop("Laser Beam");
        laserBeam.GetComponentInChildren<LaserBeam>().isHittingObj = false;
        Multiplier.RemoveMultiplier(playerMovement.movementMultipliers , new Multiplier(0.25f, "Laser"), playerMovement.movementMulti);

    }
    public override void ElementEffect()
    {
        base.ElementEffect();
        laserBeam.SetActive(true);
        laserBeam.GetComponentInChildren<LaserBeam>().SetVars(damage * (damageMultiplier + elementData.fireDamageMultiplier), attackTypes);
        Multiplier.AddMultiplier(playerMovement.movementMultipliers, new Multiplier(0.25f, "Laser"), playerMovement.movementMulti);
        useLaserBeam = true;
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
}
