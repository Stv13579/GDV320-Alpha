using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lotl3 : Quest
{
    int timesFound = 0;

    [SerializeField]
    int chanceToSpawn;

    List<GameObject> levelRooms = new List<GameObject>();

    [SerializeField]
    string hiddenObject;

    public override void LevelStartQuestBehaviour()
    {

        levelRooms.Clear();
        levelRooms = new List<GameObject>();

        //Check against a chance based thing whether to spawn a hidden Lotl on this floor
        if (Random.Range(0, 100) <= chanceToSpawn)
        {
            foreach (GameObject room in LevelGeneration.GetLevelGeneration().GetGenericRooms())
            {
                levelRooms.Add(room);
            }

            GameObject roomToHideIn = levelRooms[Random.Range(0, LevelGeneration.GetLevelGeneration().GetGenericRooms().Count)];
            GameObject hiddenGameObj = roomToHideIn.transform.Find(hiddenObject).gameObject;
            hiddenGameObj.SetActive(true);

            hiddenGameObj.GetComponent<HiddenObject>().SetQuest(this);
        }

        //If so, spawn it at a predetermined position on a randomly selected combat room


    }

    //Called when the player interacts with a hidden lilly
    public override void FindHiddenObject()
    {
        base.FindHiddenObject();
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
