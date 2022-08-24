using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancedCompass : Trinket
{
    int activationChance;

    [SerializeField]
    int tier1Chance, tier2Chance, tier3Chance;

    public int GetActivationChance() { return activationChance; }
    public override void AddEffect(PlayerClass player)
    {
        base.AddEffect(player);

        switch((int)uState)
        {
            case 0:
                activationChance = 0;
                break;
            case 1:
                activationChance = tier1Chance;
                break;
            case 2:
                activationChance = tier2Chance;
                break;
            case 3:
                activationChance = tier3Chance;
                break;
        }

        
    }
}
