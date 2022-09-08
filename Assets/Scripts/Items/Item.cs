using System.Collections;
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
        player.itemUI.transform.parent.gameObject.GetComponent<GameplayUI>().AddItem(sprites);
        player.heldItems.Add(this);
        elementData = player.GetComponent<ElementStats>();

    }

    public virtual void RemoveEffect()
    {
        FindObjectOfType<PlayerClass>().itemUI.transform.parent.gameObject.GetComponent<GameplayUI>().RemoveItem(sprites);
    }

    //Called by certain actions which might trigger an item effect e.g. a particular attack.
    public virtual void TriggeredEffect()
    {

    }

    public virtual void DeathTriggers()
    {

    }

    public virtual void SpawnTrigger(GameObject enemySpawning)
    {

    }
}
