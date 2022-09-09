using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRunScript : MonoBehaviour
{
    public void StartRun()
    {
        Shooting player = FindObjectOfType<Shooting>();

        if(player.GetCatalystElements().Contains(player.blankElement) || player.GetPrimaryElements().Contains(player.blankElement))
        {
            Debug.Log("Choose a loadout first!");
            return;
        }

        GameObject.Find("LoadoutObject").GetComponent<Interact>().LeaveUI();

        FindObjectOfType<EndLevel>().EndCurrentLevel();
        
    }
}
