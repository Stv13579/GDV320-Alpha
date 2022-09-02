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
    public void SetOccupied() { GetComponent<Image>().color = occupiedColour; 
    
        
    
    }

    
    public void SetUnexplored() { GetComponent<Image>().color = unexploredColour; }

    public bool GetOccupied() { return GetComponent<Image>().color == occupiedColour; }

}
