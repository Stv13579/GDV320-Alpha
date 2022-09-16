using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axlotl : NPC
{
    //Only used for the tutorial!
    [SerializeField]
    SAIM tutorialSAIM;

    class Greetings : Dialogue
    {
        public Greetings(NPCData npcData) :  base(npcData){}


        public override void Action()
        {

            //Play an animation
            base.Action();
        }
    }

    public override void TutorialAction()
    {
        base.TutorialAction();
        tutorialSAIM.ManualSpawn();
        //Dissappear
        GameObject.Destroy(gameObject);
    }

}
