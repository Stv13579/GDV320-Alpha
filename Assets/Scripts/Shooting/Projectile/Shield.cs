using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField]
    EnergyElement energyElement;
    private void OnTriggerEnter(Collider other)
    {
        if (energyElement.GetUseShield() == true)
        {
            if (other.gameObject.layer == 8 || other.GetComponentInParent<BaseEnemyClass>() ||
                other.gameObject.tag == "Enemy")
            {
                if (!energyElement.GetContainedEnemies().Contains(other.gameObject))
                {
                    energyElement.GetContainedEnemies().Add(other.gameObject);
                    BaseEnemyClass enemy = other.gameObject.GetComponentInParent<BaseEnemyClass>();
                    StatModifier.AddModifier(enemy.GetDamageStat().multiplicativeModifiers, new StatModifier.Modifier(0.0f, "Shield"));
                    StatModifier.UpdateValue(enemy.GetDamageStat());
                    if(energyElement.GetUpgraded() == true)
                    {
                        StatModifier.StartAddModifierTemporary(enemy, enemy.GetSpeedStat().multiplicativeModifiers, new StatModifier.Modifier(0.0f, "Stunned"), 5.0f);
                    }
                    else
                    {
                        StatModifier.StartAddModifierTemporary(enemy, enemy.GetSpeedStat().multiplicativeModifiers, new StatModifier.Modifier(0.0f, "Stunned"), 3.0f);
                    }
	                enemy.EnableStun();
                    energyElement.SetMaterialChanger(1.0f);
                    energyElement.GetEnergyShield().transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_ShieldDamage", energyElement.GetMaterialChanger());
                    energyElement.SetBeenHit(true);
                }
            }
            if (other.gameObject.layer == 22 || other.GetComponent<BaseRangedProjectileScript>())
            {
                Destroy(other.gameObject);
                energyElement.SetMaterialChanger(1.0f);
                energyElement.GetEnergyShield().transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_ShieldDamage", energyElement.GetMaterialChanger());
                energyElement.SetBeenHit(true);
            }
        }
    }
    
	IEnumerator StopStun(BaseEnemyClass enemy, float time)
	{
		yield return new WaitForSeconds(time);
		enemy.DisableStun();
	}
}
