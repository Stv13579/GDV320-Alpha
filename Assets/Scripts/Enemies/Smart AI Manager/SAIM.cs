using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using Unity.Jobs;
using System.Threading;

[System.Serializable]
public class SAIM : MonoBehaviour
{
    public List<BaseEnemyClass> spawnedEnemies;

    [SerializeField, HideInInspector]
    public List<Node> nodes;
    [SerializeField, HideInInspector]
    List<Node> deadNodes;

    [HideInInspector]
    public List<Node> aliveNodes;

    [SerializeField, HideInInspector]
    List<List<List<Node>>> instantiateNodeGrid;

    [System.Serializable]
    public class NodeMatrix
    {

        public List<Node> nodeCol;
    }


    [SerializeField, HideInInspector]
    List<NodeMatrix> nodeGrid;

    [SerializeField]
    List<GameObject> blockers = new List<GameObject>();

    public List<GameObject> testObjects;

    [SerializeField]
    GameObject node;
    [SerializeField]
    GameObject blockerMaster;

    //Object which connects rooms
    [SerializeField]
    List<GameObject> bridges;

    //The room in which the SAIM is placed
    Room containingRoom;

    //number in nodes of the sides of the grid.
    [SerializeField]
    int gridSize, gridHeight;

    [SerializeField]
    float nodeSpacing;

    [SerializeField]
    GameObject nodeMaster;

    //FOR TESTING
    [SerializeField]
    public Material killMat;
    public LayerMask nodeLayerMask;
    public LayerMask blankSpaceLayerMask;
    public LayerMask verticalSpaceLayerMask;
    public LayerMask impassableLayerMask;
    public LayerMask cullLayerMask;

    [HideInInspector]
    public bool triggered = false, playerLeaving = false, roomComplete = false;

    bool spawningFinished = false;

    //Total number of collective spawns
    int spawnAmount;

    //The amount of elapsed time since spawning started.
    float timeInSpawning;

    //Time since last spawning event
    float spawnTimer;

    public SAIMData data;

    //Flow Field pathfinding variables
    Node destinationNode;

    GameObject player;

    [SerializeField]
    float allowableHeightDifference;

    //Nodes to pathfind to
    Node playerNode;

    //Difficulty adjustment properties
    float diffAdjTimerDAM;
    float diffAdjTimerKIL;

    float previousHealth;
    int previousEnemyCount;

    int currentKills;
    float currentDamageTaken;

	static int fireUse = 0, crystalUse = 0, waterUse = 0;

    // Aydens Audio
    AudioManager audioManager;

    RunManager runManager;

    [SerializeField]
    bool bossSaim = false;

    BossRoom bossRoom;

    TextMeshProUGUI enemyCounter;

	EnemyObjectPool ePool;
    
	EnemySpawnPool mPool;

    void Awake()
    {
        //Aydens Audio manager
	    audioManager = AudioManager.GetAudioManager();
	    runManager = RunManager.GetRunManager();
        bossRoom = FindObjectOfType<BossRoom>();
        enemyCounter = GameObject.Find("Enemy Counter").GetComponent<TextMeshProUGUI>();
	    ePool = FindObjectOfType<EnemyObjectPool>();
	    mPool = FindObjectOfType<EnemySpawnPool>();

        foreach (Node node in aliveNodes)
        {
            SetNeighbourNodes(node, false);
            SetNeighbourNodes(node, true);
        }
    }
    private void Start()
    {
	    player = PlayerClass.GetPlayerClass().gameObject;

	    data.SetAdjustedDifficulty(data.GetDifficulty());
	    data.SetPlayer(player);
	    diffAdjTimerDAM = data.GetAdjustTimerDamage();
	    diffAdjTimerKIL = data.GetAdjustTimerKills();
        
        //CreateAndKillNodes();
        foreach (Transform child in blockerMaster.transform)
        {
            blockers.Add(child.gameObject);
        }


        containingRoom = transform.parent.GetComponent<Room>();
    }

