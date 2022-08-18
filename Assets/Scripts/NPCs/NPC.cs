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

    [Serializable]
    public class StoryDialogues
    {
        public List<Dialogue> dialogues;
    }

    [SerializeField]
    protected List<StoryDialogues> storyDialogues = new List<StoryDialogues>();

    protected List<Dialogue> possibleDialogues = new List<Dialogue>();

    [HideInInspector]
    public Dialogue currentDialogue;
    public Dialogue noMoreDialogue;

    public int place = 0;
    public int interactPositon = 0;

    [SerializeField]
    protected NPCData data;

    [SerializeField]
    List<Dialogue> baseRandoms;


    [SerializeField]
    public string offeringType;

    //Defines an NPC in the broad sense
    //Holds dialogue and functionality for questlines.
    public void Start()
    {
        

        //Create an array of possible dialogues and then choose one at random.
        //Possible dialogues include the random ones and the story position, or a deterministic quest dialogue.
        if(data && data.questReady)
        {
            //Give quest dialogue
            currentDialogue = new Dialogue(data);
            currentDialogue.lines.Add("Wow");
            data.questReady = false;
        }
        else
        {
            possibleDialogues.AddRange(baseRandoms);
            //possibleDialogues.AddRange(storyDialogues[data.storyPosition].dialogues);
        }

        currentDialogue = possibleDialogues[UnityEngine.Random.Range(0, possibleDialogues.Count)];
    }

    //Choose which dialogues to add to the current dialogue list
    public virtual void AssessDialogue()
    {
        //if (data.storyPosition < 1)
        //{

        //    currentDialogue = randomDialogues[UnityEngine.Random.Range(0, randomDialogues.Count)];
        //}

        
    }

    //A function to allow changes during the interact process.
    //Called at the start of interact update
    public virtual void AssessInteract()
    {
        if (interactPositon >= 3)
        {
            interactPositon = 2;
        }
    }

}
