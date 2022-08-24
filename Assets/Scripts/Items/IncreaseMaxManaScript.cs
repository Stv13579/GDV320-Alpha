﻿using System.Collections;
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
        foreach (PlayerClass.ManaType mana in player.manaTypes)
        {
            StatModifier.AddModifier(player.manaTypes[index].mana.additiveModifiers, new StatModifier.Modifier(25.0f, "Mana Ring" + GetInstanceID()));
            index++;
        }
    }
}
