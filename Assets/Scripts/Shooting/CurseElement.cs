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

    List <GameObject> targetToCurseList = new List<GameObject>();
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
    public List<GameObject> GetTargetToCurseList() { return targetToCurseList; }
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
        for (int i = 0; i < targetToCurseList.Count; i ++)
        {
            if (targetToCurseList[i] && !targetToCurseList[i].GetComponentInParent<BaseEnemyClass>().GetDeathTriggers().Contains(DeathEffect))
            {
                //Attach an effect to it
                targetToCurseList[i].GetComponentInParent<BaseEnemyClass>().SetCursed(true);
                //Instantiate(curseVFX, targetToCurseList[i].transform.root);
                if(upgraded)
                {
                    targetToCurseList[i].GetComponentInParent<BaseEnemyClass>().GetHitTriggers().Add(HitEffect);
                }
                else
                {
                    targetToCurseList[i].GetComponentInParent<BaseEnemyClass>().GetDeathTriggers().Add(DeathEffect);
                }
                playerClass.ChangeMana(-manaCost, manaTypes);

            }
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
    }

    public void HitEffect(BaseEnemyClass enem, List<BaseEnemyClass.Types> type)
    {
        Collider[] hitColls = null;
        if (enem)
        {
            hitColls = Physics.OverlapSphere(enem.transform.position, explosionRange);
        }
        if (hitColls != null)
        {
            foreach (Collider hit in hitColls)
            {
                if (hit.tag == "Enemy" && hit.GetComponentInParent<BaseEnemyClass>())
                {
                    if(hit.GetComponentInParent<BaseEnemyClass>() == enem)
                    {
                        hit.gameObject.GetComponentInParent<BaseEnemyClass>().TakeDamage(damage * (damageMultiplier + elementData.crystalDamageMultiplier) * 0.1f, types, 1, false);
                    }
                    else
                    {
                        hit.gameObject.GetComponentInParent<BaseEnemyClass>().TakeDamage(damage * (damageMultiplier + elementData.crystalDamageMultiplier) * 0.1f, types);
                    }
                }
            }
        }


        Instantiate(curseDeath, enem.transform.position, Quaternion.identity);
        if (audioManager)
        {
            audioManager.StopSFX("Curse Explosion");
            audioManager.PlaySFX("Curse Explosion");
        }
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
                
                if(targetToCurseList.Contains(rayHit.collider.gameObject))
                {

                }
                else if (rayHit.collider.gameObject.GetComponent<EnemyShield>())
                {

                }
                else
                {
                    targetToCurse = rayHit.collider.gameObject;
	                targetToCurse.GetComponentInParent<BaseEnemyClass>().Targetted(true, outlineColour);
                    targetToCurseList.Add(targetToCurse);
                }
            }
        }
        else
        {
            for (int i = 0; i < targetToCurseList.Count; i++)
            {
                targetToCurseList[i].GetComponentInParent<BaseEnemyClass>().Targetted(false, new Color(0, 0, 0));
            }
        }
        //for (int i = 0; i < targetToCurseList.Count; i++)
        //{
        //    if (targetToCurseList[i].GetComponentInParent<BaseEnemyClass>().GetHealth() <= 0)
        //    {
        //        targetToCurseList[i].GetComponentInParent<BaseEnemyClass>().SetCursed(false);
        //        targetToCurseList[i].GetComponentInParent<BaseEnemyClass>().GetDeathTriggers().Remove(DeathEffect);
        //        targetToCurseList.Remove(targetToCurseList[i]);
        //    }
        //}
        if (Input.GetKey(KeyCode.Mouse0) && !playerHand.GetCurrentAnimatorStateInfo(4).IsName("Idle"))
        {
            playerHand.ResetTrigger("CurseCast");
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

    public override void Upgrade()
    {
        base.Upgrade();

        
    }
}
