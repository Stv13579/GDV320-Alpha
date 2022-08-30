using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class DialogueUI : NPCUI
{
    [SerializeField]
    NPC.Dialogue dialogueLines;
    GameObject dialogueBox;
    public bool noOffering = false;

    [SerializeField]
    TextMeshProUGUI nameField;

    int place = 0;
    //Show dialogue lines as specified, activate the action once exhausted, then delete itself

    private void Start()
    {
        base.Start();
        NPC.npc.AssessDialogue();
        dialogueLines = NPC.npc.currentDialogue;
        dialogueBox = transform.GetChild(0).gameObject;

        nameField.text = NPC.name;

        if(noOffering)
        {
            dialogueLines = NPC.npc.noMoreDialogue;
        }

    }

    private void Update()
    {
        if (place >= dialogueLines.lines.Count)
        {
            //Exit, delete the UI and move up
            dialogueLines.Action();
            NPC.npc.interactPositon++;
            Close();
            Destroy(this.gameObject);
            return;
        }
        else
        {
            dialogueBox.GetComponentInChildren<TextMeshProUGUI>().text = dialogueLines.lines[place];
            dialogueLines.OnSpeakAction(place);
            
        }
    }

    public void MoveDialogueAlong()
    {
        place++;

    }

    public override void Close()
    {
        place = 0;

        base.Close();
    }

    private void OnDisable()
    {
        place = 0;
    }

}
