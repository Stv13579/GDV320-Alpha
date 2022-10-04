using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuUI : NPCUI
{

	public override void Start()
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
	            NPC.instantiatedTalkUI.GetComponent<NPCUI>().SetNPC(NPC);
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
	            NPC.instantiatedTalkUI.GetComponent<NPCUI>().SetNPC(NPC);
	            NPC.instantiatedTalkUI.GetComponent<DialogueUI>().SetNoOffering(true);
            }
            else if (NPC.instantiatedOfferingUI == null)
            {
                NPC.instantiatedOfferingUI = Instantiate(NPC.npcOfferingUI);
	            NPC.instantiatedOfferingUI.GetComponent<NPCUI>().SetNPC(NPC);
            }
            else
            {
                NPC.instantiatedOfferingUI.SetActive(true);
            }

        }

        this.gameObject.SetActive(false);
    }
}
