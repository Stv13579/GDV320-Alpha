using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRunScript : MonoBehaviour
{
    GameObject portal;
    Shooting player;

    float currentPortalPer;
    AudioManager audioManager;

    EndLevel endLevel;

    public void SetPortal(GameObject nP) { portal = nP; }

    private void Start()
    {
	    player = Shooting.GetShooting();
        portal = GameObject.Find("Model_Environment_Portal_FBX");
        audioManager = AudioManager.GetAudioManager();
        endLevel = FindObjectOfType<EndLevel>();
    }

    private void Update()
    {
        if (player.GetCatalystElements().Contains(player.GetBlankElement()) || player.GetPrimaryElements().Contains(player.GetBlankElement()))
        {
            portal.GetComponent<Animator>().SetTrigger("Close");
            portal.GetComponent<Animator>().ResetTrigger("Open");
            if(audioManager)
            {
                audioManager.StopSFX("Portal Idle");
            }
            endLevel.SetInteractable(true);
        }
        else
        {
            portal.GetComponent<Animator>().SetTrigger("Open");
            portal.GetComponent<Animator>().ResetTrigger("Close");
            if(audioManager)
            {
                audioManager.PlaySFX("Portal Opening", player.transform);
                audioManager.PlaySFX("Portal Idle", player.transform);
            }
            endLevel.SetInteractable(false);
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
        if (audioManager)
        {
            audioManager.StopSFX("Portal Idle");
        }
        FindObjectOfType<EndLevel>().EndCurrentLevel();
        
    }
}
