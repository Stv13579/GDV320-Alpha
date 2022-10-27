using System.Collections;
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
	    player = Shooting.GetShooting();
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
        if (player.GetCatalystElements()[cata.GetElementSlot()].GetCurrentElementType() == BaseElementClass.ElementType.None)
        {
            comboIcon.enabled = false;

            return;
        }

        if (player.GetComboElements()[prime.GetElementSlot()].comboElements[cata.GetElementSlot()].GetCurrentElementType() != BaseElementClass.ElementType.None)
        {
            comboIcon.enabled = true;

            lVars = player.GetComboElements()[prime.GetElementSlot()].comboElements[cata.GetElementSlot()].GetLVars();

            comboIcon.sprite = lVars.Icon;
        }
    }

}
