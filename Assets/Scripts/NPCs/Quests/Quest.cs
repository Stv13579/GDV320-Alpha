using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public virtual void UpdateQuestBehaviour()
    {

    }

    public virtual void LevelStartQuestBehaviour()
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
        GetComponent<QuestManager>().RemoveFromQuests(this);
    }
}
