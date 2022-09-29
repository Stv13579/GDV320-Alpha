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

    GameObject hitMarker;

    Image lifeStealFullScreen;
    Image voidFullScreen;
    Image burnFullScreen;
    Image hurtFullScreen;
    Image lowHealthFullScreen;
    Image inToxicFullScreen;
    [SerializeField]
    Image damageIndicator;
    public Image GetLifeStealFullScreen() { return lifeStealFullScreen; }
    public Image GetVoidFullScreen() { return voidFullScreen; }
    public Image GetBurnFullScreen() { return burnFullScreen; }
    public Image GetHurtFullScreen() { return hurtFullScreen; }
    public Image GetLowHealthFullScreen() { return lowHealthFullScreen; }
    public Image GetInToxicFullScreen() { return inToxicFullScreen; }
    public Image GetDamageIndicator() { return damageIndicator; }
    public GameObject GetHitMarker() { return hitMarker; }
    public bool GetCombo() { return combo; }
    public void SetCombo(bool tempCombo) { combo = tempCombo; }

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Shooting>();
        playerClass = player.gameObject.GetComponent<PlayerClass>();
        hitMarker = GameObject.Find("GameplayUI/HitMarker");
        lifeStealFullScreen = GameObject.Find("GameplayUI/Effects/LifeSteal").GetComponent<Image>();
        voidFullScreen = GameObject.Find("GameplayUI/Effects/Void").GetComponent<Image>();
        burnFullScreen = GameObject.Find("GameplayUI/Effects/Burn").GetComponent<Image>();
        hurtFullScreen = GameObject.Find("GameplayUI/Effects/PlayerDamage").GetComponent<Image>();
        lowHealthFullScreen = GameObject.Find("GameplayUI/Effects/LowHealth").GetComponent<Image>();
        inToxicFullScreen = GameObject.Find("GameplayUI/Effects/InToxic").GetComponent<Image>();
        comboTimer = maxComboTimer;
        Debug.Log("G UI on");
        DontDestroyOnLoad(gameObject);

        if (hitMarker)
        {
            hitMarker.SetActive(false);
        }
        if (lifeStealFullScreen)
        {
            lifeStealFullScreen.gameObject.SetActive(false);
        }
        if (voidFullScreen)
        {
            voidFullScreen.gameObject.SetActive(false);
        }
        if (burnFullScreen)
        {
            burnFullScreen.gameObject.SetActive(false);
        }
        if (hurtFullScreen)
        {
            hurtFullScreen.gameObject.SetActive(false);
        }
        if (lowHealthFullScreen)
        {
            lowHealthFullScreen.gameObject.SetActive(false);
        }
        if (inToxicFullScreen)
        {
            inToxicFullScreen.gameObject.SetActive(false);
        }
        if (damageIndicator)
        {
            damageIndicator.gameObject.SetActive(false);
        }
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

        Vector2 leftNextMana = player.GetNextLeftMana();
        leftInactiveFill.fillAmount = leftNextMana.x / leftNextMana.y;

        //leftManaText.text = leftMana[0] + "/" + leftMana[1];

        Vector2 rightMana = player.GetRightMana();
        rightActiveFill.fillAmount = rightMana.x / rightMana.y;

        Vector2 rightNextMana = player.GetNextRightMana();
        rightInactiveFill.fillAmount = rightNextMana.x / rightNextMana.y;

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
        if (hitMarker)
        {
            if (hitMarker.active == true)
            {
                StartCoroutine(HitMarker());
            }
        }
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

    public void RemoveItem(Sprite[] sprites)
    {

        foreach (Image item in items)
        {
            if(item.sprite == sprites[0])
            {
                item.sprite = sprites[0];
                item.transform.GetChild(0).GetComponent<Image>().sprite = sprites[1];
                item.enabled = false;
                item.transform.GetChild(0).GetComponent<Image>().enabled = false;
                itemIndex--;
            }
            
            
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
    public IEnumerator HitMarker()
    {
        if (hitMarker)
        {
            hitMarker.SetActive(true);
        }
        yield return new WaitForSeconds(0.2f);
        if (hitMarker)
        {
            hitMarker.SetActive(false);
        }
    }

    public IEnumerator DamageIndicator()
    {
        if(damageIndicator)
        {
            Instantiate(damageIndicator);
            damageIndicator.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(0.5f);
        if(damageIndicator)
        {
            Destroy(damageIndicator);
        }
    }
}
