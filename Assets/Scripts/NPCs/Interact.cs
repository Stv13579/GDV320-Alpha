using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    bool inRange = false;
    public GameObject npcUI;
    GameObject instantiatedUI = null;
    bool inUI = false;
    PlayerMovement playerMove;
    PlayerLook playerLook;
    Shooting shooting;
    GameObject gameUI;
    [HideInInspector]
    public NPC npc;
    public bool canInteract = true;

    void Start()
    {
        gameUI = GameObject.Find("GameplayUI");
        npc = GetComponent<NPC>();
    }
    void Update()
    {

     

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
            if (instantiatedUI == null)
            {
                instantiatedUI = Instantiate(npcUI);
                instantiatedUI.GetComponent<NPCUI>().NPC = this;
            }
            else
            {
                instantiatedUI.SetActive(true);
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
            instantiatedUI.GetComponent<ShopUI>().Close();
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
        //Unlock the players actions, disable the shop
        inUI = false;
        if(instantiatedUI != null)
        {
            instantiatedUI.SetActive(false);
        }

        playerLook.ForceLockCursor();
        playerMove.ableToMove = true;
        playerLook.ableToMove = true;
        shooting.ableToShoot = true;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameUI.SetActive(true);

    }
}
