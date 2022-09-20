using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axlotl : NPC
{
    //Only used for the tutorial!
    [SerializeField]
    SAIM tutorialSAIM;

    public override void TutorialAction()
    {
        base.TutorialAction();
        tutorialSAIM.ManualSpawn();
        //Dissappear

        gameObject.SetActive(false);
    }

}
