using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDeath : MonoBehaviour
{
    [SerializeField]
    GameObject regGOScreen;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
	        PlayerClass.GetPlayerClass().SetGameOverScreen(regGOScreen);
        }
    }
}
