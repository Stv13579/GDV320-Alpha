using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplier
{
    public float multiplier;
    public string source;
    public Multiplier(float multi = 1.0f, string s = "")
    {
        multiplier = multi;
        source = s;
    }
    static public float AddMultiplier(List<Multiplier> multisToCheck, Multiplier multiplierToAdd)
    {
        if (!multisToCheck.Contains(multiplierToAdd))
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
        if (multisToCheck.Contains(multiplierToRemove))
        {
            multisToCheck.Remove(multiplierToRemove);
        }
        float multiToChange = 1.0f;
        foreach (Multiplier multi in multisToCheck)
        {
            multiToChange *= multi.multiplier;
        }
        return multiToChange;
    }


}
