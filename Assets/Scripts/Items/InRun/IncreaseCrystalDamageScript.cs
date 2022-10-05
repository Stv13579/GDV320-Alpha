using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseCrystalDamageScript : Item
{

    

    void Start()
    {
        itemName = "Crystal Ring";
    }

    public override void AddEffect(PlayerClass player)
    {
        base.AddEffect(player);

        Multiplier increaseCrystalMulti = new Multiplier(1.1f, "crystalItem");

        elementData.crystalDamageMultiplier = Multiplier.AddMultiplier(elementData.crystaldamageMultis, increaseCrystalMulti);
        
    }
}
