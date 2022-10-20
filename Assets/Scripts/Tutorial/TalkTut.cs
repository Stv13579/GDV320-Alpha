using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTut : TutorialPointers
{
    public override void CheckConditions(bool done = false)
    {
        bool result = false;

        if (Input.GetKeyUp(KeyCode.T))
        {
            result = true;
        }


        base.CheckConditions(result);
    }
}
