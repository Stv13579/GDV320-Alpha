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

        Multiplier.AddMultiplier(pM.resistMultipliers, newResistMulti, pM.prophecyResistMulti);
        Multiplier.AddMultiplier(pM.weaknessMultipliers, newWeakMulti, pM.prophecyWeaknessMulti);
    }
}
