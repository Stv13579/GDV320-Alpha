using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapRoom : MonoBehaviour
{   
    [SerializeField]
    bool isShop, isBreak, isBoss;

    [SerializeField]
    Image roomIcon;

    [SerializeField]
    Sprite bossIcon, shopIcon, breakIcon;
    public void SetAsShop() { isShop = true; roomIcon.sprite = shopIcon; roomIcon.enabled = true; }
    public void SetAsBreak() { isBreak = true; roomIcon.sprite = breakIcon; roomIcon.enabled = true; }
    public void SetAsBoss() { isBoss = true; roomIcon.sprite = bossIcon; roomIcon.enabled = true; }

    [SerializeField]
    Color visitedColour, occupiedColour, unexploredColour;
    public void SetVisited() { GetComponent<Image>().color = visitedColour; }
    public void SetOccupied(Vector3 oldRoomPos) { GetComponent<Image>().color = occupiedColour;

        //Move all the rooms on the minimap so this one is central
        Vector3 amountToMove = oldRoomPos - transform.localPosition;

        foreach (Transform mmRoom in GameObject.Find("MiniMap").transform)
        {
            if(mmRoom.name != "RoomIcon(Clone)")
            {

            }
            mmRoom.localPosition += amountToMove;
        }

    }

    
    public void SetUnexplored() { GetComponent<Image>().color = unexploredColour; }

    public bool GetOccupied() { return GetComponent<Image>().color == occupiedColour; }

}
