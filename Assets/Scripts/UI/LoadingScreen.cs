using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
	[SerializeField] Image loadBar;
	static int sceneToLoad = 2;
	static AsyncOperation operation = null;
	float progress = 0.0f;
	[SerializeField] float opProg;

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
		//Turns off the player and the UI if they exists so they don't appear in the loading screen
		if(PlayerClass.GetPlayerClass())
		{
			PlayerClass.GetPlayerClass().gameObject.SetActive(false);
		}
		if(GameplayUI.GetGameplayUI())
		{
			GameplayUI.GetGameplayUI().gameObject.SetActive(false);
		}
		//If there isn't a scene loading already, start loading one
		if(operation == null)
		{
			StartSceneLoad();
		}
		else if(operation.isDone)
		{
			operation.allowSceneActivation = true;
		}

	}
	
	// Update is called every frame, if the MonoBehaviour is enabled.
	protected void Update()
	{
		progress += Time.deltaTime / 10;
		loadBar.fillAmount = progress;
		opProg = operation.progress;
		if(!operation.isDone)
		{
			if (operation.progress >= 0.9f && progress >= 1)
			{
				operation.allowSceneActivation = true;
				Debug.Log("Activate");
				operation = null;
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
		//Debug.Log(operation);
		operation = SceneManager.LoadSceneAsync(sceneToLoad);
		//Debug.Log(operation);
		operation.allowSceneActivation = false;
		//Debug.Log(sceneToLoad);
		//SceneManager.LoadScene(sceneToLoad);
	}

	public static float GetProgress()
	{
		return operation.progress;
	}
}
