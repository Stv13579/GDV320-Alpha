using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCSaveData : SaveData
{
    //Where the player is in the overall story of the NPC
    public int storyPosition;
    //How much of the current story the player has seen
    public int intraStoryPosition;

    public bool questReady;
    public bool onQuest;
    public bool questComplete;

    public bool met;

    List<int> seenStoryIndexes = new List<int>();

    public NPCSaveData(NPCData data)
    {
        storyPosition = data.storyPosition;

        intraStoryPosition = data.intraStoryPosition;

        questReady = data.questReady;
        onQuest = data.onQuest;
        questComplete = data.questComplete;

        met = data.met;
    }
}
