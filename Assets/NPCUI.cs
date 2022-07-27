using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCUI : MonoBehaviour
{
    [HideInInspector]
    public Interact NPC;

    public virtual void Close()
    {

        NPC.LeaveUI();
    }

}
