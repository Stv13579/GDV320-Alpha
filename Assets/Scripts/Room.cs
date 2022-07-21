using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int weighting;
    public bool illegal = false;
    public Vector2Int gridPos = Vector2Int.zero;
    List<GameObject> activeDoors = new List<GameObject>();
    LevelGeneration levelGenerator;

    private void Start()
    {
        levelGenerator = GameObject.Find("Level Generator").GetComponent<LevelGeneration>();
    }

    //Closes off all the illegal/irrelevant exits and collates the remaining ones
    public void CloseDoors()
    {
        levelGenerator = GameObject.Find("Level Generator").GetComponent<LevelGeneration>();

        //Check each direction, disable them if nothing there, add them to active if not
        if (!levelGenerator.CheckRoomPosition(new Vector2(gridPos.x + 1, gridPos.y)))
        {
            SetLocked("Gateway_East");
        }
        else
        {
            SetBlocked("Gateway_East");
        }

        if (!levelGenerator.CheckRoomPosition(new Vector2(gridPos.x - 1, gridPos.y)))
        {
            SetLocked("Gateway_West");
        }
        else
        {
            SetBlocked("Gateway_West");
        }

        if (!levelGenerator.CheckRoomPosition(new Vector2(gridPos.x, gridPos.y + 1)))
        {
            SetLocked("Gateway_North");
        }
        else
        {
            SetBlocked("Gateway_North");
        }

        if (!levelGenerator.CheckRoomPosition(new Vector2(gridPos.x, gridPos.y - 1)))
        {
            SetLocked("Gateway_South");
        }
        else
        {
            SetBlocked("Gateway_South");
        }

    }

    //Accessible Routes
    void SetLocked(string doorToLock)
    {
        activeDoors.Add(transform.Find(doorToLock).gameObject);
        activeDoors[activeDoors.Count - 1].transform.Find("ClosedGateway").gameObject.SetActive(false);
        activeDoors[activeDoors.Count - 1].transform.Find("LockedGateway").gameObject.SetActive(false);
    }

    //Inaccessible routes
    void SetBlocked(string doorToBlock)
    {
        transform.Find(doorToBlock).transform.Find("LockedGateway").gameObject.SetActive(false);
        transform.Find(doorToBlock).transform.Find("ClosedGateway").gameObject.SetActive(true);
    }

    public void UnlockDoors()
    {

    }

}