    void Update()
	{
        if (bossRoom)
        {
            if (!bossRoom.GetBossSpawned())
            {
                if (audioManager)
                {
                    if (runManager)
                    {
                        audioManager.FadeOutAndPlayMusic($"Level {runManager.GetSceneIndex() - 1} Non Combat", $"Level {runManager.GetSceneIndex() - 1} Combat");
                    }
                }
            }
        }

        if (!triggered || roomComplete)
        {
            return;
        }

        spawnTimer += Time.deltaTime;
        timeInSpawning += Time.deltaTime;

        if(CheckSpawnConditions() && !bossSaim)
        {
	        Spawn(1, true);
           
        }

        CheckEndOfRoom();

        //The room has been explored and defeated 
        if (triggered && !roomComplete)
        {
            containingRoom.LockDoors();
            enemyCounter.text = spawnedEnemies.Count.ToString();
        }
        else
        {
            containingRoom.UnlockDoors();
            BaseDropScript[] drops = GameObject.FindObjectsOfType<BaseDropScript>();
            foreach(BaseDropScript drop in drops)
            {
                drop.SetRoomEnd(true);
            }
	        QuestManager.GetQuestManager().FinishRoomUpdate();
            enemyCounter.text = spawnedEnemies.Count.ToString();
        }
	    AdjustDifficulty();
		




        if(spawningFinished && spawnedEnemies.Count == 0)
        {
            //end the room
        }

        //Generate flow field based on player position
        //Check nearest node, call generate integration field based on that node.
        

    }

    private void FixedUpdate()
    {
        if(!triggered || roomComplete)
        {
            return;
        }

        //GameObject.Find("Enemy Counter").GetComponent<TextMeshProUGUI>().text = spawnedEnemies.Count.ToString();

        Move();
        
        Node pNode = null;

        foreach (BaseEnemyClass enemy in spawnedEnemies)
        {
            if(enemy == null)
            {
                continue;
            }

            float distToNode = float.MaxValue;

            //If the distance moved is miniscule since the last frame, continue.
            if ((enemy.GetOldPosition() - enemy.transform.position).magnitude < 1)
            {
                
                continue;
            }

            enemy.SetOldPosition(enemy.transform.position);

            foreach (Node node in aliveNodes)
            {
                if((node.transform.position - enemy.transform.position).magnitude < distToNode)
                {
                    distToNode = (node.transform.position - enemy.transform.position).magnitude;
                    enemy.SetMoveDirection((node.bestNextNodePos - node.transform.position).normalized);
                }

            }

            
        }

        float dist = float.MaxValue;

        foreach (Node node in aliveNodes)
        {
            if ((node.transform.position - player.transform.position).magnitude < dist)
            {
                pNode = node;
                dist = (node.transform.position - player.transform.position).magnitude;
            }

        }

        if (pNode != playerNode)
        {
            playerNode = pNode;
            CreateIntegrationFlowField();
         //   Thread flowThread = new Thread(CreateIntegrationFlowField);
	        //flowThread.Start();
	        //Thread genThread = new Thread(GenerateFlowField);
	        //genThread.Start();
	        GenerateFlowField();
        }
        
    }

