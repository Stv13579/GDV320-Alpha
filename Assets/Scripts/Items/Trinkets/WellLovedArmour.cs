using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellLovedArmour : Trinket
{
    public override void AddEffect(PlayerClass player)
    {
        base.AddEffect(player);

        StatModifier.Modifier reduceDamageMulti = new StatModifier.Modifier((int)uState * 5 + 5, "WellLovedArmour");

        GameObject.Find("Player").GetComponent<PlayerClass>().GetDefenseStat().multiplicativeModifiers.Add(reduceDamageMulti);
    }
}
