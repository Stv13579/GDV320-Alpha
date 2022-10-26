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

    [SerializeField]
    List<GameObject> targetedList = new List<GameObject>();
    protected override void Start()
    {
        base.Start();
        activatedVFX.SetActive(false);
    }

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
        for (int i = 0; i < targetedList.Count; i++)
        {
            if (targetedList[i] && targetedList[i].GetComponentInParent<BaseEnemyClass>() && !targetedList[i].GetComponent<EnemyShield>() &&
                targetedList[i].GetComponentInParent<BaseEnemyClass>().GetDamageResistance() != 2.0f ||
                targetedList[i] && targetedList[i].GetComponentInParent<BaseEnemyClass>() && !targetedList[i].GetComponent<EnemyShield>() &&
                targetedList[i].GetComponentInParent<BaseEnemyClass>().GetDamageResistance() != 3.0f)
            {
                if (targetedList[i].GetComponentInParent<BaseEnemyClass>().GetDamageResistance() != 2.0f || targetedList[i].GetComponentInParent<BaseEnemyClass>().GetDamageResistance() != 3.0f)
                {
                    playerClass.ChangeMana(-manaCost * (upgraded ? 1 : 0.5f) * targetedList.Count, manaTypes);
                    targetedList[i].GetComponentInParent<BaseEnemyClass>().SetWithered(true);
                    targetedList[i].GetComponentInParent<BaseEnemyClass>().SetDamageResistance(targetedList[i].GetComponentInParent<BaseEnemyClass>().GetDamageResistance() * (upgraded ? 3.0f : 2.0f));
                }
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
            activatedVFX.SetActive(true);
            RaycastHit targetRayCast;
            if (Physics.Raycast(lookScript.GetCamera().transform.position, lookScript.GetCamera().transform.forward, out targetRayCast, rayCastRange, NecroticTarget))
            {
                // if facing a target thats already targeted
                if(targetedList.Contains(targetRayCast.collider.gameObject))
                {

                }
                // if facing a shield
                else if (targetRayCast.collider.gameObject.GetComponent<EnemyShield>())
                {
                	
                }
                else
                {
                    // else if facing a new target
                    // and put target on new target
                    targetToSlow = targetRayCast.collider.gameObject;
	                targetToSlow.GetComponentInParent<BaseEnemyClass>().Targetted(true, outlineColour);
                    targetedList.Add(targetToSlow);
                }
            }
        }
        else
        {
            for (int i = 0; i < targetedList.Count; i++)
            {
                targetedList[i].GetComponentInParent<BaseEnemyClass>().Targetted(false, new Color(0, 0, 0));
            }
        }

        //resets the nercrotic since object pooling
        for (int i = 0; i < targetedList.Count; i++)
        {
            if (targetedList[i].GetComponentInParent<BaseEnemyClass>().GetHealth() <= 0)
            {
                targetedList[i].GetComponentInParent<BaseEnemyClass>().SetWithered(false);
                targetedList[i].GetComponentInParent<BaseEnemyClass>().SetDamageResistance(1);
                targetedList.Remove(targetedList[i]);
            }
        }
        if(Input.GetKey(KeyCode.Mouse1) && !playerHand.GetCurrentAnimatorStateInfo(3).IsName("Idle"))
        {
            playerHand.ResetTrigger("NecroticCast");
        }
    }
    public override void LiftEffect()
    {
        base.LiftEffect();

        playerHand.SetTrigger("NecroticStopCast");

        activatedVFX.SetActive(false);
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
