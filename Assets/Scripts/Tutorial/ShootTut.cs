using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTut : TutorialPointers
{
    public override void CheckConditions(bool done = false)
    {
        bool result = false;

        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            result = true;
        }


        base.CheckConditions(result);
    }
}
