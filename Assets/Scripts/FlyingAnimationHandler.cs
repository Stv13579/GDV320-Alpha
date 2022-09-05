using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAnimationHandler : MonoBehaviour
{
	public void Effect()
	{
		this.transform.parent.gameObject.GetComponent<BaseFlyingEnemyScript>().Effect();
	}
	
}
