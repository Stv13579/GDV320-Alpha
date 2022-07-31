using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespiteRoom : Room
{

    /// <summary>
    /// The first one should always by the shop
    /// </summary>
    [SerializeField]
    List<GameObject> NPCs;

    [HideInInspector]
    public bool isShoppe = false;

    private void Start()
    {
        foreach (GameObject NPC in NPCs)
        {
            NPC.SetActive(false);
        }

        if(isShoppe)
        {
            NPCs[0].SetActive(true);
        }
        else
        {
            NPCs[Random.Range(1, NPCs.Count)].SetActive(true);
        }

    }
}
