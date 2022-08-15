using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrath : Prophecy
{
    Multiplier newResistMulti;
    Multiplier newWeakMulti;


    public override void InitialEffect()
    {
        base.InitialEffect();

        ProphecyManager pM = GetComponentInParent<ProphecyManager>();

        newResistMulti = new Multiplier(1.25f, "Wrath");
        newWeakMulti = new Multiplier(1.25f, "Wrath");

        pM.prophecyResistMulti = Multiplier.AddMultiplier(pM.resistMultipliers, newResistMulti);
        pM.prophecyWeaknessMulti = Multiplier.AddMultiplier(pM.weaknessMultipliers, newWeakMulti);
    }
}