    //Creates a grid of nodes with a given bounds, then kills the illegal ones (too high, inside collison, over dead space etc). 
    public void CreateAndKillNodes()
    {
        //deadNodes = new List<Node>();
        //aliveNodes = new List<Node>();

        instantiateNodeGrid = new List<List<List<Node>>>();

        for (int i = 0; i < gridSize; i++)
        {
            instantiateNodeGrid.Add(new List<List<Node>>());
            for (int j = 0; j < gridSize; j++)
            {
                instantiateNodeGrid[i].Add(new List<Node>());
            }
        }

        nodeGrid = new List<NodeMatrix>();
        
        for (int g = 0; g < gridSize; g++)
        {
            nodeGrid.Add(new NodeMatrix());
            nodeGrid[g].nodeCol = new List<Node>();
        }





        nodeMaster = transform.GetChild(1).gameObject;

        //Create a field of nodes
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                for (int k = 0; k < gridSize; k++)
                {
                    Vector3 nodeVec = new Vector3(i, j, k) * nodeSpacing;
                    GameObject newNode = Instantiate(node, nodeMaster.transform);

                    newNode.transform.localPosition = nodeVec;

                    nodes.Add(newNode.GetComponent<Node>());

                    instantiateNodeGrid[i][j].Add(newNode.GetComponent<Node>());
                }
            }
        }

        Vector3 nodeMasterPosition = nodeMaster.transform.position;
        nodeMasterPosition.x -= (gridSize * nodeSpacing) / 2;
        nodeMasterPosition.y -= (gridHeight * nodeSpacing) / 2;
        nodeMasterPosition.z -= (gridSize * nodeSpacing) / 2;

        nodeMaster.transform.position = nodeMasterPosition;

        //Kill the illegal ones

        //Check all nodes that are inside a collider, and kill them.

        //Raycast from above and below. if both hit enviro triggers, kill it. 
        for (int i = 0; i < nodes.Count; i++)
        {
            RaycastHit hit;

            
            if (
                Physics.Raycast(nodes[i].transform.position + (nodes[i].transform.TransformDirection(Vector3.down) * 1000), nodes[i].transform.TransformDirection(Vector3.up), out hit, 1000, nodeLayerMask) &&
                Physics.Raycast(nodes[i].transform.position + (nodes[i].transform.TransformDirection(Vector3.up) * 1000), nodes[i].transform.TransformDirection(Vector3.down), out hit, 1000, nodeLayerMask)
                )
            {

                KillNode(i);
                
            }

            //raycast down and if it hits catchall, kill it
            if (Physics.Raycast(nodes[i].transform.position, nodes[i].transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, blankSpaceLayerMask))
            {
                if (hit.collider.isTrigger || hit.collider.gameObject.layer == 17)
                {
                    KillNode(i);
                }
            }

            //Check if the node is inside an impassble collider (e.g. barriers, buildings etc)
            

        }

        //Second check for after

        //Of the remaining nodes, raycast to see if they are just above a collider that isn't a node, and kill the rest. 


        for (int i = 0; i < nodes.Count; i++)
        {
            RaycastHit hit1;
            //Check its not superfluous (too high)
            nodes[i].GetComponent<Collider>().enabled = false;
            

            if (nodes[i].GetComponent<Node>().GetAlive() && Physics.Raycast(nodes[i].transform.position, nodes[i].transform.TransformDirection(Vector3.down), out hit1, Mathf.Infinity, verticalSpaceLayerMask))
            {

                KillNode(i);
            }
            nodes[i].GetComponent<Collider>().enabled = true;
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            //Check if the node is inside an impassble collider (e.g. barriers, buildings etc)
            if (
              Physics.Raycast(nodes[i].transform.position + (nodes[i].transform.TransformDirection(Vector3.down) * 1000), nodes[i].transform.TransformDirection(Vector3.up),  1000, impassableLayerMask) &&
              Physics.Raycast(nodes[i].transform.position + (nodes[i].transform.TransformDirection(Vector3.up) * 1000), nodes[i].transform.TransformDirection(Vector3.down), 1000, impassableLayerMask)
              )
            {

                KillNode(i);

            }

        }

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {

                bool isAliveNode = false;
                Node aliveNode = null;
                for (int k = 0; k < gridHeight; k++)
                {
                    //The logic here is to create a 2D node grid not taking into account the Y element which does exist in practice.
                    //This is done by making a new grid removing the third dimension of the reference in the list.
                    //on every 'y' there should only be one alive node, or none.
                    if(instantiateNodeGrid[i][k][j].GetAlive())
                    {
                        isAliveNode = true;
                        aliveNode = instantiateNodeGrid[i][k][j];
                    }    
                }
                if (isAliveNode)
                {
                    aliveNode.gridIndex = new Vector2Int(i, j);
                    nodeGrid[i].nodeCol.Add(aliveNode);
                }
                else
                {
                    nodeGrid[i].nodeCol.Add(null);
                }
                
            }
        }


        for (int i = 0; i < nodes.Count; i++)
        {
            DestroyImmediate(nodes[i].GetComponent<BoxCollider>());
            if(nodes[i].GetComponent<Node>().GetAlive())
            {
                aliveNodes.Add(nodes[i]);
            }
            else
            {

                DestroyImmediate(nodes[i].gameObject);
                //deadNodes.Add(nodes[i]);
            }
        }


        instantiateNodeGrid.Clear();
    } 

    void KillNode(int index)
    {
        nodes[index].GetComponent<MeshRenderer>().material = killMat;
        nodes[index].GetComponent<MeshRenderer>().enabled = false;
        nodes[index].GetComponent<Node>().SetAlive(false);

        nodes[index].gameObject.layer = LayerMask.NameToLayer("DeadNode");

        deadNodes.Add(nodes[index]);
    }

    public void DestroyAllNodes()
    {
        nodeMaster.transform.localPosition = Vector3.zero;
        int l = 0;
        foreach (Node node in nodes)
        {
            if(nodes[l] != null)
            {
                DestroyImmediate(nodes[l].gameObject);
            }
            
            l++;
        }

        nodes.Clear();
        aliveNodes.Clear();
        deadNodes.Clear();
        //nodeGrid.Clear();
        //instantiateNodeGrid.Clear();

    }

    void CheckEndOfRoom()
    {
        if(spawningFinished && spawnedEnemies.Count <= 0)
        {
            roomComplete = true;
            //Aydens Audio
            if (audioManager)
            {
                audioManager.SetCurrentStateToFadeOutAudio1();
            }
            if(!bossSaim)
            {
                Destroy(this);
            }
	            
        }
    }

    bool CheckSpawnConditions()
    {
        if(spawningFinished)
        {
            return false;
        }

        if (!triggered)
        {
            return false;
        }

	    if (spawnAmount >= data.GetTotalSpawns())
        {
            spawningFinished = true;
            return false;
        }

	    if (timeInSpawning >= data.GetSpawnDuration())
        {
            spawningFinished = true;
            return false;
        }

	    if (spawnTimer >= data.GetTotalSpawnTimer())
        {
            return true;
        }

	    if(spawnedEnemies.Count <= data.GetEnemyMinimum())
        {
            return true;
        }

        return false;
    }

    //if there is a spawn event, use this to spawn the enemies randomly.
    public void Spawn(int amountToSpawn)
    {
        List<Node> spawnNodes = new List<Node>();

        foreach (Transform spawnNode in transform.Find("SpawnPositions"))
        {
            spawnNodes.Add(spawnNode.GetComponent<Node>());
        }

        for (int i = 0; i < amountToSpawn; i++)
        {
	        Vector3 spawnPosition = spawnNodes[UnityEngine.Random.Range(0, spawnNodes.Count - 1 )].transform.position;

            spawnPosition.x += Random.Range(-1.0f, 2.0f);
            spawnPosition.z += Random.Range(-1.0f, 2.0f);
            spawnPosition.y += 2;

            ChooseEnemy();

            GameObject spawnedEnemy = SetSpawn(data.GetEnemyList()[Random.Range(0, data.GetEnemyList().Count)], spawnPosition);
	        spawnedEnemy.GetComponent<BaseEnemyClass>().SetSpawner(this.gameObject);

	        if(QuestManager.GetQuestManager())
            {
		        QuestManager.GetQuestManager().SpawnUpdate(spawnedEnemy, "Regular");
            }
            
	        foreach (Item item in PlayerClass.GetPlayerClass().GetHeldItems())
            {
                item.SpawnTrigger(spawnedEnemy);
            }

            spawnedEnemies.Add(spawnedEnemy.GetComponent<BaseEnemyClass>());
            spawnAmount++;
        }
        // Aydens Audio
        if(audioManager)
        {
            audioManager.SetCurrentStateToFadeOutAudio2();
        }
    }

    //use this overload to spawn enemies of a certain type more specifically.
    public void Spawn(int amountToSpawn, int spawnTypeIndex)
    {
        List<Node> spawnNodes = new List<Node>();

        foreach (Transform spawnNode in transform.Find("SpawnPositions"))
        {
            spawnNodes.Add(spawnNode.GetComponent<Node>());
        }

        for (int i = 0; i < amountToSpawn; i++)
        {
            Vector3 spawnPosition = spawnNodes[Random.Range(0, spawnNodes.Count - 1)].transform.position;

            spawnPosition.x += Random.Range(-1.0f, 2.0f);
            spawnPosition.z += Random.Range(-1.0f, 2.0f);
            spawnPosition.y += 2;

           // GameObject spawnedEnemy = Instantiate(data.GetEnemyList()[spawnTypeIndex], spawnPosition, Quaternion.identity);
            GameObject spawnedEnemy = SetSpawn(data.GetEnemyList()[spawnTypeIndex], spawnPosition);
            spawnedEnemy.GetComponent<BaseEnemyClass>().SetSpawner(this.gameObject);

	        if (QuestManager.GetQuestManager())
            {
		        QuestManager.GetQuestManager().SpawnUpdate(spawnedEnemy, "Regular");
            }

	        foreach (Item item in PlayerClass.GetPlayerClass().GetHeldItems())
            {
                item.SpawnTrigger(spawnedEnemy);
            }

            spawnedEnemies.Add(spawnedEnemy.GetComponent<BaseEnemyClass>());
            spawnAmount++;
        }
        // Aydens Audio
        if (audioManager)
        {
            audioManager.SetCurrentStateToFadeOutAudio2();
        }
    }
	//Spawn override using squad system
	public void Spawn(int squadsToSpawn, bool overrider)
	{
		List<Node> spawnNodes = new List<Node>();

		foreach (Transform spawnNode in transform.Find("SpawnPositions"))
		{
			spawnNodes.Add(spawnNode.GetComponent<Node>());
		}
		
		int iterator = 0;
		
		for(int i = 0; i < squadsToSpawn; i++)
		{
			EnemySquad squad = ChooseSquad();

			foreach(GameObject enemy in squad.enemies)
			{
				Vector3 spawnPosition = spawnNodes[iterator % 4].transform.position;
				spawnPosition.x += Random.Range(-1.0f, 2.0f);
				spawnPosition.z += Random.Range(-1.0f, 2.0f);
				spawnPosition.y += 2;
				
				GameObject spawnedEnemy = GetSpawn(enemy);
				GameObject spawnedMask = SetSpawnEffect(squad.element, spawnPosition);
				spawnedEnemy.GetComponent<BaseEnemyClass>().SetSpawner(this.gameObject);
				spawnedMask.GetComponent<SpawnerScript>().SetEnemy(spawnedEnemy);
				spawnedMask.GetComponent<SpawnerScript>().StartSpawn();

				if (QuestManager.GetQuestManager())
				{
					QuestManager.GetQuestManager().SpawnUpdate(spawnedEnemy, "Regular");
				}

				foreach (Item item in PlayerClass.GetPlayerClass().GetHeldItems())
				{
					item.SpawnTrigger(spawnedEnemy);
				}

				spawnedEnemies.Add(spawnedEnemy.GetComponent<BaseEnemyClass>());
				spawnAmount++;
				iterator++;
			}
		}
		// Aydens Audio
		if (audioManager)
		{
			audioManager.SetCurrentStateToFadeOutAudio2();
		}

	}
	

    
    //Given a SAIM data object, will manually spawn an enemy of each type on that object.
    public void ManualSpawn()
    {
        List<Node> spawnNodes = new List<Node>();

        foreach (Transform spawnNode in transform.Find("SpawnPositions"))
        {
            spawnNodes.Add(spawnNode.GetComponent<Node>());
        }

        foreach (GameObject eType in data.GetEnemyList())
        {
            Vector3 spawnPosition = spawnNodes[Random.Range(0, spawnNodes.Count - 1)].transform.position;

            spawnPosition.x += Random.Range(-1.0f, 2.0f);
            spawnPosition.z += Random.Range(-1.0f, 2.0f);
            spawnPosition.y += 2;

            //GameObject spawnedEnemy = Instantiate(eType, spawnPosition, Quaternion.identity);
            GameObject spawnedEnemy = SetSpawn(eType, spawnPosition);
            spawnedEnemy.GetComponent<BaseEnemyClass>().SetSpawner(gameObject);

            if (QuestManager.GetQuestManager())
            {
                QuestManager.GetQuestManager().SpawnUpdate(spawnedEnemy, "Regular");
            }

            foreach (Item item in PlayerClass.GetPlayerClass().GetHeldItems())
            {
                item.SpawnTrigger(spawnedEnemy);
            }

            spawnedEnemies.Add(spawnedEnemy.GetComponent<BaseEnemyClass>());
            spawnAmount++;
        }
        // Aydens Audio
        if (audioManager)
        {
            audioManager.SetCurrentStateToFadeOutAudio2();
        }

        triggered = true;
    }

    //Use object pooling on a new object to spawn it
    public GameObject SetSpawn(GameObject eType, Vector3 position)
    {
        GameObject newEnem = ePool.GetReadiedEnemy(eType);

        if(newEnem == null)
        {
            newEnem = Instantiate(eType, position, Quaternion.identity);
        }
        else
        {
            newEnem.transform.position = position;
            newEnem.SetActive(true);
            
        }

        return newEnem;
    }
    
	//Get an enemy from the object pool without spawning it
	public GameObject GetSpawn(GameObject eType)
	{
		GameObject newEnem = ePool.GetReadiedEnemy(eType);

		if(newEnem == null)
		{
			newEnem = Instantiate(eType);
			newEnem.SetActive(false);
		}

		return newEnem;
	}
	//Use object pooling on a new spawn effect object to spawn it
	GameObject SetSpawnEffect(BaseEnemyClass.Types element, Vector3 position)
	{
		GameObject newMask = mPool.GetReadiedMask(element);

		if(newMask == null)
		{
			//newEnem = Instantiate(eType, position, Quaternion.identity);
		}
		else
		{
			newMask.transform.position = position;
			newMask.SetActive(true);
            
		}

		return newMask;
	}

    public int ChooseEnemy()
    {
        if(fireUse > crystalUse)
        {
            return Mathf.Min(Random.Range(0, data.GetEnemyList().Count), Random.Range(0, data.GetEnemyList().Count));
        }
        else
        {
            return Mathf.Max(Random.Range(0, data.GetEnemyList().Count), Random.Range(0, data.GetEnemyList().Count));
        }
    }
    
	public EnemySquad ChooseSquad()
	{
		float rand = Random.Range(0.0f, 1.0f);
		if(rand > 0.5f)
		{
			return data.GetSquadsList()[Random.Range(0, data.GetSquadsList().Count)];
		}
		else if (rand > 0.3f)
		{
			//Select weak element
			bool searching = true;
			while(searching)
			{
				EnemySquad squad = data.GetSquadsList()[Random.Range(0, data.GetSquadsList().Count)];
				if(fireUse >= crystalUse && fireUse >= waterUse && squad.element == BaseEnemyClass.Types.Crystal)
				{
					searching = false;
					return squad;
				}
				else if(crystalUse >= fireUse && crystalUse >= waterUse && squad.element == BaseEnemyClass.Types.Water)
				{
					searching = false;
					return squad;
				}
				else if(waterUse >= crystalUse && waterUse >= fireUse && squad.element == BaseEnemyClass.Types.Fire)
				{
					searching = false;
					return squad;
				}
			}
			return data.GetSquadsList()[0];
		}
		else
		{
			//Select strong element
			bool searching = true;
			while(searching)
			{
				EnemySquad squad = data.GetSquadsList()[Random.Range(0, data.GetSquadsList().Count)];
				if(fireUse >= crystalUse && fireUse >= waterUse && squad.element == BaseEnemyClass.Types.Water)
				{
					searching = false;
					return squad;
				}
				else if(crystalUse >= fireUse && crystalUse >= waterUse && squad.element == BaseEnemyClass.Types.Fire)
				{
					searching = false;
					return squad;
				}
				else if(waterUse >= crystalUse && waterUse >= fireUse && squad.element == BaseEnemyClass.Types.Crystal)
				{
					searching = false;
					return squad;
				}
			}
			return data.GetSquadsList()[0];

		}
	}

    public void SelectSpawnNode()
    {
        //Check the node is in front of the player, not overlapping an existing enemy or soon to be spawned enemy.
    }

    public void Move()
    {
        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            Bounce(i);
            
        }
    }

    public void Bounce(int elementIndex)
    {

        if(!spawnedEnemies[elementIndex])
        {

            return;
        }

        //Bouncing away from each other
        for (int j = 0; j < spawnedEnemies[elementIndex].GetComponent<BaseEnemyClass>().GetBounceList().Count; j++)
        {
            if (spawnedEnemies[elementIndex].GetComponent<BaseEnemyClass>().GetBounceList()[j] == null)
            {
                spawnedEnemies[elementIndex].GetComponent<BaseEnemyClass>().GetBounceList().RemoveAt(j);
            }
            else 
            {
                Vector3 newDir = spawnedEnemies[elementIndex].GetComponent<BaseEnemyClass>().GetBounceList()[j].transform.position - spawnedEnemies[elementIndex].gameObject.transform.position;
                newDir.y = 0;
                if (newDir.magnitude == 0)
                {
                    newDir = new Vector3(Random.Range(-1, 2), 0, Random.Range(-1, 2));
                    newDir = newDir.normalized;

                }
                //spawnedEnemies[elementIndex].gameObject.GetComponent<Rigidbody>().AddForce(-newDir * Time.deltaTime * 1000);

                spawnedEnemies[elementIndex].gameObject.transform.position += -newDir * Time.deltaTime * 0.1f;

                spawnedEnemies[elementIndex].GetComponent<BaseEnemyClass>().GetBounceList().RemoveAt(j);
            }

            
            j--;

        }
    }

    //Given a target location, give out nodes the values which will eventually dictate direction
    public void CreateIntegrationFlowField()
    {
        foreach (Node node in aliveNodes)
        {
            node.ResetNode();
        }


        destinationNode = GetPlayerNode();

        destinationNode.SetDestination();

        Queue<Node> nodesToCheck = new Queue<Node>();

        nodesToCheck.Enqueue(destinationNode);

        //Grow the queue as we check local nodes, finishing once all nodes are checked, starting with the destination node.
        while(nodesToCheck.Count > 0)
        {
            Node currentNode = nodesToCheck.Dequeue();
            //List<Node> currentNeighbours = GetNeighbourNodes(currentNode, false);

            foreach (Node node in currentNode.neighbourNodes)
            {

                if (node.cost == int.MaxValue )
                {
                    continue;
                }
                


	            if(CheckHeightDifference(currentNode, node))//|| CollisonCull(currentNode, node))
                {
                    continue;
                }

                //If the neigbour node being checked has a higher best cost than the current node's best cost plus this neigbour node's best cost,
                //change it's best cost to that value and enque it to become the next node to be checked. 
                //This will stop the algo backtracking, as long as max best cost is sufficiently high enough.
                if(node.cost + currentNode.bestCost < node.bestCost)
                {
                    node.bestCost = node.cost + currentNode.bestCost;
                    nodesToCheck.Enqueue(node);
                }

            }
        }

    }

    //If there is a height difference (going up), return true, otherwise false
    bool CheckHeightDifference(Node mainNode, Node neighbourNode)
    {
        if (mainNode.position.y > neighbourNode.position.y + allowableHeightDifference
            || mainNode.position.y < neighbourNode.position.y - allowableHeightDifference)
        {
            return true;
        }

        return false;
    }

    //If the node goes through a wall, dont connect it. This is to account for thin walls not otherwise picked up on by the placed nodes
    bool CollisonCull(Node mainNode, Node neighbourNode)
    {

        if (Physics.Raycast(mainNode.transform.position, neighbourNode.transform.position - mainNode.transform.position, (neighbourNode.transform.position - mainNode.transform.position).magnitude, cullLayerMask))
        {
            return true;
        }

        return false;
    }

    //Using the integrated field, generate the flow field by pointing each node at the next node
    void GenerateFlowField()
    {

        //Iterate through each node
        foreach (Node currentNode in aliveNodes)
        {
            //List<Node> currentNodeNeigbours = GetNeighbourNodes(currentNode, true);

            int bestCost = currentNode.bestCost;

            //Look at the node's neigbours to decide which to 'point' at.
            //This will be the node with the lowest bestCost.
            foreach (Node currentNeigbourNode in currentNode.cornerNodes)
            {
                if(currentNeigbourNode.bestCost < bestCost /*&& !CheckHeightDifference(currentNode, currentNeigbourNode) && !CollisonCull(currentNode, currentNeigbourNode)*/)
                {
                    bestCost = currentNeigbourNode.bestCost;
                    currentNode.bestNextNodePos = currentNeigbourNode.position;

                }
            }
        }
    }

    //Gets the north south east and west nodes of a given node, plus diags if true
    void SetNeighbourNodes(Node nodeCentre, bool isDiag)
    {
        //List<Node> neigbours = new List<Node>();

        for (int i = -1; i < 2; i++)
        {
            for (int k = -1; k < 2; k++) 
            {

                //Checks whether it is itself or is null (which would be an edge for instance)
                if(k == 0 && i == 0)
                {

                }
                //If not getting diagonals
                else if (!isDiag && k != 0 && i != 0)
                {

                }
                else
                {
                    if(nodeCentre.gridIndex.x + i >= gridSize || nodeCentre.gridIndex.x + i < 0 ||
                       nodeCentre.gridIndex.y + k >= gridSize || nodeCentre.gridIndex.y + k < 0)
                    {

                    }
                    else if(nodeGrid[nodeCentre.gridIndex.x+i].nodeCol[nodeCentre.gridIndex.y+k] != null)
                    {

                        if (CheckHeightDifference(nodeCentre, nodeGrid[nodeCentre.gridIndex.x + i].nodeCol[nodeCentre.gridIndex.y + k]) || CollisonCull(nodeCentre, nodeGrid[nodeCentre.gridIndex.x + i].nodeCol[nodeCentre.gridIndex.y + k]))
                        {
                            continue;
                        }
                        if(isDiag)
                        {
                            nodeCentre.cornerNodes.Add(nodeGrid[nodeCentre.gridIndex.x + i].nodeCol[nodeCentre.gridIndex.y + k]);
                        }
                        else
                        {
                            nodeCentre.neighbourNodes.Add(nodeGrid[nodeCentre.gridIndex.x + i].nodeCol[nodeCentre.gridIndex.y + k]);
                        }
                        
                    }
                    
                }
            }
        }

    }

    //Check every frame and adjust variables accordingly
    public void AdjustDifficulty()
    {
        //Check if the player has taken a significant amount of damage over a period of time
        if(previousHealth > player.GetComponent<PlayerClass>().GetCurrentHealth())
        {
            currentDamageTaken += previousHealth - player.GetComponent<PlayerClass>().GetCurrentHealth();
        }
        previousHealth = player.GetComponent<PlayerClass>().GetCurrentHealth();

        //
        diffAdjTimerDAM -= Time.deltaTime;
        if(diffAdjTimerDAM < 0)
        {
	        if(currentDamageTaken >= data.GetPlayerDamageThreshold())
            {
                //If so, reduce diff.
		        data.SetAdjustedDifficulty(data.GetAdjustedDifficulty() - 1);
                Debug.Log("Diff Down!" + data.GetAdjustedDifficulty());
            }

	        diffAdjTimerDAM = data.GetAdjustTimerDamage();
            currentDamageTaken = 0;

        }

        //See how many enemies the player has killed in that time. If it's a lot, adj diff up.

        if(previousEnemyCount > spawnedEnemies.Count)
        {
            currentKills += previousEnemyCount - spawnedEnemies.Count;
        }

        previousEnemyCount = spawnedEnemies.Count;

        diffAdjTimerKIL -= Time.deltaTime;
        if (diffAdjTimerKIL < 0)
        {
	        if (currentKills >= data.GetEnemyKillThreshold())
            {
                //If so, reduce diff.
		        data.SetAdjustedDifficulty(data.GetAdjustedDifficulty() + 1);
                Debug.Log("Diff Up!" + data.GetAdjustedDifficulty());
            }

	        diffAdjTimerKIL = data.GetAdjustTimerKills();
            currentKills = 0;

        }

        //See difficulty difference and make changes to spawning and behaviour as appropriate.
        SetBasedOnDiffculty();

    }

    //Sets the variables that control actual difficulty (spawn rates for example) based on the diff variables
    void SetBasedOnDiffculty()
	{ 
	    if(data.GetAdjustedDifficulty() > 10)
        {
		    data.SetAdjustedDifficulty(10);
            Debug.Log("Diff capped!");
        }

	    int actualDiff = data.GetDifficulty() + (data.GetAdjustedDifficulty() < 1 ? 1 : data.GetAdjustedDifficulty());

        if(actualDiff < 5)
        {
	        data.SetMaxSpawns(3);
	        data.SetMinSpawns(1);
        }
        else if (actualDiff < 10)
        {
	        data.SetMaxSpawns(6);
	        data.SetMinSpawns(2);
        }
        else if (actualDiff < 15)
        {
	        data.SetMaxSpawns(10);
	        data.SetMinSpawns(5);
        }
        else if (actualDiff < 20)
        {
	        data.SetMaxSpawns(20);
	        data.SetMinSpawns(10);
        }


    }
    
    public Node GetPlayerNode()
    {
        return playerNode;
    }
    
	public static void AddFireUse()
	{
		fireUse += 1;
	}
	
	public static void AddCrystalUse()
	{
		crystalUse += 1;
	}
	
	public static void AddWaterUse()
	{
		waterUse += 1;
	}

}


