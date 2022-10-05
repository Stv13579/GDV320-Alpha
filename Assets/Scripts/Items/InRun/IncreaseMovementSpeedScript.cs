using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseMovementSpeedScript : Item
{
    void Start()
    {
        itemName = "Slippery Socks";
    }

    public override void AddEffect(PlayerClass player)
    {
        base.AddEffect(player);

        PlayerMovement pMove = player.GetComponent<PlayerMovement>();

        //Multiplier movementMulti = new Multiplier(1.1f, "moveItem");

        //pMove.movementMulti = Multiplier.AddMultiplier(pMove.movementMultipliers, movementMulti);

        StatModifier.AddModifier(pMove.GetSpeedStat().multiplicativeModifiers, new StatModifier.Modifier(1.1f, "moveItem" + GetInstanceID()));

    }
}
