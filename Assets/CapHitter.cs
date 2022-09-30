using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapHitter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GetComponentInParent<ShieldEnemyScript>().CapDamage();
            GetComponentInParent<ShieldEnemyScript>().capHitter.SetActive(false);
        }
    }
}
