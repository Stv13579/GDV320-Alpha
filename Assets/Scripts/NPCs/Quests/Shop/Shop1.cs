using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop1 : Quest
{
    [SerializeField]
    int currencyCompletion;

    public override void DeathBehaviour()
    {
        base.DeathBehaviour();

        if(GameObject.Find("Player").GetComponent<PlayerClass>().GetMoney() >= currencyCompletion)
        {
            FinishQuest();
        }
    }

    public override void FinishRunBehaviour()
    {
        base.FinishRunBehaviour();

        if (GameObject.Find("Player").GetComponent<PlayerClass>().GetMoney() >= currencyCompletion)
        {
            FinishQuest();
        }
    }
}
