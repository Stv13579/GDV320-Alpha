using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Famine : Prophecy
{
    DropListEntry drop;
    Multiplier newFamineHealthMulti;

    public override void InitialEffect()
    {
        base.InitialEffect();
        

        ProphecyManager pM = GetComponentInParent<ProphecyManager>();
        pM.famine = true;
        newFamineHealthMulti = new Multiplier(0.5f, "Famine");

        drops.minAmmoSpawn /= 2;
        drops.maxAmmoSpawn /= 2;

        Multiplier.AddMultiplier(pM.healthMultipliers, newFamineHealthMulti, pM.prophecyHealthMulti);

    }
}
