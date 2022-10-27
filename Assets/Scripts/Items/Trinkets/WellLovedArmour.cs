using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellLovedArmour : Trinket
{
    public override void AddEffect(PlayerClass player)
    {
        base.AddEffect(player);


        StatModifier.Modifier reduceDamageMulti = new StatModifier.Modifier(1 - ((int)uState * 5 + 5)/100, "WellLovedArmour");

        StatModifier.AddModifier(player.GetDefenseStat().multiplicativeModifiers, reduceDamageMulti);
        StatModifier.UpdateValue(player.GetDefenseStat());
    }
    
	public override void RemoveEffect()
	{
		base.RemoveEffect();
		StatModifier.RemoveModifier(PlayerClass.GetPlayerClass().GetDefenseStat().multiplicativeModifiers, new StatModifier.Modifier(1 - ((int)uState * 5 + 5)/100, "WellLovedArmour"));
		StatModifier.UpdateValue(PlayerClass.GetPlayerClass().GetDefenseStat());
		
	}
}
