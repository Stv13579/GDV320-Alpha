using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPool : MonoBehaviour
{
    //[SerializeField]
    //Transform crystalSlime, fireSlime, waterSlime,
    //    crystalRanged, fireRanged, waterRanged,
    //    crystalShield, fireShield, waterShield,
    //    crystalFlying, fireFlying, waterFlying;
    [SerializeField]
    List<Transform> pools;

    public GameObject GetReadiedEnemy(GameObject enemyType)
    {
        GameObject selectedEnemy = null;

        foreach(Transform trans in pools)
        {
            //Find the right pool of enemy objects
            if(trans.GetChild(0).name == enemyType.name)
            {
                //iterate through each object in that pool to find an inactive one
                foreach (Transform tra in trans)
                {
                    if(!tra.gameObject.activeInHierarchy)
                    {
                        selectedEnemy = tra.gameObject; 
                    }
                    
                }
            }
        }

        return selectedEnemy;
    }
}
