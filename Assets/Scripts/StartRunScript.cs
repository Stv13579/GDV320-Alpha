using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRunScript : MonoBehaviour
{
    public void StartRun()
    {
        Shooting player = FindObjectOfType<Shooting>();

        if(player.GetCatalystElements().Contains(player.GetBlankElement()) || player.GetPrimaryElements().Contains(player.GetBlankElement()))
        {
            Debug.Log("Choose a loadout first!");
            return;
        }

        GameObject.Find("LoadoutObject").GetComponent<Interact>().LeaveUI();

        FindObjectOfType<EndLevel>().EndCurrentLevel();
        
    }
}
