using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : NPCUI
{
	List<Item> shopItems = new List<Item>();
	[SerializeField]
	List<GameObject> buttons;
	[SerializeField]
    ItemList items;

    List<int> ids = new List<int>();
	
	[SerializeField]
    TextMeshProUGUI moneyText;


	public override void Start()
    {
        base.Start();


        //For vertical slice purposes, remove for full game
        foreach (ItemEntry item in items.GetItemList())
        {
            item.alreadyAdded = false;
        }

        int itemsAdded = 0;
        int exitCounter = 0;
        while(itemsAdded < 4)
        {
            //Get a random item from the global item list, check if the item is valid to give to the player, and if so add it, otherwise try again
            int i = Random.Range(0, items.GetItemList().Count);
            Item item = (Item)this.gameObject.AddComponent(System.Type.GetType(items.GetItemList()[i].item));
            if(!items.GetItemList()[i].alreadyAdded || (items.GetItemList()[i].alreadyAdded && items.GetItemList()[i].mulipleAllowed))
            {
                item.sprites = items.GetItemList()[i].sprites;
                item.itemName = items.GetItemList()[i].itemName;
                item.currencyCost = items.GetItemList()[i].price;
                item.description = items.GetItemList()[i].description;
                shopItems.Add(item);
                ids.Add(i);
                items.GetItemList()[i].alreadyAdded = true;
                itemsAdded += 1;
            }
            else
            {
                Destroy(item);
            }
            exitCounter++;
            if(exitCounter > 50)
            {
                //If the loop goes on too long, place default items

                //to do, have default items get added (npc items most likely)
                Debug.Log("Exited");
                itemsAdded = 3;
            }
        }
        for(int i = 0; i < 4; i++)
        {
            //Give the UI buttons the necessary information for each item they contain
            buttons[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = shopItems[i].itemName;
            buttons[i].transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = shopItems[i].sprites[0];
            buttons[i].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().sprite = shopItems[i].sprites[1];
            buttons[i].transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = shopItems[i].description;
            buttons[i].transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = shopItems[i].currencyCost.ToString();
        }
    }

    private void Update()
    {
        moneyText.text = "Money: " + player.GetMoney().ToString();
    }
    public void Button(int button)
    {
        if(player.GetMoney() >= shopItems[button].currencyCost)
        {
            //If he player has enough money, give the player the item, take away their money, and remove he option from the shop
            Item item = (Item)inventory.AddComponent(shopItems[button].GetType());
            item.sprites = shopItems[button].sprites;
            Destroy(shopItems[button]);
            player.AddItem(item);
            buttons[button].SetActive(false);
            player.ChangeMoney(-shopItems[button].currencyCost);

            if (audioManager)
            {
                audioManager.StopSFX("Shop Buy");
                audioManager.PlaySFX("Shop Buy");
            }
        }
    }

    public override void Close()
    {
        for (int i = 0; i < 3; i++)
        {
            if (buttons[i].activeInHierarchy)
            {
                //If an item isn't bought when leaving the shop, mark it available to be obtained again
                items.GetItemList()[ids[i]].alreadyAdded = false;
            }
        }
        base.Close();
    }

}
