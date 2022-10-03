using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    [SerializeField]
    GameObject ignore;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && other.gameObject.transform != transform.parent)
        {
            if(ignore && other.gameObject == ignore)
            {
                return;
            }
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
