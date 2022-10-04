﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEditor;

[CreateAssetMenu(fileName = "Item List")]
//Scriptable Object to store all items in for access elsewhere
public class ItemList : ScriptableObject
{
	[SerializeField]
	List<ItemEntry> itemList;
    
	public void ResetList()
	{
		for(int i = 0; i < itemList.Count; i++)
		{
			itemList[i].alreadyAdded = false;
		}
		
	}
	
	public List<ItemEntry> GetItemList()
	{
		return itemList;
	}
	
}
