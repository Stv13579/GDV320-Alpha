using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LotlUI : NPCUI
{
    List<Item> giftItems = new List<Item>();
    public List<GameObject> buttons;
    public ItemList items;

    List<int> ids = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        foreach (ItemEntry item in items.itemList)
        {
            item.alreadyAdded = false;
        }


        int itemsAdded = 0;
        int exitCounter = 0;
        while (itemsAdded < 3)
        {
            //Get a random item from the global item list, check if the item is valid to give to the player, and if so add it, otherwise try again
            int i = Random.Range(0, items.itemList.Count);
            Item item = (Item)this.gameObject.AddComponent(System.Type.GetType(items.itemList[i].item));
            if (!items.itemList[i].alreadyAdded || (items.itemList[i].alreadyAdded && items.itemList[i].mulipleAllowed))
            {
                item.sprites = items.itemList[i].sprites;
                item.itemName = items.itemList[i].itemName;
                item.description = items.itemList[i].description;
                giftItems.Add(item);
                ids.Add(i);
                items.itemList[i].alreadyAdded = true;
                itemsAdded += 1;
            }
            else
            {
                Destroy(item);
            }
            exitCounter++;
            if (exitCounter > 50)
            {
                //If the loop goes on too long, place default items

                //to do, have default items get added (npc items most likely)
                Debug.Log("Exited");
                itemsAdded = 3;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            //Give the UI buttons the necessary information for each item they contain
            buttons[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = giftItems[i].itemName;
            buttons[i].transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = giftItems[i].sprites[0];
            buttons[i].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().sprite = giftItems[i].sprites[1];
            buttons[i].transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = giftItems[i].description;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Button(int button)
    {
        //When the player chooses an item, add it and close the screen permanently
        Item item = (Item)inventory.AddComponent(giftItems[button].GetType());
        item.sprites = giftItems[button].sprites;
        Destroy(giftItems[button]);
        player.AddItem(item);
        buttons[button].SetActive(false);
        if (audioManager)
        {
            audioManager.StopSFX("Shop Buy");
            audioManager.PlaySFX("Shop Buy");
        }
        Close();
        NPC.npc.interactPositon++;
        NPC.canSeeOfferings = false;
    }

    public override void Close()
    {
        for (int i = 0; i < 3; i++)
        {
            if (buttons[i].activeInHierarchy)
            {
                //If an item isn't bought when leaving the shop, mark it available to be obtained again
                items.itemList[ids[i]].alreadyAdded = false;
            }
        }
        base.Close();
    }

}
