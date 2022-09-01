using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trinket : Item
{
    public enum UnlockState
    {
        Locked,
        One,
        Two,
        Three
    }

    public UnlockState uState = UnlockState.Locked;

    public override void AddEffect(PlayerClass player)
    {
        base.AddEffect(player);
    }


    public void Upgrade()
    {
        uState++;
    }

}
