using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGameController : MonoBehaviour
{
    AudioManager audioManager;

    [SerializeField]
    bool isMainMenu = false;

    public void Start()
    {
	    audioManager = AudioManager.GetAudioManager();
    }
    public void EndGame()
    {
        if (audioManager)
        {
            audioManager.StopSFX("Menu and Pause");
            audioManager.PlaySFX("Menu and Pause");
        }
        //Save the game
        SaveSystem.SaveNPCData((NPCData)Resources.Load("NPCs/Lotl"));
        SaveSystem.SaveNPCData((NPCData)Resources.Load("NPCs/Blacksmith"));
        SaveSystem.SaveNPCData((NPCData)Resources.Load("NPCs/Fortune"));
        SaveSystem.SaveNPCData((NPCData)Resources.Load("NPCs/Shop"));
        if(isMainMenu)
        {
            Application.Quit();
        }
	    Time.timeScale = 1;
	    Destroy(PlayerClass.GetPlayer());
	    if(FindObjectOfType<TrinketManager>())
		    Destroy(FindObjectOfType<TrinketManager>().gameObject);
	    Destroy(GameplayUI.GetGameplayUI().gameObject);
	    SceneManager.LoadScene(0);
	    
    }
}
