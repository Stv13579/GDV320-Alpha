using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    List<Quest> activeQuests;

    public bool inHub;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (inHub)
        {
            return;
        }

        foreach (Quest q in activeQuests)
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

    public void FinishRoomUpdate()
    {
        foreach (Quest q in activeQuests)
        {
            q.RoomFinishQuestBehaviour();
        }
    }


    public void AddToQuests(Quest qToAdd)
    {
        if(!activeQuests.Contains(qToAdd))
            activeQuests.Add(qToAdd);
    }

    public bool RemoveFromQuests(Quest qToRemove)
    {
        if (activeQuests.Contains(qToRemove))
        {
            activeQuests.Remove(qToRemove);
            return true;
        }
        return false;
    }
}