//public struct FlowfieldJob : IJob
//{

//    List<Node> aliveNodes;
//    Node destinationNode;
		
//	public FlowfieldJob()
//	{
			
//	}
		
//	public void Execute()
//	{
//        foreach (Node node in aliveNodes)
//        {
//            node.ResetNode();
//        }


//        destinationNode = ;

//        destinationNode.SetDestination();

//        Queue<Node> nodesToCheck = new Queue<Node>();

//        nodesToCheck.Enqueue(destinationNode);

//        //Grow the queue as we check local nodes, finishing once all nodes are checked, starting with the destination node.
//        while (nodesToCheck.Count > 0)
//        {
//            Node currentNode = nodesToCheck.Dequeue();
//            List<Node> currentNeighbours = GetNeighbourNodes(currentNode, false);

//            foreach (Node node in currentNeighbours)
//            {
//                //If the neighbour is a wall or other impassable terrain, straight up ignore it and move on

//                //Adjust for height here
//                if (node.cost == int.MaxValue)
//                {
//                    continue;
//                }


//                if (CheckHeightDifference(currentNode, node) || CollisonCull(currentNode, node))
//                {
//                    continue;
//                }

//                //If the neigbour node being checked has a higher best cost than the current node's best cost plus this neigbour node's best cost,
//                //change it's best cost to that value and enque it to become the next node to be checked. 
//                //This will stop the algo backtracking, as long as max best cost is sufficiently high enough.
//                if (node.cost + currentNode.bestCost < node.bestCost)
//                {
//                    node.bestCost = node.cost + currentNode.bestCost;
//                    nodesToCheck.Enqueue(node);
//                }

//            }
//        }
//    }

//    bool CheckHeightDifference(Node mainNode, Node neighbourNode)
//    {
//        if (mainNode.transform.position.y > neighbourNode.transform.position.y + allowableHeightDifference
//            || mainNode.transform.position.y < neighbourNode.transform.position.y - allowableHeightDifference)
//        {
//            return true;
//        }

//        return false;
//    }
//}
