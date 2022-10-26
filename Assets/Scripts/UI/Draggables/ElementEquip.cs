using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementEquip : DraggedObject
{
    [SerializeField]
    string elementType;

    public string GetElementType() { return elementType; }

    Shooting player;

    [SerializeField]
    bool isPrimary;

    public bool GetPrimary() { return isPrimary; }

    public override void Equip(DragSlot slot)
    {
        base.Equip(slot);
	    player = Shooting.GetShooting();
        ElementSlots eSlot = (ElementSlots)slot;
        if (eSlot.IsPrimary())
        {
            player.GetPrimaryElements()[eSlot.GetElementSlot()] = (BaseElementClass)player.GetComponent(System.Type.GetType(elementType));
        }
        else
        {
            player.GetCatalystElements()[eSlot.GetElementSlot()] = (BaseElementClass)player.GetComponent(System.Type.GetType(elementType));
        }

        if(isPrimary)
        {
            if (player.GetPrimaryElements()[eSlot.GetElementSlot()] && player.GetCatalystElements()[0])
            {
                player.GetComboElements()[eSlot.GetElementSlot()].comboElements[0] = player.CalculateCombo(player.GetPrimaryElements()[eSlot.GetElementSlot()], player.GetCatalystElements()[0]);
            }
            if (player.GetPrimaryElements()[eSlot.GetElementSlot()] && player.GetCatalystElements()[1])
            {
                player.GetComboElements()[eSlot.GetElementSlot()].comboElements[1] = player.CalculateCombo(player.GetPrimaryElements()[eSlot.GetElementSlot()], player.GetCatalystElements()[1]);
            }
        }
        else
        {
            if (player.GetPrimaryElements()[0] && player.GetCatalystElements()[eSlot.GetElementSlot()])
            {
                player.GetComboElements()[0].comboElements[eSlot.GetElementSlot()] = player.CalculateCombo(player.GetPrimaryElements()[0], player.GetCatalystElements()[eSlot.GetElementSlot()]);
            }
            if (player.GetPrimaryElements()[1] && player.GetCatalystElements()[eSlot.GetElementSlot()])
            {
                player.GetComboElements()[1].comboElements[eSlot.GetElementSlot()] = player.CalculateCombo(player.GetPrimaryElements()[1], player.GetCatalystElements()[eSlot.GetElementSlot()]);
            }
        }
    }


}
