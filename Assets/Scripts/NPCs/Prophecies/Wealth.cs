using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wealth : Prophecy
{
    [SerializeField]
    DropsList drops;
    DropListEntry drop;
    Multiplier newWealthDamageMulti;

    public override void InitialEffect()
    {
        base.InitialEffect();
        drops.dropsList[0].weighting *= 2;
        ProphecyManager pM = GetComponentInParent<ProphecyManager>();
        pM.wealth = true;
        newWealthDamageMulti.source = "Wealth";
        newWealthDamageMulti.multiplier = 1.25f;
        Multiplier.AddMultiplier(pM.damageMultipliers, newWealthDamageMulti, pM.prophecyDamageMulti);
        
    }
}

  
