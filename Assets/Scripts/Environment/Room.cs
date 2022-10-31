using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [HideInInspector]
    public int weighting;
    public bool illegal = false;
    public Vector2Int gridPos = Vector2Int.zero;
    [SerializeField]
    List<GameObject> activeDoors = new List<GameObject>();
    protected LevelGeneration levelGenerator;
    bool locked = false;
    protected GameObject roomTrigger;
    public bool visited = false;

    public MinimapRoom minimapRoom;

    public bool hasLilly;

    [SerializeField]
    GameObject hiddenLilly;

    protected void Start()
    {
        
        

        this.GetComponent<TerrainCollider>().enabled = false;
        this.GetComponent<TerrainCollider>().enabled = true;
        
        roomTrigger = transform.Find("RoomTriggerBox").gameObject;

	    if(LevelGeneration.GetLevelGeneration())
		    levelGenerator = LevelGeneration.GetLevelGeneration();

    }



    //Closes off all the illegal/irrelevant exits and collates the remaining ones
    public void CloseDoors()
    {
        levelGenerator = LevelGeneration.GetLevelGeneration();

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
        transform.Find(doorToLock).gameObject.transform.Find("ClosedGateway").gameObject.SetActive(false);
        transform.Find(doorToLock).gameObject.transform.Find("LockedGateway").gameObject.SetActive(false);
        //activeDoors[activeDoors.Count - 1].transform.Find("ClosedGateway").gameObject.SetActive(false);
        //activeDoors[activeDoors.Count - 1].transform.Find("LockedGateway").gameObject.SetActive(false);
    }

    //Inaccessible routes
    void SetBlocked(string doorToBlock)
    {
        transform.Find(doorToBlock).transform.Find("LockedGateway").gameObject.SetActive(false);
        transform.Find(doorToBlock).transform.Find("ClosedGateway").gameObject.SetActive(true);
        
        
    }

    //Use to lock all of the doors
    public void LockDoors()
    {
        if(locked)
        {
            return;
        }

        foreach (GameObject door in activeDoors)
        {
            door.transform.Find("LockedGateway").gameObject.SetActive(true);
            foreach (Transform trans in door.transform)
            {
                if (trans.GetComponent<Animator>())
                {
                    //do fire anim
                    trans.GetComponent<Animator>();
                }
            }
        }

        locked = true;
        visited = true;
    }

    //Call to unlock all of the doors
    public void UnlockDoors()
    {
        if(!locked)
        {
            return;
        }

        foreach (GameObject door in activeDoors)
        {
            door.transform.Find("LockedGateway").gameObject.SetActive(false);
            foreach (Transform trans in door.transform)
            {
                if (trans.GetComponent<Animator>())
                {
                    //do fire anim
                    trans.GetComponent<Animator>();
                }
            }
        }

        locked = false;
    }

    //For Lilly's final quest. If called and available, place an interactable lilly somewhere in the room.
    public void HideLilly()
    {
        if(!hasLilly)
        {
            return;
        }


        hiddenLilly.SetActive(true);
    }


}
