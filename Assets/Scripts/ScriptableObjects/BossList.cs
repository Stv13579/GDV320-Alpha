using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public struct BossListEntry
{
	[SerializeField]
	GameObject boss;
	[SerializeField]
	int difficulty;
	[SerializeField]
    bool spawned;

    public void SetSpawned(bool set)
    {
        spawned = set;
    }
    
	public bool GetSpawned()
	{
		return spawned;
	}
	
	public GameObject GetBoss()
	{
		return boss;
	}
}
[CreateAssetMenu(fileName = "Boss List")]

public class BossList : ScriptableObject
{
    public List<BossListEntry> bossList;

    public GameObject GetBoss()
    {
        bool searching = true;
        while(searching)
        {
            int i = UnityEngine.Random.Range(0, bossList.Count);
	        if(!bossList[i].GetSpawned())
            {
                bossList[i].SetSpawned(true);
                searching = false;
		        return (bossList[i].GetBoss());
            }
        }
        return null; //Shouldn't be needed but it complained witout it
    }

    public void ResetList()
    {
        foreach (BossListEntry boss in bossList)
        {
            boss.SetSpawned(false);
        }
    }

}
