using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseElement : BaseElementClass
{
    bool targeting = false;

    [SerializeField]
    LayerMask curseTargets;

    [SerializeField]
    float range;

    GameObject targetToCurse;

    [SerializeField]
    GameObject curseVFX;

    [SerializeField]
    float explosionRange;

    [SerializeField]
    List<BaseEnemyClass.Types> types;

    [SerializeField]
    GameObject curseDeath;

    [SerializeField]
    Color outlineColour;

    protected override void StartAnims(string animationName, string animationNameAlt = null)
    {
        base.StartAnims(animationName);

        playerHand.SetTrigger(animationName);
        targeting = true;

    }
    public override void ElementEffect()
    {
        base.ElementEffect();
        if (audioManager)
        {
            audioManager.StopSFX(shootingSoundFX);
            audioManager.PlaySFX(otherShootingSoundFX);
        }
        targeting = false;
        //curse the target
        //Give it a death trigger
	    if (targetToCurse && !targetToCurse.GetComponentInParent<BaseEnemyClass>().GetDeathTriggers().Contains(DeathEffect))
        {
		    //Attach an effect to it
		    targetToCurse.GetComponentInParent<BaseEnemyClass>().SetCursed();
		    Instantiate(curseVFX, targetToCurse.transform.root);
		    targetToCurse.GetComponentInParent<BaseEnemyClass>().GetDeathTriggers().Add(DeathEffect);
            playerClass.ChangeMana(-manaCost, manaTypes);
        }
    }
    
    public void DeathEffect(GameObject temp)
    {
        Collider[] hitColls = null;
        if (temp)
        {
            hitColls = Physics.OverlapSphere(temp.transform.position, explosionRange);
        }
        if(hitColls != null)
        {
            foreach (Collider hit in hitColls)
            {
	            if (hit.tag == "Enemy" && hit.GetComponentInParent<BaseEnemyClass>())
                {
		            hit.gameObject.GetComponentInParent<BaseEnemyClass>().TakeDamage(damage * (damageMultiplier + elementData.crystalDamageMultiplier), types);
                }
            }
        }


        Instantiate(curseDeath, temp.transform.position, Quaternion.identity);
        if (audioManager)
        {
            audioManager.StopSFX("Curse Explosion");
            audioManager.PlaySFX("Curse Explosion");
        }
        Debug.Log("Explodded");
    }

    public override void LiftEffect()
    {
        base.LiftEffect();

        playerHand.SetTrigger("CurseStopCast");
    }

    protected override void Update()
    {
        base.Update();

        if(targeting)
        {
            RaycastHit rayHit;

            if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out rayHit, range, curseTargets))
            {
                
                if(targetToCurse == rayHit.collider.gameObject)
                {

                }
                else
                {

                    if (targetToCurse)
                    {
	                    targetToCurse.GetComponentInParent<BaseEnemyClass>().Targetted(false, new Color(0, 0, 0));
                    }
                    if(!rayHit.collider.gameObject.GetComponent<EnemyShield>())
                    {
                        targetToCurse = rayHit.collider.gameObject;
	                    targetToCurse.GetComponentInParent<BaseEnemyClass>().Targetted(true, outlineColour);
                    }

                }
                


            }
        }
        else
        {
            if (targetToCurse)
            {
	            targetToCurse.GetComponentInParent<BaseEnemyClass>().Targetted(false, new Color(0, 0, 0));
            }
        }
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
