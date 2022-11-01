using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBob : MonoBehaviour
{
    [SerializeField]
    BaseFlyingEnemyScript flyingScript;

    private void OnTriggerEnter(Collider other)
    {
        flyingScript.HitFloor();
    }
}
