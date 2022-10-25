using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPool : MonoBehaviour
{
	[SerializeField]
	List<Transform> pools;
	
	public GameObject GetReadiedMask(BaseEnemyClass.Types element)
	{
		GameObject selectedMask = null;

		foreach(Transform trans in pools)
		{
			//Find the right pool of enemy objects
			if(trans.name == element.ToString())
			{
				//iterate through each object in that pool to find an inactive one
				foreach (Transform tra in trans)
				{
					if(!tra.gameObject.activeInHierarchy)
					{
						selectedMask = tra.gameObject; 
					}
                    
				}
			}
		}

		return selectedMask;
	}
}
