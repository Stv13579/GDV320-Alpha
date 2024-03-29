﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drops List")]
//Scriptable Object to store all items enemies can drop for access elsewhere

public class DropsList : ScriptableObject
{
	[SerializeField]
	List<DropListEntry> currencyList;
	[SerializeField]
	List<DropListEntry> ammoList;
	[SerializeField]
    List<DropListEntry> healthList;
	
	[SerializeField]
	int minCurrencySpawn = 2, maxCurrencySpawn = 4;
	
	[SerializeField]
	int minAmmoSpawn = 0, maxAmmoSpawn = 2;
	
	[SerializeField]
	int minHealthSpawn = 0, maxHealthSpawn = 1;

    public GameObject GetDrop(List<DropListEntry> dropsList)
    {
        int totalWeight = 0;
        foreach(DropListEntry drop in dropsList)
        {
	        totalWeight += drop.GetWeighting();
        }
        int rand = Random.Range(1, totalWeight);
        int i = -1;
        while(rand > 0)
        {
            i++;
	        rand -= dropsList[i].GetWeighting();
            
        }
        if(i >= 8)
        {
            Debug.Log("8");
        }
	    return (dropsList[i].GetDrop());
    }

    public GameObject GetAmmoDrop()
    {
        //Get the cata and prime elements and add only the elements of that type to the list to choose from

        int index = 0;
        List<DropListEntry> dropsList = new List<DropListEntry>();

        foreach (DropListEntry manaType in ammoList)
        {
	        if(manaType.GetElement() == Shooting.GetShooting().GetCatalystElements()[0].GetManaName() 
                || manaType.GetElement() == Shooting.GetShooting().GetPrimaryElements()[0].GetManaName()
                || manaType.GetElement() == Shooting.GetShooting().GetCatalystElements()[1].GetManaName()
                || manaType.GetElement() == Shooting.GetShooting().GetPrimaryElements()[1].GetManaName())
            {
                dropsList.Add(manaType);
            }
            index++;
        }

        int totalWeight = 0;
        foreach (DropListEntry drop in dropsList)
        {
	        totalWeight += drop.GetWeighting();
        }
        int rand = Random.Range(1, totalWeight);
        int i = -1;
        while (rand > 0)
        {
            i++;
	        rand -= dropsList[i].GetWeighting();

        }
        if (i >= 8)
        {
            Debug.Log("8");
        }
	    return (dropsList[i].GetDrop());
    }
    
	public List<DropListEntry> GetCurrencyList()
	{
		return currencyList;
	}
	
	public List<DropListEntry> GetHealthList()
	{
		return healthList;
	}
	
	public List<DropListEntry> GetAmmoList()
	{
		return ammoList;
	}
    
	public void ModifyHealthDropQuantity(int valueToModify)
	{
		minHealthSpawn += valueToModify;
		maxHealthSpawn += valueToModify;
	}
	
	public int GetMinHealthSpawn()
	{
		return minHealthSpawn;
	}

	public int GetMaxHealthSpawn()
	{
		return maxHealthSpawn;
	}
	
	public void ModifyCurrencyDropQuantity(int valueToModify)
	{
		minCurrencySpawn += valueToModify;
		maxCurrencySpawn += valueToModify;
	}
	
	public void MultiplyCurrencyDropQuantity(int multiplier)
	{
		minCurrencySpawn *= multiplier;
		maxCurrencySpawn *= multiplier;
	}
	
	public int GetMinCurrencySpawn()
	{
		return minCurrencySpawn;
	}

	public int GetMaxCurrencySpawn()
	{
		return maxCurrencySpawn;
	}
	
	public void ModifyAmmoDropQuantity(int valueToModify)
	{
		minAmmoSpawn += valueToModify;
		maxAmmoSpawn += valueToModify;
	}
	
	public void MultiplyAmmoDropQuantity(float multiplier)
	{
		minAmmoSpawn = Mathf.RoundToInt((float)minAmmoSpawn * multiplier);
		maxAmmoSpawn = Mathf.RoundToInt((float)maxAmmoSpawn * multiplier);
	}
	
	public int GetMinAmmoSpawn()
	{
		return minAmmoSpawn;
	}

	public int GetMaxAmmoSpawn()
	{
		return maxAmmoSpawn;
	}
	

}
