using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAnimationHandler : MonoBehaviour
{
	public void Attack()
	{
		this.transform.parent.gameObject.GetComponent<RangedEnemyScript>().Attack();
	}
	
	public void Burrow()
	{
		this.transform.parent.gameObject.GetComponent<RangedEnemyScript>().StartBurrow();
	}
}
