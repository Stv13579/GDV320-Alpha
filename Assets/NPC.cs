using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPC : MonoBehaviour
{
    //A single piece of dialogue
    [Serializable]
    public class Dialogue
    {
        public List<string> lines;
        protected NPCData heldData;

        public Dialogue(NPCData npcData)
        {
            heldData = npcData;
        }

        //Called once all the lines of this dialogue are exhausted, call this action
        public virtual void Action()
        {

        }

        public virtual void OnSpeakAction(int placeToAct)
        {
            //An acion which is taken on a give place
        }

    }

    
    List<Dialogue> storyDialogues, randomDialogues;
    [HideInInspector]
    public List<Dialogue> currentDialogue;

    public int place = 0; 
    //Defines an NPC in the broad sense
    //Holds dialogue and functionality for questlines.

    //Choose which dialogues to add to the current dialogue list
    public virtual void AssessDialogue()
    {

    }

}
