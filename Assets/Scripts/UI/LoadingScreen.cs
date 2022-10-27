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
	[SerializeField] float progress = 0.0f;

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
		PlayerClass.GetPlayerClass().gameObject.SetActive(false);
		GameplayUI.GetGameplayUI().gameObject.SetActive(false);

	}
	
	// Update is called every frame, if the MonoBehaviour is enabled.
	protected void Update()
	{
		progress += Time.deltaTime / 10;
		loadBar.fillAmount = progress;
		
		if(!operation.isDone)
		{
			if (operation.progress >= 0.9f && progress >= 1)
			{
				operation.allowSceneActivation = true;
			}
		}
	}
    
	public static void SetSceneToLoad(int newScene)
	{
		sceneToLoad = newScene;
	}
	
	public static void SetOperation(AsyncOperation newOperation)
	{
		operation = newOperation;
	}
	
	public static void StartSceneLoad()
	{
		operation = SceneManager.LoadSceneAsync(sceneToLoad);
		operation.allowSceneActivation = false;
	}
}
