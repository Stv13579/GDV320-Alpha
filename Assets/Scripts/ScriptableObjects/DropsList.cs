using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drops List")]
//Scriptable Object to store all items enemies can drop for access elsewhere

public class DropsList : ScriptableObject
{
    public List<DropListEntry> currencyList;
    public List<DropListEntry> ammoList;
    public List<DropListEntry> healthList;

    public int minCurrencySpawn = 1;
    public int maxCurrencySpawn = 2;

    public int minAmmoSpawn = 2;
    public int maxAmmoSpawn = 4;

    public int minHealthSpawn = 1;
    public int maxHealthSpawn = 1;

    public GameObject GetDrop(List<DropListEntry> dropsList)
    {
        int totalWeight = 0;
        foreach(DropListEntry drop in dropsList)
        {
            totalWeight += drop.weighting;
        }
        int rand = Random.Range(1, totalWeight);
        int i = -1;
        while(rand > 0)
        {
            i++;
            rand -= dropsList[i].weighting;
            
        }
        if(i >= 8)
        {
            Debug.Log("8");
        }
        return (dropsList[i].drop);
    }

    public GameObject GetAmmoDrop()
    {
        //Get the cata and prime elements and add only the elements of that type to the list to choose from

        int index = 0;
        List<DropListEntry> dropsList = new List<DropListEntry>();

        foreach (DropListEntry manaType in ammoList)
        {
            if(manaType.element == FindObjectOfType<Shooting>().GetCatalystElements()[0].GetManaName() 
                || manaType.element == FindObjectOfType<Shooting>().GetPrimaryElements()[0].GetManaName()
                || manaType.element == FindObjectOfType<Shooting>().GetCatalystElements()[1].GetManaName()
                || manaType.element == FindObjectOfType<Shooting>().GetPrimaryElements()[1].GetManaName())
            {
                dropsList.Add(manaType);
            }
            index++;
        }

        int totalWeight = 0;
        foreach (DropListEntry drop in dropsList)
        {
            totalWeight += drop.weighting;
        }
        int rand = Random.Range(1, totalWeight);
        int i = -1;
        while (rand > 0)
        {
            i++;
            rand -= dropsList[i].weighting;

        }
        if (i >= 8)
        {
            Debug.Log("8");
        }
        return (dropsList[i].drop);
    }


}
