﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameplayUI : MonoBehaviour
{
    PlayerClass playerClass;
    Shooting player;

    //Serialize all UI elements
    [SerializeField]
    Image healthBar, activePrimaryElement, inactivePrimaryElement, activeCatalystElement, inactiveCatalystElement, activeComboElement, inactiveComboElement, crosshair;

    [SerializeField]
    GameObject inactiveComboBorder, activeComboBorder;

    [SerializeField]
    List<Image> items;
    int itemIndex = 0;

    [SerializeField]
    TextMeshProUGUI moneyText, leftManaText, rightManaText;

    [SerializeField]
    Image leftActiveFill, leftInactiveFill, rightActiveFill, rightInactiveFill;

    bool combo = false;


    [SerializeField]
    float maxComboTimer = 1.0f;

    float comboTimer = 1.0f;

    public bool GetCombo() { return combo; }
    public void SetCombo(bool tempCombo) { combo = tempCombo; }

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Shooting>();
        playerClass = player.gameObject.GetComponent<PlayerClass>();
        comboTimer = maxComboTimer;
        Debug.Log("G UI on");
    }

    void Update()
    {
        //Getting the current values from the player and updating the UI with them
        healthBar.fillAmount = playerClass.GetCurrentHealth() / playerClass.GetMaxHealth();
        moneyText.text = playerClass.money.ToString();

        activePrimaryElement.sprite = player.GetPrimaryElementSprite();
        inactivePrimaryElement.sprite = player.GetNextPrimaryElementSprite();
        
        activeCatalystElement.sprite = player.GetCatalystElementSprite();
        inactiveCatalystElement.sprite = player.GetNextCatalystElementSprite();

        activeComboElement.sprite = player.GetComboElementSprite();
        inactiveComboElement.sprite = player.GetComboElementSprite();
        crosshair.sprite = player.GetCrosshair();

        Vector2 leftMana = player.GetLeftMana();
        leftActiveFill.fillAmount = leftMana.x / leftMana.y;

        //leftManaText.text = leftMana[0] + "/" + leftMana[1];

        Vector2 rightMana = player.GetRightMana();
        rightActiveFill.fillAmount = rightMana.x / rightMana.y;

        //rightManaText.text = rightMana[0] + "/" + rightMana[1];

        if (combo)
        {
            comboTimer -= Time.deltaTime;
            activeComboBorder.SetActive(true);
            inactiveComboBorder.SetActive(false);
        }
        else
        {
            comboTimer += Time.deltaTime;
            inactiveComboBorder.SetActive(true);
            activeComboBorder.SetActive(false);
        }
        comboTimer = Mathf.Clamp(comboTimer, 0, maxComboTimer);

        ChangeCombo(activePrimaryElement.transform.parent, true);
        ChangeCombo(inactivePrimaryElement.transform.parent, true);
        ChangeCombo(activeCatalystElement.transform.parent, true);
        ChangeCombo(inactiveCatalystElement.transform.parent, true);
        ChangeCombo(activeComboElement.transform.parent, false);
        ChangeCombo(inactiveComboElement.transform.parent, false);
    }

    public void AddItem(Sprite[] sprites)
    {
        if(itemIndex < items.Count)
        {
            items[itemIndex].sprite = sprites[0];
            items[itemIndex].transform.GetChild(0).GetComponent<Image>().sprite = sprites[1];
            items[itemIndex].enabled = true;
            items[itemIndex].transform.GetChild(0).GetComponent<Image>().enabled = true;
            itemIndex++;
        }
    }

    void ChangeCombo(Transform uiObject, bool doCombo)
    {
        if(doCombo)
        {
            Color colour = uiObject.GetComponent<Image>().color;
            float alpha = Mathf.Lerp(0.15f, 1.0f, comboTimer / maxComboTimer);
            uiObject.GetComponent<Image>().color = new Color(colour.r, colour.g, colour.b, alpha);

        }
        else
        {
            //Color colour = uiObject.GetComponent<Image>().color;
            //float alpha = Mathf.Lerp(1.0f, 0.15f, comboTimer / maxComboTimer);
            //uiObject.GetComponent<Image>().color = new Color(colour.r, colour.g, colour.b, alpha);
        }
        for (int i = 0; i < uiObject.childCount; i++)
        {
            if(uiObject.GetChild(i).name == "ActiveFill" || uiObject.GetChild(i).name == "InactiveFill")
            {
                continue;
            }
            if (doCombo)
            {
                float alpha = Mathf.Lerp(0.15f, 1.0f, comboTimer / maxComboTimer);
                if (uiObject.GetChild(i).GetComponent<Image>())
                {
                    Color colour = uiObject.GetChild(i).GetComponent<Image>().color;
                    uiObject.GetChild(i).GetComponent<Image>().color = new Color(colour.r, colour.g, colour.b, alpha);
                }
                else if (uiObject.GetChild(i).GetComponent<TextMeshProUGUI>())
                {
                    Color colour = uiObject.GetChild(i).GetComponent<TextMeshProUGUI>().color;
                    uiObject.GetChild(i).GetComponent<TextMeshProUGUI>().color = new Color(colour.r, colour.g, colour.b, alpha);
                }



            }
            else
            {
                float alpha = Mathf.Lerp(1.0f, 0.15f, comboTimer / maxComboTimer);

                if (uiObject.GetChild(i).GetComponent<Image>())
                {
                    Color colour = uiObject.GetChild(i).GetComponent<Image>().color;
                    uiObject.GetChild(i).GetComponent<Image>().color = new Color(colour.r, colour.g, colour.b, alpha);
                }
                else if (uiObject.GetChild(i).GetComponent<TextMeshProUGUI>())
                {
                    Color colour = uiObject.GetChild(i).GetComponent<TextMeshProUGUI>().color;
                    uiObject.GetChild(i).GetComponent<TextMeshProUGUI>().color = new Color(colour.r, colour.g, colour.b, alpha);
                }
            }
        }

    }
}
