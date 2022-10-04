using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmeraldEye : Item
{
    [SerializeField]
    DropsList dropList;

    [SerializeField]
	int increaseDropAmount = 1;

    public override void AddEffect(PlayerClass player)
    {
	    base.AddEffect(player);
	    DropsList[] instances = Resources.FindObjectsOfTypeAll<DropsList>();
	    dropList = instances[0];
	    dropList.ModifyHealthDropQuantity(increaseDropAmount);


    }

    public override void RemoveEffect()
    {
        base.RemoveEffect();
	    dropList.ModifyHealthDropQuantity(-increaseDropAmount);

    }
}
