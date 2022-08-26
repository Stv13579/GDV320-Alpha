using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameController : MonoBehaviour
{
    AudioManager audioManager;
    public void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    public void StartGame()
    {
        if (audioManager)
        {
            audioManager.StopSFX("Menu and Pause");
            audioManager.PlaySFX("Menu and Pause");
            audioManager.StopMusic(audioManager.GetInitialMusic());
            audioManager.PlayMusic("Hub Room Music");
        }
        SceneManager.LoadScene(1);
    }
}
