using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    GameObject playerIndicator, startRoom, minimap;


    // Start is called before the first frame update
    void Start()
    {
        Instantiate(playerIndicator, minimap.transform);
        foreach (MinimapRoom mmRoom in FindObjectsOfType<MinimapRoom>())
        {
            if (mmRoom.isShop)
            {
                mmRoom.SetAsShop();
            }
            if (mmRoom.isBreak)
            {
                mmRoom.SetAsBreak();
            }
            if (mmRoom.isBoss)
            {
                mmRoom.SetAsBoss();
            }
            mmRoom.SetMaxDist(60);
            mmRoom.SetUnexplored();
        }
        startRoom.GetComponent<Room>().minimapRoom.SetOccupied(Vector3.zero);

        
    }

    
}
