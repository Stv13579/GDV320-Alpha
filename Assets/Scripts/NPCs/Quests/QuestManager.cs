using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	static QuestManager currentQuestManager;
    [SerializeField]
	List<Quest> activeQuests;
	[SerializeField]
	List<NPCData> npcs;

	public bool inHub;
    
	static bool exists = false;

	// Awake is called when the script instance is being loaded.
	protected void Awake()
	{
		if(exists)
		{
			Destroy(this.gameObject);
		}
	}
	
    private void Start()
    {
	    DontDestroyOnLoad(this.gameObject);
	    currentQuestManager = this;
	    exists = true;
	    foreach(NPCData npc in npcs)
	    {
	    	if(npc.onQuest)
	    	{
	    		AddToQuests((Quest)GetComponent(npc.quests[npc.storyPosition]));
	    	}
	    }
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
    
	public static QuestManager GetQuestManager()
	{
		return currentQuestManager;
	}
	
}
