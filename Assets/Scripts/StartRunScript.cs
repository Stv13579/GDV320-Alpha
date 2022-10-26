using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRunScript : MonoBehaviour
{
    GameObject portal;
    Shooting player;

    float currentPortalPer;

    public void SetPortal(GameObject nP) { portal = nP; }

    private void Start()
    {
	    player = Shooting.GetShooting();
        portal = GameObject.Find("Model_Environment_Portal_FBX");
    }

    private void Update()
    {
        if (player.GetCatalystElements().Contains(player.GetBlankElement()) || player.GetPrimaryElements().Contains(player.GetBlankElement()))
        {
            portal.GetComponent<Animator>().SetTrigger("Close");
            portal.GetComponent<Animator>().ResetTrigger("Open");
        }
        else
        {
            portal.GetComponent<Animator>().SetTrigger("Open");
            portal.GetComponent<Animator>().ResetTrigger("Close");
           
        }

        
    }

    public void StartRun()
    {
        

        if(player.GetCatalystElements().Contains(player.GetBlankElement()) || player.GetPrimaryElements().Contains(player.GetBlankElement()))
        {
            Debug.Log("Choose a loadout first!");
            return;
        }

        GameObject.Find("LoadoutObject").GetComponent<Interact>().LeaveUI();

        FindObjectOfType<EndLevel>().EndCurrentLevel();
        
    }
}
