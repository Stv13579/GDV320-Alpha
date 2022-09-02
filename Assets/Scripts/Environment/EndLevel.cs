using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{

    private int sceneToLoad;
    [SerializeField]
    private GameObject loadingScreen;

    private GameObject player;

    private AudioManager audioManager;
    private void Start()
    {
        player = GameObject.Find("Player");
        audioManager = FindObjectOfType<AudioManager>();
    }
    public int GetSceneToLoad() { return sceneToLoad; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Update quests as the player has just finished the room
            GameObject.Find("Quest Manager").GetComponent<QuestManager>().FinishRoomUpdate();


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

            // chooses the music for different scenes
            // e.g. if the player is entering scene 1 then stop all music and play the hub music
            if(audioManager)
            {
                if(sceneToLoad == 1)
                {
                    for (int i = 0; i < audioManager.GetMusics().Length; i++)
                    {
                        audioManager.GetMusics()[i].audioSource.Stop();
                    }
                    audioManager.PlayMusic("Hub Room Music");
                }
                else
                {
                    audioManager.StopMusic("Hub Room Music");
                    audioManager.PlayMusic($"Level {sceneToLoad - 1} Non Combat");
                }

            }
        }
    }
}
