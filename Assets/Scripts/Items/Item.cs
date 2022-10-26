﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class Item : MonoBehaviour
{
    GameObject UIWidget;
    public float currencyCost;
    public Sprite[] sprites;
    public string itemName = "";
    public string description = "";
	protected ElementStats elementData;
    



    //Any effects from obtaining an item go here e.g. if the item increases max health, add it here.
    public virtual void AddEffect(PlayerClass player)
    {
	    GameplayUI.GetGameplayUI().AddItem(sprites);
        player.GetHeldItems().Add(this);
        elementData = player.GetComponent<ElementStats>();

    }

    public virtual void RemoveEffect()
    {
	    GameplayUI.GetGameplayUI().RemoveItem(sprites);
    }

    //Called by certain actions which might trigger an item effect e.g. a particular attack.
    public virtual void TriggeredEffect()
    {

    }

    public virtual void DeathTriggers()
    {

    }



    //Called whenever the player hits an enemy (specifically the take damage function in enemy)
    public virtual void OnHitTriggers(BaseEnemyClass enemyHit, List<BaseEnemyClass.Types> types)
    {

    }

    public virtual void SpawnTrigger(GameObject enemySpawning)
    {

    }
}
