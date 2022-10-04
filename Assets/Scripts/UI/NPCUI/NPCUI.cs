﻿using System.Collections;
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
        audioManager = FindObjectOfType<AudioManager>();
        player = GameObject.Find("Player").GetComponent<PlayerClass>();
        inventory = GameObject.Find("Player").transform.GetChild(0).gameObject;
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
