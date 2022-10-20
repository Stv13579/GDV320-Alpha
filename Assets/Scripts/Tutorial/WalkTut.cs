using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkTut : TutorialPointers
{
    public override void CheckConditions(bool done = false)
    {
        bool result = false;

        if (Input.GetKeyUp(KeyCode.W) ||
            Input.GetKeyUp(KeyCode.A) ||
            Input.GetKeyUp(KeyCode.S) ||
            Input.GetKeyUp(KeyCode.D))
        {
            result = true;
        }
        base.CheckConditions(result);
    }
}
