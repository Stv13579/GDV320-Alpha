using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!SaveSystem.LoadStartedState())
        {
            Debug.Log("First Time in hub");
            SaveSystem.SaveFirstLoad();
        }
    }

    
}
