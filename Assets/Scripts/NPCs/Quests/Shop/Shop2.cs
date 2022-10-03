using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop2 : Quest
{
    [SerializeField]
    int currencyCompletion;

    public override void FinishRunBehaviour()
    {
        base.FinishRunBehaviour();

        if (GameObject.Find("Player").GetComponent<PlayerClass>().GetMoney() >= currencyCompletion)
        {
            FinishQuest();
        }
    }
}
