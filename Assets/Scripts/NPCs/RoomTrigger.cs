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
            GetComponentInParent<Room>().visited = true;
            triggered = true;

            foreach (Transform mmRoom in GameObject.Find("MiniMap").transform)
            {
                if(mmRoom.GetComponent<MinimapRoom>().GetOccupied())
                {
                    mmRoom.GetComponent<MinimapRoom>().SetVisited();
                }    
            }

            GetComponentInParent<Room>().minimapRoom.SetOccupied();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
        }
    }
}
