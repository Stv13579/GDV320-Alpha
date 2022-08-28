using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fortune1 : Quest
{
    [SerializeField]
    Prophecy q1Proph;

    public void ActivateProphecy()
    {
        q1Proph.SetActive();
    }
}
