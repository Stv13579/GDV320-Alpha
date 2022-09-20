using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunManager : MonoBehaviour
{

    GameObject player;
    bool lastLevel;
    int sceneIndex;

    public int GetSceneIndex() { return sceneIndex; }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        GameObject.Find("Quest Manager").GetComponent<QuestManager>().StartRunUpdate();
        //DontDestroyOnLoad(gameObject);
    }

    void Awake()
    {
        player = GameObject.Find("Player");
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
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
        player.GetComponent<Shooting>().SetLoadOutChosen(true);
    }

    public void FinishRun()
    {
        if (GameObject.Find("Quest Manager"))
        {
            GameObject.Find("Quest Manager").GetComponent<QuestManager>().FinishRunUpdate();
        }

        if(FindObjectOfType<BossRoom>())
        {
            FindObjectOfType<BossRoom>().GetList().ResetList();
        }

    }
}
