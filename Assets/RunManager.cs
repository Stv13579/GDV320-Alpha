using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunManager : MonoBehaviour
{

    GameObject player;
    bool lastLevel;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(gameObject);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewLevel()
    {
        player.transform.position = new Vector3(60, 5, 60);
    }
}
