using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunManager : MonoBehaviour
{

    GameObject player;
    bool lastLevel;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        GameObject.Find("Quest Manager").GetComponent<QuestManager>().StartRunUpdate();
        DontDestroyOnLoad(player);
        //DontDestroyOnLoad(gameObject);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewLevel()
    {
        player.transform.position = new Vector3(60, 5, 60);
        player.GetComponent<PlayerClass>().StartLevel();
        if(GameObject.Find("Quest Manager"))
        {
            GameObject.Find("Quest Manager").GetComponent<QuestManager>().StartLevelUpdate();
            GameObject.Find("Quest Manager").GetComponent<QuestManager>().inHub = false;
        }

    }

    public void FinishRun()
    {
        if (GameObject.Find("Quest Manager"))
        {
            GameObject.Find("Quest Manager").GetComponent<QuestManager>().FinishRunUpdate();
        }
    }
}
