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
        playerHandL.SetTrigger("LaserStopCast");
        audioManager.Stop("Laser Beam");
        laserBeam.GetComponentInChildren<LaserBeam>().isHittingObj = false;
        playerMovement.RemoveMovementMultiplier(new PlayerMovement.movementMultiSource(0.25f, "Laser"));

    }
    public override void ElementEffect()
    {
        base.ElementEffect();
        laserBeam.SetActive(true);
        laserBeam.GetComponentInChildren<LaserBeam>().SetVars(damage * damageMultiplier, attackTypes);
        playerMovement.AddMovementMultiplier(new PlayerMovement.movementMultiSource(0.25f, "Laser"));
    }
    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }

    protected override void StartAnims(string animationName)
    {
        base.StartAnims(animationName);

        playerHand.ResetTrigger("LaserStopCast");
        playerHandL.ResetTrigger("LaserStopCast");

        playerHand.SetTrigger(animationName);
        playerHandL.SetTrigger(animationName);
    }
}
