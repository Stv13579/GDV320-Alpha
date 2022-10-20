using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboTut : TutorialPointers
{
    public override void CheckConditions(bool done = false)
    {
        bool result = false;

        if (Input.GetKeyUp(KeyCode.F))
        {
            result = true;
        }


        base.CheckConditions(result);
    }
}
