using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NPCData : ScriptableObject
{
    //Where the player is in the overall story of the NPC
    public int storyPosition;
    //How much of the current story the player has seen
    public int intraStoryPosition;

    public bool questReady = false;
    public bool onQuest = false;
    public bool questComplete = false; 

    public List<NPC.Dialogue> seenStoryPoints = new List<NPC.Dialogue>();


    public List<string> quests;
}
