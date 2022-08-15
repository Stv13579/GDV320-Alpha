using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamElement : BaseElementClass
{
    [SerializeField]
    private GameObject laserBeam;

    [SerializeField]
    private PlayerMovement playerMovement;

    private bool usingLaser;
    protected override void Start()
    {
        base.Start();
        usingLaser = false;
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        // same check as the energy shield need to check if player are in theses states
        if(usingLaser)
        {
            if (!PayCosts(10 * Time.deltaTime) || Input.GetKeyDown(KeyCode.E) ||
               Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Q))
            {
                DeactivateLaser();
            }
        }
    }

    public void DeactivateLaser()
    {
       usingLaser = false;
       laserBeam.SetActive(false);
       playerHand.SetTrigger("LaserBeamStopCast");
       audioManager.Stop("Laser Beam");
       laserBeam.GetComponentInChildren<LaserBeam>().isHittingObj = false;
       playerMovement.movementMulti = Multiplier.RemoveMultiplier(playerMovement.movementMultipliers, new Multiplier(0.25f, "Laser"));
    }
    public override void ElementEffect()
    {
        base.ElementEffect();
        usingLaser = true;
        laserBeam.SetActive(true);
        laserBeam.GetComponentInChildren<LaserBeam>().SetVars(damage * (damageMultiplier + elementData.fireDamageMultiplier), attackTypes);
        playerMovement.movementMulti = Multiplier.AddMultiplier(playerMovement.movementMultipliers, new Multiplier(0.25f, "Laser"));
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
