using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fortune3 : Quest
{
    [SerializeField]
    Prophecy q1Proph;

    public void ActivateProphecy()
    {
        q1Proph.InitialEffect();


    }

    public override void StartRunBehaviour()
    {
        base.StartRunBehaviour();

        ActivateProphecy();
    }

    public override void FinishRunBehaviour()
    {
        base.FinishRunBehaviour();

        if (q1Proph.GetActive())
        {
            FinishQuest();
        }
    }
}
