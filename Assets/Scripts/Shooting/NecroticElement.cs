using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroticElement : BaseElementClass
{
    [SerializeField]
    private GameObject necroticVFX;

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

    protected override void StartAnims(string animationName, string animationNameAlt = null)
    {
        base.StartAnims(animationName);

        playerHand.ResetTrigger("NecroticStopCast");
        playerHand.SetTrigger(animationName);

        audioManager.Play("Soul Element");
        isTargeting = true;
    }
    public override void ElementEffect()
    {
        base.ElementEffect();
        isTargeting = false;
        if (targetToSlow && targetToSlow.GetComponent<BaseEnemyClass>() && !targetToSlow.GetComponent<EnemyShield>() &&
            (targetToSlow.GetComponent<BaseEnemyClass>().GetMoveMulti() != 0.5f || targetToSlow.GetComponent<BaseEnemyClass>().GetMoveMulti() != 0.3f))
        {
            playerClass.ChangeMana(-manaCost, manaTypes);
            Instantiate(necroticVFX, targetToSlow.transform);
            targetToSlow.GetComponent<BaseEnemyClass>().SetMoveMulti(Multiplier.AddMultiplier(targetToSlow.GetComponent<BaseEnemyClass>().GetMoveMultis(), new Multiplier(upgraded ? 0.5f : 0.3f, "Necrotic")));
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
            if (shootingScript.GetRightOrbPos().childCount < 2)
            {
                Instantiate(activatedVFX, shootingScript.GetRightOrbPos());
            }
            RaycastHit targetRayCast;
            if (Physics.Raycast(lookScript.GetCamera().transform.position, lookScript.GetCamera().transform.forward, out targetRayCast, rayCastRange, NecroticTarget))
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

        playerHand.SetTrigger("NecroticStopCast");

        audioManager.Stop("Soul Element");

        if (shootingScript.GetRightOrbPos().childCount > 1)
        {
            Destroy(shootingScript.GetRightOrbPos().GetChild(1).gameObject);
        }
    }

    public override void ActivateVFX()
    {
        base.ActivateVFX();
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
