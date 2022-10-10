using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIceSlash : MonoBehaviour
{
    [SerializeField]
    LayerMask environment;

    // if this box collider hits the environment layermask the destroy iceslash
    void OnTriggerEnter(Collider other)
    {
        if(((1<<other.gameObject.layer) & environment) != 0 || other.GetComponent<EnemyShield>())
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
