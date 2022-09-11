using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmeraldEye : Item
{
    [SerializeField]
    DropsList dropList;

    [SerializeField]
    int increaseDropAmount;

    public override void AddEffect(PlayerClass player)
    {
        base.AddEffect(player);
        dropList.maxHealthSpawn += increaseDropAmount;
        dropList.minHealthSpawn += increaseDropAmount;

    }

    public override void RemoveEffect()
    {
        base.RemoveEffect();
        dropList.maxHealthSpawn -= increaseDropAmount;
        dropList.minHealthSpawn -= increaseDropAmount;
    }
}
