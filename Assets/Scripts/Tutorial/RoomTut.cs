using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTut : TutorialPointers
{

    [SerializeField]
    SAIM saimInRoom;

    public override void CheckConditions(bool done = false)
    {
        bool result = false;

        if (saimInRoom.spawnedEnemies.Count == 0 && saimInRoom.triggered)
        {
            result = true;
        }


        base.CheckConditions(result);
    }
}
