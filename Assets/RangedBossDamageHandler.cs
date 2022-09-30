using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBossDamageHandler : BaseEnemyClass
{
	public override void Update()
	{
		base.Update();
	}
	public override void TakeDamage(float damageToTake, List<Types> attackTypes, float extraSpawnScale = 1, bool applyTriggers = true)
	{
		this.transform.parent.parent.GetComponent<RangedBossScript>().TakeDamage(damageToTake, attackTypes, extraSpawnScale, applyTriggers);
	}
}
