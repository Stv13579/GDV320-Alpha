using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lotl3 : Quest
{
    int timesFound = 0;


    public override void LevelStartQuestBehaviour()
    {
        //Check against a chance based thing whether to spawn a hidden Lotl on this floor

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
