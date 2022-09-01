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

    public void StartRunUpdate()
    {
        foreach (Quest q in activeQuests)
        {
            q.StartRunBehaviour();
        }
    }

    public void FinishRunUpdate()
    {
        foreach (Quest q in activeQuests)
        {
            q.FinishRunBehaviour();
        }
    }

    public void DeathUpdate()
    {
        foreach (Quest q in activeQuests)
        {
            q.DeathBehaviour();
        }
    }

    public void SpawnUpdate(GameObject enemySpawning, string spawnOrigin)
    {
        foreach (Quest q in activeQuests)
        {
            q.SpawnEventBehaviour(enemySpawning, spawnOrigin);
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
