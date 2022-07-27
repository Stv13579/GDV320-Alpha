using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{

    public bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            triggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            
        }
    }
}
