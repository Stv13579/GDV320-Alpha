using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCUI : MonoBehaviour
{
    [HideInInspector]
    public Interact NPC;

    protected PlayerClass player;
    protected GameObject inventory;
    protected AudioManager audioManager;

    public void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        player = GameObject.Find("Player").GetComponent<PlayerClass>();
        inventory = GameObject.Find("Player").transform.GetChild(2).gameObject;
    }

    public virtual void Close()
    {

        NPC.LeaveUI();
    }

}
