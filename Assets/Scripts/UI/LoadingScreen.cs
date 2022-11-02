using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
	[SerializeField] Image loadBar;
	[SerializeField] GameObject spinner;
	float rot = 0;
	static int sceneToLoad = 2;
	static AsyncOperation operation = null;
	float progress = 0.0f;

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
		//Turns off the player and the UI if they exists so they don't appear in the loading screen
		if(PlayerClass.GetPlayerClass())
		{
			PlayerClass.GetPlayerClass().gameObject.SetActive(false);
			PlayerClass.GetPlayerClass().gameObject.GetComponent<VoidElement>().SetPPVolumeWeight(0.0f);
		}
		if(GameplayUI.GetGameplayUI())
		{
			GameplayUI.GetGameplayUI().gameObject.SetActive(false);
		}
		//If there isn't a scene loading already, start loading one
		StartSceneLoad();


	}
	
	// Update is called every frame, if the MonoBehaviour is enabled.
	protected void Update()
	{
		progress += Time.deltaTime / 5;
		loadBar.fillAmount = operation.progress;
		rot -= Time.deltaTime * 100;
		spinner.transform.eulerAngles = new Vector3(0, 0, rot);
		if(!operation.isDone)
		{
			if (operation.progress >= 0.9f)
			{
				operation.allowSceneActivation = true;
			}
		}
	}
	public static void SetSceneToLoad(int newScene)
	{
		sceneToLoad = newScene;
	}
	
	//Starts loading sceneToLoad scene in the background
	public static void StartSceneLoad()
	{
		operation = SceneManager.LoadSceneAsync(sceneToLoad);
		operation.allowSceneActivation = false;
	}
}
