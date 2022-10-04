using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BlacksmithUI : NPCUI
{

    List<BaseElementClass> elements = new List<BaseElementClass>();

    List<GameObject> buttons = new List<GameObject>();

    
    
    void Start()
    {
        base.Start();

        //find the player's current elements in the arrays in shooting.
        elements.AddRange(player.GetComponent<Shooting>().GetPrimaryElements());
        elements.AddRange(player.GetComponent<Shooting>().GetCatalystElements());
        elements.AddRange(player.GetComponent<Shooting>().GetComboElements()[0].comboElements);
        elements.AddRange(player.GetComponent<Shooting>().GetComboElements()[1].comboElements);

        //Apply the buttons functionality to each element, allowing them to upgrade.
        int i = 0;
        foreach (Transform button in GameObject.Find("UpgradeButtons").transform)
        {
            buttons.Add(button.gameObject);
	        buttons[i].transform.GetChild(1).GetComponent<Image>().sprite = elements[i].GetSprite();
            buttons[i].GetComponent<UpgradeButton>().SetElement(elements[i]);       
            i++;
        }
    }

    private void Update()
    {
        
    }

    public void UpgradeButton(int index)
    {
	    if (player.GetMoney() >= elements[index].GetUpgradeCost())
        {
            //Call an upgrade function for that element
            elements[index].Upgrade();
            buttons[index].SetActive(false);
            player.ChangeMoney(-elements[index].GetUpgradeCost());
            if (audioManager)
            {
                audioManager.StopSFX("Shop Buy");
                audioManager.PlaySFX("Shop Buy");
            }
        }
    }

}
