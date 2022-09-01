using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldEncrustedDoll : Trinket
{
    [SerializeField]
    DropsList dropList;
    public override void AddEffect(PlayerClass player)
    {
        base.AddEffect(player);
        dropList.maxAmmoSpawn += (int)uState;
        dropList.minAmmoSpawn += (int)uState;
        dropList.maxCurrencySpawn += (int)uState;
        dropList.minCurrencySpawn += (int)uState;
        dropList.maxHealthSpawn += (int)uState;
        dropList.minHealthSpawn += (int)uState;

    }

    public override void RemoveEffect()
    {
        base.RemoveEffect();

        dropList.maxAmmoSpawn     -= (int)uState;
        dropList.minAmmoSpawn     -= (int)uState;
        dropList.maxCurrencySpawn -= (int)uState;
        dropList.minCurrencySpawn -= (int)uState;
        dropList.maxHealthSpawn   -= (int)uState;
        dropList.minHealthSpawn   -= (int)uState;
    }
}
