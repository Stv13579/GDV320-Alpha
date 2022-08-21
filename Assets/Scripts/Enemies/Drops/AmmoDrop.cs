using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDrop : BaseDropScript  //Sebastian
{
    [SerializeField]
    float ammoRestore = 10;
    [SerializeField]
    protected List<PlayerClass.ManaName> manaTypes;
    protected override void PickupEffect()
    {
        base.PickupEffect();
        player.gameObject.GetComponent<PlayerClass>().ChangeMana(ammoRestore, manaTypes);
    }

    public void SetAmmoRestore(float ammo)
    {
        ammoRestore = ammo;
    }



}
