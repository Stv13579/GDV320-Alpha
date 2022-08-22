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
        protected bool actionTaken = false;
        public void SetHeldData(NPCData nData) { heldData = nData; }
        
        //
        public Predicate<int> onSpeakAction;

        public Dialogue(NPCData npcData)
        {
            heldData = npcData;
        }

        //Called once all the lines of this dialogue are exhausted, call this action
        public virtual void Action()
        {
            actionTaken = true;
        }

        public virtual void OnSpeakAction(int placeToAct)
        {
            //An acion which is taken on a give place
        }

    }

    [Serializable]
    public class Story : Dialogue
    {
        public Story(NPCData npcData) : base(npcData) { }

        public override void Action()
        {
            base.Action();

            if(!heldData.seenStoryPoints.Contains(this))
            {
                heldData.seenStoryPoints.Add(this);
                heldData.intraStoryPosition++;
                if(heldData.intraStoryPosition == 3)
                {
                    heldData.intraStoryPosition = 0;
                    
                    heldData.questReady = true;
                }
            }
        }
    }
    

    [Serializable]
    public class Assignment : Dialogue
    {
        public Assignment(NPCData npcData) : base(npcData) { }

        //Add the quest to the manager
        public override void Action()
        {
            if(actionTaken)
            {
                return;
            }
            Quest q = (Quest)GameObject.Find("Quest Manager").GetComponent(heldData.quests[heldData.storyPosition]);
            q.ActivateQuest();

            heldData.onQuest = true;
            heldData.questReady = false;
            base.Action();
        }
    }

    [Serializable]
    public class HandIn : Dialogue
    {
        public HandIn(NPCData npcData) : base(npcData) { }

        //Add the quest to the manager
        public override void Action()
        {
            if(actionTaken)
            {
                return;
            }
            base.Action();
            Quest q = (Quest)GameObject.Find("Quest Manager").GetComponent(heldData.quests[heldData.storyPosition]);
            q.FinishQuest();
            heldData.storyPosition++;

            heldData.onQuest = false;
            heldData.questComplete = false;
        }
    }

    //A Serialzed way to store all story dialogue
    [Serializable]
    public class StoryDialogues
    {
        public List<Story> dialogues;
    }

    [SerializeField]
    protected List<StoryDialogues> storyDialogues = new List<StoryDialogues>();

    protected List<Dialogue> possibleDialogues = new List<Dialogue>();

    [SerializeField]
    protected List<Assignment> giveQuest = new List<Assignment>();
    [SerializeField]
    protected List<HandIn> recieveHandIn = new List<HandIn>();


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
        //Possible dialogues include the random ones, the current story position, or a deterministic quest dialogue.
        int storyTime = UnityEngine.Random.Range(0, 2);


        //Initialise seralized dialogues
        foreach(StoryDialogues diag in storyDialogues)
        {
            foreach(Dialogue dg in diag.dialogues)
            {
                dg.SetHeldData(data);
            }
        }

        foreach (Dialogue dg in giveQuest)
        {
            dg.SetHeldData(data);
        }

        foreach (Dialogue dg in recieveHandIn)
        {
            dg.SetHeldData(data);
        }

        if (data.questComplete)
        {
            ResolveQuest();
            return;
        }
        else if(data && data.questReady)
        {
            GiveQuest();
            return;
        }
        else if(storyTime > 0 && storyDialogues[data.storyPosition].dialogues.Count > 0 && !data.onQuest)
        {
            possibleDialogues.AddRange(storyDialogues[data.storyPosition].dialogues);

            //Remove all story stuff that has been seen already
            possibleDialogues.RemoveAll(DiagPred);
        }
        else
        {
            possibleDialogues.AddRange(baseRandoms);
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

    public virtual void GiveQuest()
    {
        //Give quest dialogue
        currentDialogue = giveQuest[data.storyPosition];

        
    }

    public virtual void ResolveQuest()
    {
        //Give quest dialogue
        currentDialogue = recieveHandIn[data.storyPosition];

        
    }


    bool DiagPred(Dialogue diag)
    {
        return data.seenStoryPoints.Contains(diag);
    }


}
