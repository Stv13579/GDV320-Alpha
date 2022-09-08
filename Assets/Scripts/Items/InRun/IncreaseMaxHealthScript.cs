using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseMaxHealthScript : Item
{
    public override void AddEffect(PlayerClass player)
    {
        base.AddEffect(player);
        IncreaseMaxHealth(player);
    }
    public void IncreaseMaxHealth(PlayerClass player)
    {
        StatModifier.AddModifier(player.GetHealthStat().additiveModifiers, new StatModifier.Modifier(100.0f, "Health Ring" + GetInstanceID()));
        player.ChangeHealth(100.0f);
    }
}
