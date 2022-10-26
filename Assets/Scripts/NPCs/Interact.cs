using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    bool inRange = false;
    public GameObject npcTalkUI;
    public GameObject npcOfferingUI;
    public GameObject menu;
    GameObject instantiatedMenu;
    [HideInInspector]
    public GameObject instantiatedTalkUI;
    [HideInInspector]
    public GameObject instantiatedOfferingUI;
    bool inUI = false;
    PlayerMovement playerMove;
    PlayerLook playerLook;
    Shooting shooting;
    GameObject gameUI;
    [HideInInspector]
    public NPC npc;
    public bool canInteract = true;
    public bool canSeeOfferings = true;

    void Start()
    {
	    gameUI = GameplayUI.GetGameplayUI().gameObject;
        npc = GetComponent<NPC>();
    }
    void Update()
    {
        if(npc)
        {
            npc.AssessInteract();
        }
        if(!canInteract)
        {

            LeaveUI();
            inRange = false;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            Destroy(this);
            return;
        }

        if (Input.GetKeyDown(KeyCode.T) && inRange && !inUI)
        {

            if (instantiatedMenu == null)
            {
                instantiatedMenu = Instantiate(menu);
                if(npc)
                {
	                instantiatedMenu.GetComponent<NPCUI>().SetNPC(this);
                }

            }
            else
            {
                instantiatedMenu.SetActive(true);
            }

            inUI = true;
            //Lock the players actions, enable the shop
            playerLook.ToggleCursor();
            //playerMove.ableToMove = true;
            playerLook.SetAbleToMove(false);
            shooting.SetAbleToShoot(false);
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameUI.SetActive(false);

            //Set focus
            playerLook.SetFocus(true);

        }
        if (playerMove != null)
        {
            this.gameObject.transform.GetChild(0).LookAt(new Vector3(playerMove.gameObject.transform.position.x, this.gameObject.transform.GetChild(0).position.y, playerMove.gameObject.transform.position.z));
        }


        if (Input.GetKeyDown(KeyCode.Escape) && inRange && inUI)
        {

            if (instantiatedTalkUI != null)
            {
                instantiatedTalkUI.GetComponent<NPCUI>().Close();
            }

            if (instantiatedOfferingUI != null)
            {
                instantiatedOfferingUI.GetComponent<NPCUI>().Close();
            }

            if (instantiatedMenu != null)
            {
                instantiatedMenu.GetComponent<NPCUI>().Close();
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerClass>())
        {
	        gameUI = GameplayUI.GetGameplayUI().gameObject;

            inRange = true;
            playerMove = other.gameObject.GetComponent<PlayerMovement>();
            playerLook = other.gameObject.GetComponent<PlayerLook>();
            shooting = other.gameObject.GetComponent<Shooting>();
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            LeaveUI();
            inRange = false;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            

        }
    }

    public void LeaveUI()
    {
        if(npc)
        {
            npc.AssessInteract();
        }
        //Unlock the players actions, disable the shop
        inUI = false;
        if(instantiatedTalkUI != null)
        {
            instantiatedTalkUI.SetActive(false);
        }

        if (instantiatedOfferingUI != null)
        {
            instantiatedOfferingUI.SetActive(false);
        }

        if(instantiatedMenu != null)
        {
            instantiatedMenu.SetActive(false);
        }

        playerLook.ForceLockCursor();
        playerMove.SetAbleToMove(true);
        playerLook.SetAbleToMove(true);
        shooting.SetAbleToShoot(true);
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameUI.SetActive(true);

        playerLook.SetFocus(false);

    }

}
