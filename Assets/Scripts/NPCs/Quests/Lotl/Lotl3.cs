using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lotl3 : Quest
{
    int timesFound = 0;

    [SerializeField]
    int chanceToSpawn;

    List<Room> levelRooms = new List<Room>();

    public override void LevelStartQuestBehaviour()
    {

        levelRooms.Clear();
        levelRooms = new List<Room>();

        //Check against a chance based thing whether to spawn a hidden Lotl on this floor
        if(Random.Range(0, 100) <= chanceToSpawn)
        {
            foreach (GameObject room in GameObject.Find("Level Generator").GetComponent<LevelGeneration>().GetRooms())
            {
                levelRooms.Add(room.GetComponent<Room>());
            }
        }
        //If so, spawn it at a predetermined position on a randomly selected combat room


    }

    //Called when the player interacts with a hidden lilly
    public void LillyFound()
    {
        timesFound++;
    }

    public override void RoomFinishQuestBehaviour()
    {
        base.RoomFinishQuestBehaviour();

        if (timesFound == 3)
        {
            FinishQuest();
        }
    }


}
