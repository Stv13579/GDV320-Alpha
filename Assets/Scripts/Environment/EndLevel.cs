using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    
    int sceneToLoad;
    [SerializeField]
    GameObject loadingScreen;

    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");

    }


    private void OnTriggerEnter(Collider other)
    {
        //GameObject screen = Instantiate(loadingScreen);
        int index = 0;

        //Temporary scene system; consider upgrading to dynamically building scenes and refering to scenes by name with a dedicated scene manager/ level manager
        index = SceneManager.GetActiveScene().buildIndex;

        index++;

        if (index > SceneManager.sceneCountInBuildSettings)
        {
            index = 0;
            Destroy(player);
        }

        sceneToLoad = index;

        SceneManager.LoadScene(sceneToLoad);

        //StartCoroutine(screen.GetComponent<LoadingScreen>().LoadScene(sceneToLoad));
    }
}
