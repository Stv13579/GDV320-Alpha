using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroticElement : BaseElementClass
{
    //[SerializeField]
    //private GameObject chargeVFX;

    [SerializeField]
    private GameObject test;

    [SerializeField]
    private bool isTargeting = false;

    private GameObject targetToSlow;

    [SerializeField]
    private PlayerLook lookScript;

    [SerializeField]
    private LayerMask NecroticTarget;

    [SerializeField]
    private float rayCastRange;

    [SerializeField]
    Color outlineColour;

    // maybe
    [SerializeField]
    private float maxSlowTimer;

    [SerializeField]
    private float currentSlowTimer;

    protected override void StartAnims(string animationName)
    {
        base.StartAnims(animationName);

        playerHand.ResetTrigger("SoulStopCast");
        playerHand.SetTrigger(animationName);

        audioManager.Play("Soul Element");
        //Instantiate(chargeVFX, playerClass.gameObject.GetComponent<Shooting>().GetRightOrbPos());

        isTargeting = true;
    }
    public override void ElementEffect()
    {
        base.ElementEffect();
        isTargeting = false;

        if(targetToSlow && targetToSlow.GetComponent<BaseEnemyClass>().moveSpeedMulti != 0.5f)
        {
            Instantiate(test, targetToSlow.transform);
            targetToSlow.GetComponent<BaseEnemyClass>().AddMovementMultiplier(0.5f);
        }
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        // if istargeting = true
        // player can target and enemy
        // this turns on when helded
        if(isTargeting)
        {
            RaycastHit targetRayCast;
            
            if(Physics.Raycast(lookScript.GetCamera().transform.position, lookScript.GetCamera().transform.forward, out targetRayCast, rayCastRange, NecroticTarget))
            {
                // if facing a target thats already targeted
                if(targetToSlow == targetRayCast.collider.gameObject)
                {

                }
                else
                {
                    // else if facing a new target
                    // take away target off old target
                    // and put target on new target
                    if(targetToSlow)
                    {
                        targetToSlow.GetComponent<BaseEnemyClass>().Targetted(false, new Color(0, 0, 0));
                    }
                    targetToSlow = targetRayCast.collider.gameObject;
                    targetToSlow.GetComponent<BaseEnemyClass>().Targetted(true, outlineColour);
                }
            }
        }
        else
        {
            if(targetToSlow)
            {
                targetToSlow.GetComponent<BaseEnemyClass>().Targetted(false, new Color(0, 0, 0));
            }
        }
    }
    public override void LiftEffect()
    {
        base.LiftEffect();

        playerHand.SetTrigger("SoulStopCast");

        audioManager.Stop("Soul Element");
        Destroy(playerClass.gameObject.GetComponent<Shooting>().GetRightOrbPos().GetChild(1).gameObject);
    }

    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }
}
