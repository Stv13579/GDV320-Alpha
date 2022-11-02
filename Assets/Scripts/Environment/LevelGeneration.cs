using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
	static LevelGeneration currentLevelGeneration;
    [SerializeField]
    float roomSize;
    [SerializeField]
    int maxWeighting, minWeighting, weightingChange, minRooms, maxRooms;

    public GameObject startRoom;
    GameObject bossRoom;
    GameObject shop;
    GameObject NPC;

    List<GameObject> placedRooms = new List<GameObject>();

    List<GameObject> genericRooms = new List<GameObject>();
    public List<GameObject> GetGenericRooms() { return genericRooms; }

    List<GameObject> otherRooms = new List<GameObject>();

    public List<GameObject> GetRooms() { return placedRooms; }

    List<Vector2> roomPositions = new List<Vector2>();

    [SerializeField]
    List<GameObject> possibleGenericRooms, possibleBossRooms, possibleRespiteRooms, possibleEdgeRooms;

    [SerializeField]
    GameObject mmRoom, playerIndicator;

    [SerializeField]
    float mmSpacing;

    GameObject minimap;

    [SerializeField]
    bool tutorial;

    AudioManager audioManager;
    int rand;
    float currentTime;
    void Start()
	{
		PlayerClass.GetPlayer().SetActive(true);
		GameplayUI.GetUIObject().SetActive(true);
		currentLevelGeneration = this;
        minimap = GameObject.Find("MiniMap");
        audioManager = AudioManager.GetAudioManager();
        GenerateLevel();

		if (RunManager.GetRunManager())
        {
			RunManager.GetRunManager().StartNewLevel();
        }
    }
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= 15.0f)
        {
            int rand = Random.Range(1,3);
            if(rand == 1)
            {
                audioManager.PlaySFX("Ambient Wind");
            }
            else if(rand == 2)
            {
                audioManager.PlaySFX("Ambient Bugs");
            }
            else
            {
                
            }
            currentTime = 0.0f;
        }
    }

    //Encapsulates the generation process, and returns false if there is a fail for whatever reason
    bool GenerateLevel()
    {
        foreach(Transform room in minimap.transform)
        {
            Destroy(room.gameObject);
        }


        startRoom = PlaceRoom(new Vector3(0, 0, 0), genericRooms, possibleGenericRooms);
        

        int noRooms = Random.Range(minRooms, maxRooms + 1);

        for (int i = 0; i < noRooms; i++)
        {
            PlaceRoom(ChoosePosition(ChooseRoom()), genericRooms, possibleGenericRooms);
        }

        //Place the boss room
        bossRoom = PlaceRoom(ChoosePositionWithOneConnection(ChooseRoom()), otherRooms, possibleBossRooms);
        bossRoom.GetComponent<Room>().illegal = true;
        bossRoom.GetComponent<Room>().minimapRoom.SetAsBoss();

        ResetWeighting();

        //Place shop and NPC rooms
        shop = PlaceRoom(ChoosePositionWithOneConnection(ChooseRoom()), otherRooms, possibleRespiteRooms);
        shop.GetComponent<Room>().illegal = true;
        shop.GetComponent<Room>().minimapRoom.SetAsShop();
        shop.GetComponent<RespiteRoom>().isShoppe =  true;

        NPC = PlaceRoom(ChoosePositionWithOneConnection(ChooseRoom()), otherRooms, possibleRespiteRooms);
        NPC.GetComponent<Room>().illegal = true;
        NPC.GetComponent<Room>().minimapRoom.SetAsBreak();

        //Check if the player is carrying the balanced compass and roll to see if it triggers if so.
	    if(TrinketManager.GetTrinketManager())
        {
		    if (PlayerClass.GetPlayerClass().GetHeldItems().Contains(TrinketManager.GetTrinketManager().gameObject.GetComponent<BalancedCompass>()))
            {
                Debug.Log("Checking for extra room!");

                int randomRoll = Random.Range(0, 100);

                if (randomRoll < TrinketManager.GetTrinketManager().gameObject.GetComponent<BalancedCompass>().GetActivationChance())
                {
                    NPC = PlaceRoom(ChoosePositionWithOneConnection(ChooseRoom()), otherRooms, possibleRespiteRooms);
	                NPC.GetComponent<Room>().illegal = true;
	                NPC.GetComponent<Room>().minimapRoom.SetAsBreak();

                }
            }
        }

        Instantiate(playerIndicator, minimap.transform);
        
        foreach(GameObject room in placedRooms)
        {
            room.GetComponent<Room>().CloseDoors();
        }

        startRoom.GetComponent<Room>().minimapRoom.SetOccupied(Vector3.zero);

        AddWorldEdge();

        return true;
    }

    //Instantiates a new room at a given position and add it to the appropriate list, returning it for further use
    GameObject PlaceRoom(Vector3 roomPos, List<GameObject> typedList, List<GameObject> roomPrefabs, bool dontPlaceOnMap = false, bool addToPlacedRooms = true)
    {
        GameObject roomToSpawn = roomPrefabs[Random.Range(0, roomPrefabs.Count)];

        GameObject room = Instantiate(roomToSpawn, roomPos, Quaternion.identity);
        if(addToPlacedRooms)
        {
            placedRooms.Add(room);
        }
        typedList.Add(room);

        RecalculateWeighting(room);

        room.GetComponent<Room>().gridPos = new Vector2Int((int)(room.transform.position.x / roomSize), (int)(room.transform.position.z / roomSize));
        roomPositions.Add(room.GetComponent<Room>().gridPos);

        if(!dontPlaceOnMap)
        {
            room.GetComponent<Room>().minimapRoom = Instantiate(mmRoom, minimap.transform, false).GetComponent<MinimapRoom>();
            room.GetComponent<Room>().minimapRoom.SetUnexplored();
            room.GetComponent<Room>().minimapRoom.transform.localPosition = new Vector3(roomPos.x * mmSpacing, roomPos.z * mmSpacing, 0);
            room.GetComponent<Room>().minimapRoom.SetMaxDist(mmSpacing * 120);
        }

        

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
            if (placedRoom.GetComponent<Room>().weighting + runningTotal > chosenWeighting && !placedRoom.GetComponent<Room>().illegal)
            {
                room = placedRoom;
                return room;
            }


            runningTotal += placedRoom.GetComponent<Room>().weighting;

        }


        if (room == null) { 
            return ChooseRoom();
        };
        return room;
    }

    //Randomly choose a position at each direction of the given room to generate at, making sure that is legal
    Vector3 ChoosePosition(GameObject chosenRoom)
    {
        Vector3 pos = Vector3.zero;
        Vector3 chosenPos = chosenRoom.transform.position;
        Room chRoom = chosenRoom.GetComponent<Room>();

        //Find the legal positions of the room, if non, make it illegal and choose a new room
        List<Vector2> legalPositions = FindLegalPositions(chRoom);

        if(legalPositions.Count <= 0)
        {
            chRoom.illegal = true;
            return ChoosePosition(ChooseRoom());
        }

        Vector2 position = legalPositions[Random.Range(0, legalPositions.Count)];

        pos = new Vector3(position.x * roomSize, 0, position.y * roomSize);

        return pos;
    }

    //Randomly choose a position at each direction of the given room to generate at, making sure that is legal under the restriction that the new room can only have one connection
    Vector3 ChoosePositionWithOneConnection(GameObject chosenRoom)
    {
        Vector3 pos = Vector3.zero;
        Vector3 chosenPos = chosenRoom.transform.position;
        Room chRoom = chosenRoom.GetComponent<Room>();

        //Find the legal positions of the room, if non, make it illegal and choose a new room
        List<Vector2> legalPositions = FindLegalPositions(chRoom);

        if (legalPositions.Count <= 0)
        {
            chRoom.illegal = true;
            return ChoosePositionWithOneConnection(ChooseRoom());
        }

        Vector2 position = legalPositions[Random.Range(0, legalPositions.Count)];

        int breakCount = 0;

        while (!CheckAllRoomDirections(position))
        {
            legalPositions = FindLegalPositions(chRoom);

            if (legalPositions.Count <= 0)
            {
                chRoom.illegal = true;
                return ChoosePositionWithOneConnection(ChooseRoom());
            }

            position = legalPositions[Random.Range(0, legalPositions.Count)];

            breakCount++;
            if(breakCount > 9999)
            {
                Debug.Log("Breaking Free of Loop");
                return ChoosePositionWithOneConnection(ChooseRoom());
            }
        }

        pos = new Vector3(position.x * roomSize, 0, position.y * roomSize);

        return pos;
    }
    

    List<Vector2> FindLegalPositions(Room chosenRoom)
    {
        List<Vector2> legalPositions = new List<Vector2>();

        if (CheckRoomPosition(new Vector2(chosenRoom.gridPos.x + 1, chosenRoom.gridPos.y)))
        {
            legalPositions.Add(new Vector2(chosenRoom.gridPos.x + 1, chosenRoom.gridPos.y));
        }
        if (CheckRoomPosition(new Vector2(chosenRoom.gridPos.x - 1, chosenRoom.gridPos.y)))
        {
            legalPositions.Add(new Vector2(chosenRoom.gridPos.x - 1, chosenRoom.gridPos.y));
        }
        if (CheckRoomPosition(new Vector2(chosenRoom.gridPos.x, chosenRoom.gridPos.y + 1)))
        {
            legalPositions.Add(new Vector2(chosenRoom.gridPos.x, chosenRoom.gridPos.y + 1));
        }
        if (CheckRoomPosition(new Vector2(chosenRoom.gridPos.x, chosenRoom.gridPos.y - 1)))
        {
            legalPositions.Add(new Vector2(chosenRoom.gridPos.x, chosenRoom.gridPos.y - 1));
        }

        return legalPositions;
    }

    //if all but one room directions are empty, return true. Otherwise false
    bool CheckAllRoomDirections(Vector2 position)
    {
        int count = 0;

        if (!CheckRoomPosition(new Vector2(position.x + 1, position.y)))
        {
            count++;
        }
        if (!CheckRoomPosition(new Vector2(position.x - 1, position.y)))
        {
            count++;
        }
        if (!CheckRoomPosition(new Vector2(position.x, position.y + 1)))
        {
            count++;
        }
        if (!CheckRoomPosition(new Vector2(position.x, position.y - 1)))
        {
            count++;
        }

        if(count > 1)
        {
            return false;
        }

        return true;
    }

    //See if a given position has a room in it already
    public bool CheckRoomPosition(Vector2 posToCheck)
    {
        //Iterate through all taken room positions and return false if the room is found to be taken
        foreach (Vector2 pos in roomPositions)
        {
            if(pos == posToCheck)
            {
                return false;
            }
        }    

        return true;
    }

    public bool CheckEdgePosition(Vector2 posToCheck)
    {
        //Iterate through all taken room positions and return false if the room is found to be taken
        foreach (Vector2 pos in roomPositions)
        {
            foreach(GameObject room in placedRooms)
            {
                if (pos == posToCheck && room.GetComponent<Room>().gridPos == pos)
                {
                    return false;
                }
            }

            
        }

        return true;
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

    //Resets all weighting to an equalised value. Be careful, this loses the order of placed rooms
    void ResetWeighting()
    {
        foreach (GameObject room in placedRooms)
        {
            room.GetComponent<Room>().weighting = maxWeighting;
            if (room.GetComponent<Room>().illegal)
            {
                room.GetComponent<Room>().weighting = 0;
            }
        }
    }

    //Add the edge to the world in the form of additional rooms which fill up each empty spot on existing rooms (and corners?)
    void AddWorldEdge()
    {
        //Iterate through each placed room, checking if it has empty edges, then placing an edge piece
        foreach (GameObject room in placedRooms)
        {
            List<Vector2> legalPositions = FindLegalPositions(room.GetComponent<Room>());

            foreach (Vector2 pos in legalPositions)
            {
                GameObject edgeRoom = PlaceRoom(new Vector3(pos.x * roomSize, 0, pos.y * roomSize), genericRooms, possibleEdgeRooms, true, false);

                

                //Rotate it so that it is placed with its south edge against the given edge

            }
            
        }

    }
    
	public static LevelGeneration GetLevelGeneration()
	{
		return currentLevelGeneration;
	}


}
