using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
	[SerializeField]
	GameObject options, mainMenu;
	AudioManager audioManager;

	private void Start()
    {
        options.GetComponent<OptionsMenuScript>().LoadSettings();
		audioManager = AudioManager.GetAudioManager();
	}
	public void OpenOptions()
	{
		mainMenu.SetActive(false);
		options.SetActive(true);
		options.GetComponent<OptionsMenuScript>().LoadSettings();
        options.GetComponent<OptionsMenuScript>().UpdateFOV();
        options.GetComponent<OptionsMenuScript>().UpdateMusicVolume();
        options.GetComponent<OptionsMenuScript>().UpdateSensitivity();
        options.GetComponent<OptionsMenuScript>().UpdateSoundVolume();
		if (audioManager)
		{
			audioManager.StopSFX("Menu and Pause");
			audioManager.PlaySFX("Menu and Pause");
		}
	}
	
	public void CloseOptions()
	{
		mainMenu.SetActive(true);
		options.GetComponent<OptionsMenuScript>().SaveSettings();
		options.SetActive(false);
		if (audioManager)
		{
			audioManager.StopSFX("Menu and Pause");
			audioManager.PlaySFX("Menu and Pause");
		}
	}
}
