using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseMaxManaScript : Item
{
    public override void AddEffect(PlayerClass player)
    {
        base.AddEffect(player);
        IncreaseMaxMana(player);
    }

    public void IncreaseMaxMana(PlayerClass player)
    {
        int index = 0;
        foreach (PlayerClass.ManaType mana in player.GetManaTypeArray())
        {
            StatModifier.AddModifier(player.GetManaTypeArray()[index].mana.additiveModifiers, new StatModifier.Modifier(100.0f, "Mana Ring" + GetInstanceID()));
            player.GetManaTypeArray()[index].maxMana = StatModifier.UpdateValue(player.GetManaTypeArray()[index].mana); 
            index++;
        }
    }
}
