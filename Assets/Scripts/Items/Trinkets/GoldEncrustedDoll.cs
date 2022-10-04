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
	    dropList.ModifyAmmoDropQuantity((int)uState);
	    dropList.ModifyCurrencyDropQuantity((int)uState);
	    dropList.ModifyHealthDropQuantity((int)uState);

    }

    public override void RemoveEffect()
    {
        base.RemoveEffect();

	    dropList.ModifyAmmoDropQuantity((int)uState);
	    dropList.ModifyCurrencyDropQuantity((int)uState);
	    dropList.ModifyHealthDropQuantity(-(int)uState);

    }
}
