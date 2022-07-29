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

    int place = 0;
    //Show dialogue lines as specified, activate the action once exhausted, then delete itself

    private void Start()
    {
        dialogueLines = NPC.npc.currentDialogue[NPC.npc.place];
    }

    private void Update()
    {
        if (place >= dialogueLines.lines.Count)
        {
            //Exit, delete the UI and move up
            dialogueLines.Action();
            NPC.npc.place++;
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
