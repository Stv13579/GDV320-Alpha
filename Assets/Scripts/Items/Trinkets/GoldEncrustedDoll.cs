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
	    DropsList[] instances = Resources.FindObjectsOfTypeAll<DropsList>();
	    dropList = instances[0];
        dropList.maxAmmoSpawn += (int)uState;
        dropList.minAmmoSpawn += (int)uState;
        dropList.maxCurrencySpawn += (int)uState;
	    dropList.minCurrencySpawn += (int)uState;
	    dropList.SetHealthDrops((int)uState);

    }

    public override void RemoveEffect()
    {
        base.RemoveEffect();

        dropList.maxAmmoSpawn     -= (int)uState;
        dropList.minAmmoSpawn     -= (int)uState;
        dropList.maxCurrencySpawn -= (int)uState;
        dropList.minCurrencySpawn -= (int)uState;
	    dropList.SetHealthDrops(-(int)uState);

    }
}
