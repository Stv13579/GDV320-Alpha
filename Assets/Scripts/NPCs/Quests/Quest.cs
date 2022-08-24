using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{

    [SerializeField]
    NPCData npc;

    [SerializeField]
    Trinket trinketToUpgrade;

    public void SetData(NPCData data) { npc = data; }

    public virtual void UpdateQuestBehaviour()
    {

    }

    public virtual void LevelStartQuestBehaviour()
    {

    }

    public virtual void RoomFinishQuestBehaviour()
    {

    }

    //Adds itself to the quest manager for future updates and implements derived behaviours
    public virtual void ActivateQuest()
    {
        GetComponent<QuestManager>().AddToQuests(this);
    }

    //Removes itself from the quest manager for future updates and implements derived behaviours
    public virtual void FinishQuest()
    {
        if(GetComponent<QuestManager>().RemoveFromQuests(this))
            npc.questComplete = true;

        trinketToUpgrade.Upgrade();
    }
}
