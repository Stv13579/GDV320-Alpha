using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public struct BossListEntry
{
    public GameObject boss;
    public int difficulty;
    public bool spawned;

    public void SetSpawned(bool set)
    {
        spawned = set;
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
            if(!bossList[i].spawned)
            {
                bossList[i].SetSpawned(true);
                searching = false;
                return (bossList[i].boss);
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
