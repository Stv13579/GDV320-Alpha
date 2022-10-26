using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCUI : MonoBehaviour
{
	protected Interact NPC;

    protected PlayerClass player;
    protected GameObject inventory;
    protected AudioManager audioManager;

	public virtual void Start()
    {
	    audioManager = AudioManager.GetAudioManager();
	    player = PlayerClass.GetPlayerClass();
	    inventory = player.gameObject.transform.GetChild(0).gameObject;
    }

    public virtual void Close()
    {

        NPC.LeaveUI();
    }
    
	public Interact GetNPC()
	{
		return NPC;
	}
	
	public void SetNPC(Interact newNPC)
	{
		NPC = newNPC;
	}

}
