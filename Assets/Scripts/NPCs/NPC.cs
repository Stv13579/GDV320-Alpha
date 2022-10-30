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
            if(heldData)
            {
                heldData.met = true;
            }
             
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

            
            heldData.intraStoryPosition++;
            if(heldData.intraStoryPosition == 3)
            {
                heldData.intraStoryPosition = 0;

                heldData.questReady = true;
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
	        Quest q = (Quest)QuestManager.GetQuestManager().gameObject.GetComponent(heldData.quests[heldData.storyPosition]);
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
            Quest q = (Quest)QuestManager.GetQuestManager().gameObject.GetComponent(heldData.quests[heldData.storyPosition]);
            heldData.storyPosition++;
            q.HandInQuest();

            heldData.onQuest = false;
            heldData.questComplete = false;
        }
    }

    [Serializable]
    public class Tutorial : Dialogue
    {
        public delegate void TutorialAction();

        public TutorialAction tutAct;

        public Tutorial(NPCData npcData) : base(npcData) { }

        //Add the quest to the manager
        public override void Action()
        {
            if (actionTaken)
            {
                return;
            }
            base.Action();
            tutAct();
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
    public Dialogue firstMeeting;

    public int place = 0;
    public int interactPositon = 0;

    [SerializeField]
    string dataToApply;
    [SerializeField]
    protected NPCData data;

    //Initialise with this on load
    protected NPCSaveData saveData;

    [SerializeField]
    List<Dialogue> baseRandoms;

    [SerializeField]
    List<Tutorial> tutorialDialogues;

    [SerializeField]
    public string offeringType;

    [SerializeField]
    bool tutorial;

    [SerializeField]
    GameObject neckBone;
    GameObject player;
    Animator anims;


    //Defines an NPC in the broad sense
    //Holds dialogue and functionality for questlines.
    public void Start()
    {
        
        //Create an array of possible dialogues and then choose one at random.
        //Possible dialogues include the random ones, the current story position, or a deterministic quest dialogue.
        int storyTime = UnityEngine.Random.Range(0, 2);
        anims = transform.GetComponentInChildren<Animator>();
	    player = PlayerClass.GetPlayerClass().gameObject;
        data = (NPCData)Resources.Load("NPCs/" + dataToApply);

        //Initialise seralized dialogues
        foreach (StoryDialogues diag in storyDialogues)
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

        foreach (Dialogue dg in baseRandoms)
        {
            dg.SetHeldData(data);
        }

        if (tutorial)
        {
            tutorialDialogues[0].SetHeldData(data);
            tutorialDialogues[0].tutAct = TutorialAction;
            possibleDialogues.Add(tutorialDialogues[0]);
        }
        else if (data.met == false)
        {

            firstMeeting.SetHeldData(data);
            possibleDialogues.Add(firstMeeting);
            

        }
        else if (data.questComplete)
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
            possibleDialogues.Add(storyDialogues[data.storyPosition].dialogues[data.intraStoryPosition]);
        }
        else
        {
            possibleDialogues.AddRange(baseRandoms);
        }

        currentDialogue = possibleDialogues[UnityEngine.Random.Range(0, possibleDialogues.Count)];
    }

    private void FixedUpdate()
    {
        if(neckBone)
        {
            //Rotate towards the player
            if (Vector3.Angle(transform.forward, player.transform.position - transform.position) < 60
                && Vector3.Angle(transform.forward, player.transform.position - transform.position) > 0.01f)
            {
                Vector3 rot = neckBone.transform.localRotation.eulerAngles;
                rot.x = -Vector3.SignedAngle(transform.forward, player.transform.position - transform.position, Vector3.up);
                neckBone.transform.localRotation = Quaternion.Euler(rot);
            }
        }
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

    public virtual void TutorialAction()
    {
    
    }
   

    public virtual void PlayRandomFidget()
    {
        //Choose a random fidget based on the first three parameters in the controller

        if(anims != null)
        {
            int rand = UnityEngine.Random.Range(0, 3);

            if(rand == 2)
            {
                return;
            }

            anims.SetTrigger(anims.parameters[rand].name); 
        }
    }

    public virtual void PlayInteractFidget()
    {
        //Choose a random fidget based on the first three parameters in the controller

        if (anims != null)
        {
           
            anims.SetTrigger("Interact");
        }
    }



}
