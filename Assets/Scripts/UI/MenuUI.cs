using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuUI : NPCUI
{

    private void Start()
    {
        transform.Find("Offerings").GetComponentInChildren<TextMeshProUGUI>().text = NPC.npc.offeringType;
        transform.Find("Talk").GetComponentInChildren<TextMeshProUGUI>().text = "Talk";

    }

    public void MenuButton(bool isTalk)
    {
        if(isTalk)
        {
            if (NPC.instantiatedTalkUI == null)
            {
                NPC.instantiatedTalkUI = Instantiate(NPC.npcTalkUI);
                NPC.instantiatedTalkUI.GetComponent<NPCUI>().NPC = NPC;
            }
            else
            {
                NPC.instantiatedTalkUI.SetActive(true);
            }

            
        }
        else
        {

            if(NPC.canSeeOfferings == false)
            {
                NPC.instantiatedTalkUI = Instantiate(NPC.npcTalkUI);
                NPC.instantiatedTalkUI.GetComponent<NPCUI>().NPC = NPC;
                NPC.instantiatedTalkUI.GetComponent<DialogueUI>().noOffering = true;
            }
            else if (NPC.instantiatedOfferingUI == null)
            {
                NPC.instantiatedOfferingUI = Instantiate(NPC.npcOfferingUI);
                NPC.instantiatedOfferingUI.GetComponent<NPCUI>().NPC = NPC;
            }
            else
            {
                NPC.instantiatedOfferingUI.SetActive(true);
            }

        }

        this.gameObject.SetActive(false);
    }
}
