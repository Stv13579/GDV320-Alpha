using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Multiplier
{
    float multiplier;
    string source;
    public Multiplier(float multi = 1.0f, string s = "")
    {
        multiplier = multi;
        source = s;
    }
    static public float AddMultiplier(List<Multiplier> multisToCheck, Multiplier multiplierToAdd)
    {
        bool contains = false;
        foreach(Multiplier multi in multisToCheck)
        {
            if(multi.source == multiplierToAdd.source)
            {
                contains = true;
            }
        }
        if (!contains)
        {
            multisToCheck.Add(multiplierToAdd);
        }
        float multiToChange = 1.0f;
        foreach (Multiplier multi in multisToCheck)
        {
            multiToChange *= multi.multiplier;
        }
        return multiToChange;
    }

    static public float RemoveMultiplier(List<Multiplier> multisToCheck, Multiplier multiplierToRemove)
    {
        bool contains = false;
        foreach (Multiplier multi in multisToCheck)
        {
            if (multi.source == multiplierToRemove.source)
            {
                contains = true;
            }
        }
        if(contains)
        {
            
            multisToCheck.Remove(multisToCheck.Find(x => x.source == multiplierToRemove.source));
        }
        float multiToChange = 1.0f;
        foreach (Multiplier multi in multisToCheck)
        {
            multiToChange *= multi.multiplier;
        }
        return multiToChange;
    }


}
