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
	    SceneManager.LoadScene(0);
    }
}
