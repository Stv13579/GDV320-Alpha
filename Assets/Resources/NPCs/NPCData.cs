using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu, System.Serializable]
public class NPCData : ScriptableObject
{
    //Where the player is in the overall story of the NPC
    public int storyPosition;
    //How much of the current story the player has seen
    public int intraStoryPosition;

    public bool questReady = false;
    public bool onQuest = false;
    public bool questComplete = false;
    public bool met = false;
    public List<string> quests;
    public List<string> questDescriptions;

    public bool LoadData(NPCSaveData sData)
    {
        if(sData == null)
        {
            return false;
        }
        storyPosition = sData.storyPosition;
        intraStoryPosition = sData.intraStoryPosition;

        questReady = sData.questReady;
        onQuest = sData.onQuest;
	    questComplete = sData.questComplete;

        met = sData.met;

        return true;
    }
}
