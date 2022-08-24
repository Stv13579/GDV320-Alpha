using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && other.gameObject.transform != transform.parent)
        {
            GetComponentInParent<BaseEnemyClass>().GetBounceList().Add(other.gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && other.gameObject.transform != transform.parent)
        {
            GetComponentInParent<BaseEnemyClass>().GetBounceList().Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && other.gameObject.transform != transform.parent)
        {
            GetComponentInParent<BaseEnemyClass>().GetBounceList().Remove(other.gameObject);
        }

    }
}
