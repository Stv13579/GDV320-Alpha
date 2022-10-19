using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FidgetAnimationControl : MonoBehaviour
{
    public void SetRandomFidget()
    {
        GetComponentInParent<NPC>().PlayRandomFidget();
    }
}
