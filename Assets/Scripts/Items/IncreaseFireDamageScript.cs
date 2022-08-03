using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseFireDamageScript : Item
{
    public override void AddEffect(PlayerClass player)
    {
        base.AddEffect(player);
        Multiplier increaseFireMulti = new Multiplier(1.1f, "fireItem");

        Multiplier.AddMultiplier(elementData.fireDamageMultis, increaseFireMulti, elementData.fireDamageMultiplier);

    }

}
