using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackPool : MonoBehaviour
{
	[SerializeField]
	List<Transform> pools;
	
	static EnemyAttackPool currentPoolObject;
	
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
		currentPoolObject = this;
	}
	
	public static EnemyAttackPool GetAttackPool()
	{
		return currentPoolObject;
	}

	public GameObject GetReadiedAttack(GameObject attackType)
	{
		GameObject selectedAttack = null;

		foreach(Transform trans in pools)
		{
			//Find the right pool of enemy objects
			if(trans.GetChild(0).name == attackType.name)
			{
				//iterate through each object in that pool to find an inactive one
				foreach (Transform tra in trans)
				{
					if(!tra.gameObject.activeInHierarchy)
					{
						selectedAttack = tra.gameObject; 
						return selectedAttack;
					}
                    
				}
				selectedAttack = Instantiate(attackType, trans);
				return selectedAttack;
			}
		}
		return selectedAttack;
	}
}
