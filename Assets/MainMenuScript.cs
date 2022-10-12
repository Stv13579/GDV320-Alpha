using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
	[SerializeField]
	GameObject options, mainMenu;
	public void OpenOptions()
	{
		mainMenu.SetActive(false);
		options.SetActive(true);
	}
	
	public void CloseOptions()
	{
		mainMenu.SetActive(true);
		options.SetActive(false);
	}
}
