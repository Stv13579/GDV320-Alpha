using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseMovementSpeedScript : Item
{
    private void Start()
    {
        itemName = "Slippery Socks";
    }

    public override void AddEffect(PlayerClass player)
    {
        base.AddEffect(player);

        PlayerMovement pMove = player.GetComponent<PlayerMovement>();

        Multiplier movementMulti = new Multiplier(1.1f, "moveItem");

        Multiplier.AddMultiplier(pMove.movementMultipliers, movementMulti, pMove.movementMulti);

    }
}
