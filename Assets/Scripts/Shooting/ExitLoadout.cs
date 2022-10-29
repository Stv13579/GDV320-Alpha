using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitLoadout : MonoBehaviour
{
    public void Exit()
    {
        GameObject.Find("LoadoutStand").GetComponent<Interact>().LeaveUI();

       

    }
}
