using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    public BaseEnemyClass.Types weakness;
    public BaseEnemyClass.Types resistance;
    public float maxHealth = 10;
    float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<BaseElementSpawnClass>())
        {
            other.gameObject.GetComponent<BaseElementSpawnClass>().active = false;

        }

    }

    public void DamageShield(float damage, List<BaseEnemyClass.Types> attackTypes)
    { 
        if(attackTypes[0] == weakness)
        {
            currentHealth -= damage * 2;
        }
        else if (attackTypes[0] == resistance)
        {
            currentHealth -= damage * 0.5f;
        }
        else
        {
            currentHealth += 1;
        }
    }

}
