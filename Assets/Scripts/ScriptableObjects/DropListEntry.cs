using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class DropListEntry
{
	[SerializeField]
	GameObject drop;
	[SerializeField]
	PlayerClass.ManaName element;
	[SerializeField]
	int weighting;
    
	public GameObject GetDrop()
	{
		return drop;
	}
	
	public PlayerClass.ManaName GetElement()
	{
		return element;
	}
	
	public int GetWeighting()
	{
		return weighting;
	}
}
