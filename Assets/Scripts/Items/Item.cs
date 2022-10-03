using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class Item : MonoBehaviour
{
	[SerializeField]
	bool add = false;
    GameObject UIWidget;
    public float currencyCost;
    public Sprite[] sprites;
    public string itemName = "";
    public string description = "";
	protected ElementStats elementData;
    




	// Update is called every frame, if the MonoBehaviour is enabled.
	protected void Update()
	{
		if(add)
		{
			AddEffect(GameObject.Find("Player").GetComponent<PlayerClass>());
			add = false;
		}
			
	}
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



    //Called whenever the player hits an enemy (specifically the take damage function in enemy)
    public virtual void OnHitTriggers(BaseEnemyClass enemyHit, List<BaseEnemyClass.Types> types)
    {

    }

    public virtual void SpawnTrigger(GameObject enemySpawning)
    {

    }
}
