﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ComboSlot : HoverOver, IPointerEnterHandler
{
    [SerializeField]
    ElementSlots prime, cata;

    Shooting player;

    [SerializeField]
    Image comboIcon;


    private void Start()
    {
        player = FindObjectOfType<Shooting>();
    }

    public override void OnPointerEnter(PointerEventData pData)
    {
        base.OnPointerEnter(pData);


    }

    private void Update()
    {
        if(player.GetPrimaryElements()[prime.GetElementSlot()].GetCurrentElementType() == BaseElementClass.ElementType.None)
        {
            comboIcon.enabled = false;

            return;
        }
        if (player.GetPrimaryElements()[prime.GetElementSlot()].GetCurrentElementType() == BaseElementClass.ElementType.None)
        {
            comboIcon.enabled = false;

            return;
        }

        if (player.GetComboElements()[prime.GetElementSlot()].comboElements[cata.GetElementSlot()].GetCurrentElementType() != BaseElementClass.ElementType.None)
        {
            comboIcon.enabled = true;

            lVars = player.GetComboElements()[prime.GetElementSlot()].comboElements[cata.GetElementSlot()].lVars;

            comboIcon.sprite = lVars.Icon;
        }
    }

}
