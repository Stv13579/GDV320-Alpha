using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wealth : Prophecy
{
    
    DropListEntry drop;
    Multiplier newWealthDamageMulti;

    public override void InitialEffect()
    {
        base.InitialEffect();
        ProphecyManager pM = GetComponentInParent<ProphecyManager>();
        pM.wealth = true;
        newWealthDamageMulti = new Multiplier(1.25f, "Wealth");
	    drops.MultiplyCurrencyDropQuantity(2);
        pM.prophecyDamageMulti = Multiplier.AddMultiplier(pM.damageMultipliers, newWealthDamageMulti);
        
    }
}

  
