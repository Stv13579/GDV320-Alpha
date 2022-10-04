using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : BaseEnemyClass
{
    [SerializeField]
    protected List<Types> restoration;
    [SerializeField]
    Collider[] attackCollider, defenseCollider;
    public override void TakeDamage(float damageToTake, List<Types> attackTypes, float extraSpawnScale = 1, bool applyTriggers = true)
    {
        hitSpawn.GetComponent<ParticleSystem>().Clear();
        hitSpawn.GetComponent<ParticleSystem>().Play();
        float multiplier = 1;

        if (applyTriggers)
        {
            foreach (Item item in playerClass.GetHeldItems())
            {
                item.OnHitTriggers(this, attackTypes);
            }

            //Call all hit triggers; effects which trigger whenever the enemy is hit
            foreach (HitTrigger hTrigs in hitTriggers)
            {
                hTrigs(this, attackTypes);
            }
        }

        foreach (Types type in attackTypes)
        {
            foreach (Types weak in weaknesses)
            {
                if (weak == type)
                {
                    multiplier *= 2 * prophecyManager.prophecyWeaknessMulti;
                }
            }
            foreach (Types resist in resistances)
            {
                if (resist == type)
                {
                    multiplier *= 0.5f * prophecyManager.prophecyResistMulti;
                }
            }
            foreach (Types restore in restoration)
            {
                if (restore == type)
                {
                    multiplier *= -1;
                }
            }
        }
        currentHealth -= (damageToTake * multiplier) * damageResistance;

        if(uiScript)
        {
            StopCoroutine(uiScript.HitMarkerShield());
            StartCoroutine(uiScript.HitMarkerShield());
        }

        Death();

    }

    public void StartAttack()
    {
        foreach (Collider col in attackCollider)
        {
            col.enabled = true;
        }
        foreach (Collider col in defenseCollider)
        {
            col.enabled = false;
        }
    }

    public void EndAttack()
    {
        foreach (Collider col in attackCollider)
        {
            col.enabled = false;
        }
        foreach (Collider col in defenseCollider)
        {
            col.enabled = true;
        }

    }



    public override void Death()
    {
        if (isDead)
        {
            return;
        }
        if (currentHealth <= 0)
        {
            //Normally do death animation/vfx, might even fade alpha w/e before deleting.

            GetComponentInParent<ShieldEnemyScript>().GuardBreak();
            //Destroy(gameObject);

            currentHealth = maxHealth;
            
        }
    }
}
