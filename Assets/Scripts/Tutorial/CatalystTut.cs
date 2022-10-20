using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatalystTut : TutorialPointers
{
    public override void CheckConditions(bool done = false)
    {
        bool result = false;

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            result = true;
        }


        base.CheckConditions(result);
    }
}
