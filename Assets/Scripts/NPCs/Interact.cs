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
        gameUI = GameObject.Find("GameplayUI");
        npc = GetComponent<NPC>();
    }
    void Update()
    {
        npc.AssessInteract();


        //if(npc.interactPositon >= npcTalkUIs.Count)
        //{
        //    canInteract = false;
        //}
     

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
            //If the shop hasn't yet been opened, create it so that it generates appropriate items, otherwise reopen it
            //if (instantiatedUIs[npc.interactPositon] == null)
            //{
            //    instantiatedUIs[npc.interactPositon] = Instantiate(npcUIs[npc.interactPositon]);
            //    instantiatedUIs[npc.interactPositon].GetComponent<NPCUI>().NPC = this;
            //}
            //else
            //{
            //    instantiatedUIs[npc.interactPositon].SetActive(true);
            //}

            if (instantiatedMenu == null)
            {
                instantiatedMenu = Instantiate(menu);
                instantiatedMenu.GetComponent<NPCUI>().NPC = this;
            }
            else
            {
                instantiatedMenu.SetActive(true);
            }

            inUI = true;
            //Lock the players actions, enable the shop
            playerLook.LockCursor();
            //playerMove.ableToMove = true;
            playerLook.ableToMove = false;
            shooting.ableToShoot = false;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameUI.SetActive(false);


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
            gameUI = GameObject.Find("GameplayUI");

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
        npc.AssessInteract();
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
        playerMove.ableToMove = true;
        playerLook.ableToMove = true;
        shooting.ableToShoot = true;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameUI.SetActive(true);

    }
}
