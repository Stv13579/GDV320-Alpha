﻿using System.Collections;
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
        }
    }
}
