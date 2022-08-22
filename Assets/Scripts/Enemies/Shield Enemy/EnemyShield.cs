using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : BaseEnemyClass
{
    [SerializeField]
    protected List<Types> restoration;
    [SerializeField]
    Collider[] attackCollider, defenseCollider;
    bool attacking = false;
    public override void TakeDamage(float damageToTake, List<Types> attackTypes, float extraSpawnScale = 1)
    {
        GameObject hitSpn = Instantiate(hitSpawn, transform.position, Quaternion.identity);
        Vector3 scale = hitSpn.transform.lossyScale * extraSpawnScale;
        hitSpn.transform.localScale = scale;
        float multiplier = 1;
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
        currentHealth -= (damageToTake * multiplier) * damageResistance - damageThreshold;
        Debug.Log("Shield damage");
    }

    public void StartAttack()
    {
        foreach(Collider col in attackCollider)
        {
            col.enabled = true;
        }
        foreach (Collider col in defenseCollider)
        {
            col.enabled = false;
        }
        attacking = true;
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
        attacking = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(attacking)
        {
            if(other.gameObject.GetComponent<PlayerClass>())
            {
                other.gameObject.GetComponent<PlayerClass>().ChangeHealth(-damageAmount);
            }
        }
    }
}
