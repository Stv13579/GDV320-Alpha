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

            Vector3 cachedPos = new Vector3();

            foreach (Transform mmRoom in GameObject.Find("MiniMap").transform)
            {
                if (mmRoom.name != "RoomIcon(Clone)")
                {
                    continue;
                }
                if (mmRoom.GetComponent<MinimapRoom>().GetOccupied())
                {
                    mmRoom.GetComponent<MinimapRoom>().SetVisited();
                    cachedPos = mmRoom.transform.localPosition;
                }    
            }

            GetComponentInParent<Room>().minimapRoom.SetOccupied(cachedPos);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
        }
    }
}
