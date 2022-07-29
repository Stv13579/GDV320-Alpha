using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axlotl : NPC
{

    class Greetings : Dialogue
    {
        public Greetings(NPCData npcData) :  base(npcData){}


        public override void Action()
        {

            //Play an animation
            base.Action();
        }
    }

    private void Start()
    {
        
    }
}
