using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPreloaderScript : MonoBehaviour
{
	AsyncOperation operation;
	[SerializeField]
	int sceneToLoad;
    // Start is called before the first frame update
    void Start()
    {
	    StartCoroutine(StartSceneLoad(sceneToLoad));
    }
    
	public IEnumerator StartSceneLoad(int scene)
	{
		operation = SceneManager.LoadSceneAsync(scene);

		operation.allowSceneActivation = false;

		while(!operation.isDone)
		{
			yield return null;
		}

	}
	
	public void LoadScene()
	{
		operation.allowSceneActivation = true;
	}
}
