using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyDrop : BaseDropScript //Sebastian
{

    static float currencyAmount = 1;
    protected override void PickupEffect()
    {
        base.PickupEffect();
        player.gameObject.GetComponent<PlayerClass>().ChangeMoney(currencyAmount);
    }

    public void SetCurrency(float currency)
    {
        currencyAmount = currency;
    }




}
