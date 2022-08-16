using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDrop : BaseDropScript
{
    protected override void PickupEffect()
    {
        base.PickupEffect();
        player.gameObject.GetComponent<PlayerClass>().ChangeMana(10, manaTypes);
    }

}
