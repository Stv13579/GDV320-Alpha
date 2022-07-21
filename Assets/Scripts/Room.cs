using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int weighting;
    public bool illegal = false;
    public Vector2Int gridPos = Vector2Int.zero;
    List<GameObject> activeDoors;
    GameObject levelGenerator;

    private void Start()
    {
        levelGenerator = GameObject.Find("Level Generator");
    }

    //Closes off all the illegal/irrelevant exits and collates the remaining ones
    public void CloseDoors()
    {
        //Check each direction, disable them if nothing there, add them to active if not
        for(int i = 0; i < 4; i++)
        {

        }

    }

    //if the door location is clear, return true
    bool CheckDoor(Vector2 pos)
    {
        return true;
    }

    public void UnlockDoors()
    {

    }

}
