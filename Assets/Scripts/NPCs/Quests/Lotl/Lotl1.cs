using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lotl1 : Quest
{
    List<Room> levelRooms = new List<Room>();

    public override void LevelStartQuestBehaviour()
    {
        base.LevelStartQuestBehaviour();

        levelRooms.Clear();

        foreach (GameObject room in GameObject.Find("Level Generation").GetComponent<LevelGeneration>().GetRooms())
        {
            levelRooms.Add(room.GetComponent<Room>());
        }
        
    }

    public override void UpdateQuestBehaviour()
    {
        base.UpdateQuestBehaviour();

    }
}
