using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseWaterDamageScript : Item
{
    void Start()
    {
        itemName = "Flowing Band";
    }

    public override void AddEffect(PlayerClass player)
    {
        base.AddEffect(player);

        Multiplier increaseWaterMulti = new Multiplier(1.1f, "waterItem");

        elementData.waterDamageMultiplier = Multiplier.AddMultiplier(elementData.waterDamageMultis, increaseWaterMulti);

    }
}
