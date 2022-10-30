using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinketManager : MonoBehaviour
{
	static bool exists = false;
	static TrinketManager currentTrinketManager;
	// Awake is called when the script instance is being loaded.
	protected void Awake()
	{

	}
    void Start()
    {
		DontDestroyOnLoad(gameObject);
		exists = true;
		currentTrinketManager = this;
    }
    
	public static TrinketManager GetTrinketManager()
	{
		return currentTrinketManager;
	}




}
