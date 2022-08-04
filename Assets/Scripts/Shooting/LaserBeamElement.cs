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
        if (!Input.GetKey(KeyCode.Mouse0))
        {
            DeactivateLaser();
        }
        if (playerHand.GetCurrentAnimatorStateInfo(0).IsName("HoldLaser"))
        {
            if (Input.GetKeyUp(KeyCode.Mouse0) || !PayCosts(Time.deltaTime))
            {
                DeactivateLaser();
            }
        }
    }

    public void DeactivateLaser()
    {
        //if left click is up or if player has no mana
        // stop the laser beam
        laserBeam.SetActive(false);

        playerHand.SetTrigger("LaserStopCast");
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
    }
    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }

    protected override void StartAnims(string animationName, string animationNameAlt = null)
    {
        base.StartAnims(animationName);

        playerHand.ResetTrigger("LaserStopCast");

        playerHand.SetTrigger(animationName);
    }
}
