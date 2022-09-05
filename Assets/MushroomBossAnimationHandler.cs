using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBossAnimationHandler : MonoBehaviour
{
	public void Attack()
	{
		this.transform.parent.gameObject.GetComponent<MushroomBossScript>().SpawnSporeCloud();
		Debug.Log("Anim triggered");
	}
}
