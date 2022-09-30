using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroticElement : BaseElementClass
{
    [SerializeField]
    GameObject necroticVFX;

    [SerializeField]
    bool isTargeting = false;

    GameObject targetToSlow;

    [SerializeField]
    PlayerLook lookScript;

    [SerializeField]
    LayerMask NecroticTarget;

    [SerializeField]
    float rayCastRange;

    [SerializeField]
    Color outlineColour;

    protected override void StartAnims(string animationName, string animationNameAlt = null)
    {
        base.StartAnims(animationName);

        playerHand.ResetTrigger("NecroticStopCast");
        playerHand.SetTrigger(animationName);

        isTargeting = true;
    }
    public override void ElementEffect()
    {
        base.ElementEffect();
        if (audioManager)
        {
            audioManager.StopSFX(shootingSoundFX);
            audioManager.PlaySFX(otherShootingSoundFX);
        }

        isTargeting = false;
	    if (targetToSlow && targetToSlow.GetComponentInParent<BaseEnemyClass>() && !targetToSlow.GetComponent<EnemyShield>())
        {
		    BaseEnemyClass enemy = targetToSlow.GetComponentInParent<BaseEnemyClass>();
            if (enemy.GetDamageResistance() != 2.0f || enemy.GetDamageResistance() != 3.0f)
            {
                playerClass.ChangeMana(-manaCost, manaTypes);
	            Instantiate(necroticVFX, targetToSlow.transform.root);
                enemy.SetDamageResistance(enemy.GetDamageResistance() * (upgraded ? 3.0f : 2.0f));
            }
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
                else if (targetRayCast.collider.gameObject.GetComponent<EnemyShield>())
                {
                	
                }
                else
                {
                    // else if facing a new target
                    // take away target off old target
                    // and put target on new target
                    if(targetToSlow)
                    {
	                    targetToSlow.GetComponentInParent<BaseEnemyClass>().Targetted(false, new Color(0, 0, 0));
                    }
                    targetToSlow = targetRayCast.collider.gameObject;
	                targetToSlow.GetComponentInParent<BaseEnemyClass>().Targetted(true, outlineColour);
                }
            }
        }
        else
        {
            if(targetToSlow)
            {
	            targetToSlow.GetComponentInParent<BaseEnemyClass>().Targetted(false, new Color(0, 0, 0));
            }
        }
    }
    public override void LiftEffect()
    {
        base.LiftEffect();

        playerHand.SetTrigger("NecroticStopCast");

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
