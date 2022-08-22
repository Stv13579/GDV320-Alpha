using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    List<Quest> activeQuests;
    
    void Update()
    {
        foreach(Quest q in activeQuests)
        {
            q.UpdateQuestBehaviour();
        }
    }

    public void StartLevelUpdate()
    {
        foreach (Quest q in activeQuests)
        {
            q.LevelStartQuestBehaviour();
        }
    }

    public void AddToQuests(Quest qToAdd)
    {
        activeQuests.Add(qToAdd);
    }

    public void RemoveFromQuests(Quest qToRemove)
    {
        activeQuests.Remove(qToRemove);
    }
}
