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
        // same check as the energy shield need to check if player has interupted the laser beam
        if(usingLaser)
        {
            if (!PayCosts(manaCost * Time.deltaTime) || !Input.GetKey(KeyCode.Mouse0))
            {
                DeactivateLaser();
            }
        }
    }
    // deactivate laser function
    public void DeactivateLaser()
    {
       usingLaser = false;
       laserBeam.SetActive(false);
       playerHand.SetTrigger("LaserBeamStopCast");
       audioManager.Stop("Laser Beam");
       laserBeam.GetComponentInChildren<LaserBeam>().isHittingObj = false;
       StatModifier.RemoveModifier(playerMovement.GetSpeedStat().multiplicativeModifiers, new StatModifier.Modifier(0.25f, "Laser"));
    }
    // activates the laserBeam
    public override void ElementEffect()
    {
        base.ElementEffect();
        usingLaser = true;
        laserBeam.transform.GetChild(0).localScale = new Vector3(0, 1, 0);
        laserBeam.SetActive(true);
        laserBeam.GetComponentInChildren<LaserBeam>().SetVars(damage * (damageMultiplier + elementData.fireDamageMultiplier), attackTypes);
        StatModifier.AddModifier(playerMovement.GetSpeedStat().multiplicativeModifiers, new StatModifier.Modifier(0.25f, "Laser"));
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
