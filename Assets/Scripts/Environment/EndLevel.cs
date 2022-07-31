using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    
    int sceneToLoad;
    [SerializeField]
    GameObject loadingScreen;

    private void OnTriggerEnter(Collider other)
    {
        GameObject screen = Instantiate(loadingScreen);

        //Temporary scene system; consider upgrading to dynamically building scenes and refering to scenes by name with a dedicated scene manager/ level manager
 
        int index = SceneManager.GetActiveScene().buildIndex;

        index++;

        if (index > SceneManager.sceneCountInBuildSettings)
        {
            index = 0;
        }

        sceneToLoad = index;

        StartCoroutine(screen.GetComponent<LoadingScreen>().LoadScene(sceneToLoad));
    }
}
