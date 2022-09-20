using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : NPC
{
    [SerializeField]
    Dialogue postConversation;
    [SerializeField]
    GameObject postTutMenu;

    public override void TutorialAction()
    {
        base.TutorialAction();
        //Set current dialogue and give the option to open up the shop
        currentDialogue = postConversation;
        GetComponent<Interact>().menu = postTutMenu;
    }
}
