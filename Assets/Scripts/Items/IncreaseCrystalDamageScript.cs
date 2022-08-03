using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseCrystalDamageScript : Item
{

    

    private void Start()
    {
        itemName = "Crystal Ring";
    }

    public override void AddEffect(PlayerClass player)
    {
        base.AddEffect(player);

        Multiplier increaseCrystalMulti = new Multiplier(0.1f, "crystalItem");

        Multiplier.AddMultiplier(elementData.crystaldamageMultis, increaseCrystalMulti, elementData.crystalDamageMultiplier);
        
    }
}
