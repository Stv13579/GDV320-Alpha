using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyKnuckleBones : Trinket
{
    int reviveTimes = 0;
    
    

    public bool CanRevive()
    {
        if(reviveTimes < (int)uState)
        {
            reviveTimes++;
            return true;
        }

        return false;
        
    }
}
