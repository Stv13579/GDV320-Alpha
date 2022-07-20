using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField]
    float roomSize;
    [SerializeField]
    int maxWeighting, minWeighting, weightingChange, minRooms, maxRooms;

    GameObject startRoom;

    List<GameObject> placedRooms = new List<GameObject>();

    List<GameObject> genericRooms = new List<GameObject>();

    [SerializeField]
    GameObject roomTemplate;
    
    void Start()
    {
        GenerateLevel();
    }

    //Encapsulates the generation process, and returns false if there is a fail for whatever reason
    bool GenerateLevel()
    {
        startRoom = PlaceRoom(new Vector3(0, 0, 0), genericRooms);

        int noRooms = Random.Range(minRooms, maxRooms + 1);

        for (int i = 0; i < noRooms; i++)
        {
            PlaceRoom(ChoosePosition(ChooseRoom()), genericRooms);
        }

        return true;
    }

    //Instantiates a new room at a given position and add it to the appropriate list, returning it for further use
    GameObject PlaceRoom(Vector3 roomPos, List<GameObject> typedList)
    {
        GameObject room = Instantiate(roomTemplate, roomPos, Quaternion.identity);

        placedRooms.Add(room);
        typedList.Add(room);

        RecalculateWeighting(room);

        return room;
    }

    //Randomly chooses a new room based on weighting
    GameObject ChooseRoom()
    {
        GameObject room = null;

        int totalWeighting = 0;

        foreach (GameObject placedRoom in placedRooms)
        {
            totalWeighting += placedRoom.GetComponent<Room>().weighting;
        }

        int chosenWeighting = Random.Range(1, totalWeighting + 1);

        int runningTotal = 1;

        //If the weighting of the room (plus the running total) is grater than the chosen weighting, choose that one.
        //This is because the random number represents a position in a series of ranges for each room; the range being the weighting of the previous room (min) and the addition of the current room (max)
        foreach (GameObject placedRoom in placedRooms)
        {
            

            if (placedRoom.GetComponent<Room>().weighting + runningTotal > chosenWeighting)
            {
                room = placedRoom;
            }


            runningTotal += placedRoom.GetComponent<Room>().weighting;

        }


        if (room == null) { Debug.Log("Room not chosen!"); };
        return room;
    }

    //Randomly choose a position at each direction of the given room to generate at, making sure that is legal
    Vector3 ChoosePosition(GameObject chosenRoom)
    {
        Vector3 pos = Vector3.zero;
        Vector3 chosenPos = chosenRoom.transform.position;

        //Check all directions are legal
        if(!CheckLegalRoom(chosenRoom))
        {
            //If not, set it to illegal and choose another room
            chosenRoom.GetComponent<Room>().illegal = true;
            return ChoosePosition(ChooseRoom());
        }

        int randDir = Random.Range(-1, 2);
        if (randDir == 0) { randDir = 1; };
        int isX = Random.Range(0, 2);
        int isZ = isX == 0 ? 1 : 0;

        pos = chosenPos + new Vector3(roomSize * randDir * isX, 0, roomSize * randDir * isZ);
        
        //While the chosen position is overlapped, choose a new position
        while (CheckOverlap(pos))
        {
            randDir = Random.Range(-1, 2);
            if (randDir == 0) { randDir = 1; };
            isX = Random.Range(0, 2);
            isZ = isX == 0 ? 1 : 0;

            pos = chosenPos + new Vector3(roomSize * randDir * isX, 0, roomSize * randDir * isZ);
        }

        return pos;
    }

    //Checks to make sure that a room is legal and not already surrounded.
    bool CheckLegalRoom(GameObject room)
    {
        bool legal = true;

        int counter = 0;

        for (int j = -1; j < 1; j++)
        {
            for (int k = -1; k < 1; k++)
            {
                if (j == 0 && k == 0)
                {}
                else if(j != 0 && k != 0)
                {}
                else
                {
                    //if there is an overlap, increase the counter. 4 counters = 4 overlaps and all sides are blocked
                    if(CheckOverlap(room.transform.position + new Vector3(roomSize * j, 0 , roomSize * k)))
                    {
                        counter++;
                    }

                }
            }
        }

        if(counter == 4)
        {
            legal = false;
        }
        return legal;
    }

    //Makes sure that the room is not overlapping another if positioned there, return if overlapping or not (true = overlapped)
    bool CheckOverlap(Vector3 pos)
    {
        foreach (GameObject room in placedRooms)
        {
            if (pos == room.transform.position)
            {
                return true;
            }
        }

        return false;
    }


    //Recalculates weighting, setting the new spawn to highest, and reducing the rest
    void RecalculateWeighting(GameObject lastPlacedRoom)
    {
        lastPlacedRoom.GetComponent<Room>().weighting = maxWeighting + weightingChange;

        foreach (GameObject room in placedRooms)
        {
            room.GetComponent<Room>().weighting -= weightingChange;
            if(room.GetComponent<Room>().weighting <= minWeighting)
            {
                room.GetComponent<Room>().weighting = minWeighting;
            }
            if (room.GetComponent<Room>().illegal)
            {
                room.GetComponent<Room>().weighting = 0;
            }
        }
    }

}
