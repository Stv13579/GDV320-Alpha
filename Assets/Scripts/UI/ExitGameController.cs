using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameController : MonoBehaviour
{
    AudioManager audioManager;
    public void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    public void EndGame()
    {
        audioManager.StopSFX("Menu and Pause");
        audioManager.PlaySFX("Menu and Pause");
        Application.Quit();
    }
}
