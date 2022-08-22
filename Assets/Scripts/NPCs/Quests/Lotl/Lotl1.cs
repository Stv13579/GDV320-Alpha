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
        levelRooms = new List<Room>();

        foreach (GameObject room in GameObject.Find("Level Generator").GetComponent<LevelGeneration>().GetRooms())
        {
            levelRooms.Add(room.GetComponent<Room>());
        }
        
    }

    public override void RoomFinishQuestBehaviour()
    {
        base.RoomFinishQuestBehaviour();
        //Remove rooms from level rooms as they are finished.
        levelRooms.RemoveAll(CheckRoomPredicate);

        //If level rooms is empty, complete quest
        if (levelRooms.Count < 1)
        {
            FinishQuest();
        }
    }

    public override void UpdateQuestBehaviour()
    {
        base.UpdateQuestBehaviour();
        
    }

    bool CheckRoomPredicate(Room roomToChek)
    {
        //Predicate to check if the room has been completed, returning true if so.
        if(roomToChek.visited)
        {
            return true;
        }

        return false;
    }
}
