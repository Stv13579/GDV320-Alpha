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
	}
	
	public void CloseOptions()
	{
		mainMenu.SetActive(true);
		options.GetComponent<OptionsMenuScript>().SaveSettings();
		options.SetActive(false);
		
	}
}
