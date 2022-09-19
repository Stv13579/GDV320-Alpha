using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameController : MonoBehaviour
{
    AudioManager audioManager;

    [SerializeField]
    NPCData lilly, blaze, silvain, freya;

    public void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    public void StartGame()
    {

        LoadGame();

        if (audioManager)
        {
            audioManager.StopSFX("Menu and Pause");
            audioManager.PlaySFX("Menu and Pause");
            audioManager.StopMusic(audioManager.GetInitialMusic());
            audioManager.PlayMusic("Hub Room Music");
        }

        if (SaveSystem.LoadStartedState())
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(5);
        }

        
    }

    public void LoadGame()
    {
        //Get all npc data and equal it to the saved data, then initialise any lax variables
        lilly.LoadData(SaveSystem.LoadNPCData(lilly.name));
        blaze.LoadData(SaveSystem.LoadNPCData(blaze.name));
        silvain.LoadData(SaveSystem.LoadNPCData(silvain.name));
        freya.LoadData(SaveSystem.LoadNPCData(freya.name));

    }
}
