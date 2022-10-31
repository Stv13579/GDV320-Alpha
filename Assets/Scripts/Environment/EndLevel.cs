using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    [SerializeField]
    bool interactable;
    public void SetInteractable(bool inter) { interactable = inter; }
    int sceneToLoad;

    GameObject player;

    AudioManager audioManager;

    [SerializeField]
    int numberOfLevels;

    [SerializeField]
    GameObject portalAnim;

    private void Start()
    {
	    player = PlayerClass.GetPlayerClass().gameObject;
	    audioManager = AudioManager.GetAudioManager();
        if(!interactable)
        {
            portalAnim.GetComponent<Animator>().SetTrigger("Open");
            portalAnim.GetComponent<Animator>().ResetTrigger("Close");
            if (audioManager)
            {
                audioManager.PlaySFX("Portal Opening", player.transform);
                audioManager.PlaySFX("Portal Idle", player.transform);
            }
        }
    }
    public int GetSceneToLoad() { return sceneToLoad; }

    public void EndCurrentLevel()
    {//Update quests as the player has just finished the room
	    QuestManager.GetQuestManager().FinishRoomUpdate();


        //GameObject screen = Instantiate(loadingScreen);
        int index = 0;

        //Temporary scene system; consider upgrading to dynamically building scenes and refering to scenes by name with a dedicated scene manager/ level manager
        index = SceneManager.GetActiveScene().buildIndex;

	    if(index == 1)
	    {
	    	player.GetComponent<PlayerClass>().SubtractMoney(player.GetComponent<PlayerClass>().GetMoney());
	    }
		
        index++;

        if (index > numberOfLevels)
        {
            index = 1;
            Destroy(player);
            if(FindObjectOfType<TrinketManager>())
                Destroy(FindObjectOfType<TrinketManager>().gameObject);
	        Destroy(GameplayUI.GetGameplayUI().gameObject);
            //Save the game
            SaveSystem.SaveNPCData((NPCData)Resources.Load("NPCs/Lotl"));
            SaveSystem.SaveNPCData((NPCData)Resources.Load("NPCs/Blacksmith"));
            SaveSystem.SaveNPCData((NPCData)Resources.Load("NPCs/Fortune"));
	        SaveSystem.SaveNPCData((NPCData)Resources.Load("NPCs/Shop"));
	        QuestManager.GetQuestManager().FinishRunUpdate();

        }
        player.GetComponent<Shooting>().SetLoadScene(true);
        sceneToLoad = index;
		

	    if(SceneManager.GetActiveScene().buildIndex < 4)
	    {
		    LoadingScreen.SetSceneToLoad(SceneManager.GetActiveScene().buildIndex + 1);
	    }
	    else
	    {
		    LoadingScreen.SetSceneToLoad(1);
	    }
	    SceneManager.LoadScene(6);


        //StartCoroutine(screen.GetComponent<LoadingScreen>().LoadScene(sceneToLoad));

        // chooses the music for different scenes
        // e.g. if the player is entering scene 1 then stop all music and play the hub music
        if (audioManager)
        {
            if (sceneToLoad == 1)
            {
                for (int i = 0; i < audioManager.GetMusics().Length; i++)
                {
                    audioManager.GetMusics()[i].GetAudioSource().Stop();
                }
                audioManager.PlayMusic("Hub Room Music");
            }
            else
            {
                for (int i = 0; i < audioManager.GetMusics().Length; i++)
                {
                    audioManager.GetMusics()[i].GetAudioSource().Stop();
                }
                audioManager.PlayMusic($"Level {sceneToLoad - 1} Non Combat");
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !interactable)
        {
            if (audioManager)
            {
                audioManager.StopSFX("Portal Idle");
            }
            EndCurrentLevel();
        }
    }
}
