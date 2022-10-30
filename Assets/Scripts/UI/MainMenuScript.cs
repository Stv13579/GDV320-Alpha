using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
	[SerializeField]
	GameObject options, mainMenu;

    private void Start()
    {
        options.GetComponent<OptionsMenuScript>().LoadSettings();
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
    }
	
	public void CloseOptions()
	{
		mainMenu.SetActive(true);
		options.GetComponent<OptionsMenuScript>().SaveSettings();
		options.SetActive(false);
		
	}
}
