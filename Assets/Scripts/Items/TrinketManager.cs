using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinketManager : MonoBehaviour
{
	static bool exists = false;
	
	// Awake is called when the script instance is being loaded.
	protected void Awake()
	{
		if(exists)
		{
			Destroy(this.gameObject);
		}
	}
    void Start()
    {
	    DontDestroyOnLoad(gameObject);
	    exists = true;
    }




}
