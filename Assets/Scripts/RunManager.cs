using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunManager : MonoBehaviour
{
	static RunManager currentRunManager;
    GameObject player;
    bool lastLevel;
    int sceneIndex;

    public int GetSceneIndex() { return sceneIndex; }

    // Start is called before the first frame update
    void Start()
    {
	    player = PlayerClass.GetPlayerClass().gameObject;
	    QuestManager.GetQuestManager().StartRunUpdate();
        //DontDestroyOnLoad(gameObject);
    }

    void Awake()
    {
        player = PlayerClass.GetPlayerClass().gameObject;
	    sceneIndex = SceneManager.GetActiveScene().buildIndex;
	    currentRunManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewLevel()
    {
        player.transform.position = new Vector3(60, 5, 60);
        player.GetComponent<PlayerClass>().StartLevel();
        if(QuestManager.GetQuestManager())
        {
            QuestManager.GetQuestManager().StartLevelUpdate();
            QuestManager.GetQuestManager().inHub = false;
        }
        player.GetComponent<Shooting>().SetLoadOutChosen(true);

	    LevelGeneration.GetLevelGeneration().startRoom.GetComponentInChildren<SAIM>().triggered = true;
    }

    public void FinishRun()
    {
        if (QuestManager.GetQuestManager())
        {
            QuestManager.GetQuestManager().GetComponent<QuestManager>().FinishRunUpdate();
        }

        if(FindObjectOfType<BossRoom>())
        {
            FindObjectOfType<BossRoom>().GetList().ResetList();
        }

    }
    
	public static RunManager GetRunManager()
	{
		return currentRunManager;
	}
}
