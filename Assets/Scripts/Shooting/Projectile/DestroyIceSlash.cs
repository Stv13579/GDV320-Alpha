using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIceSlash : MonoBehaviour
{
    [SerializeField]
    private LayerMask environment;

    // if this box collider hits the environment layermask the destroy iceslash
    private void OnTriggerEnter(Collider other)
    {
        if(((1<<other.gameObject.layer) & environment) != 0)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
