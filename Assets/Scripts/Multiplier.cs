using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplier
{
    public float multiplier;
    public string source;
    public Multiplier(float multi, string s)
    {
        multiplier = multi;
        source = s;
    }
    static public void AddMultiplier(List<Multiplier> multisToCheck, Multiplier multiplierToAdd, float multiToChange)
    {
        if (!multisToCheck.Contains(multiplierToAdd))
        {
            multisToCheck.Add(multiplierToAdd);
        }
        multiToChange = 1.0f;
        foreach (Multiplier multi in multisToCheck)
        {
            multiToChange *= multi.multiplier;
        }
    }

    static public void RemoveMultiplier(List<Multiplier> multisToCheck, Multiplier multiplierToRemove, float multiToChange)
    {
        if (multisToCheck.Contains(multiplierToRemove))
        {
            multisToCheck.Remove(multiplierToRemove);
        }
        multiToChange = 1.0f;
        foreach (Multiplier multi in multisToCheck)
        {
            multiToChange *= multi.multiplier;
        }
    }


}
