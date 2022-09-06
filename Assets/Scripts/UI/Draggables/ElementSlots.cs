using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ElementSlots : DragSlot
{
    [SerializeField]
    int elementSlot;

    public int GetElementSlot() { return elementSlot; }

    [SerializeField]
    bool isPrimary;

    Shooting player;

    private void Start()
    {
        player = FindObjectOfType<Shooting>();
    }

    public bool IsPrimary() { return isPrimary; }

    public override void OnPointerEnter(PointerEventData pData)
    {
        if(FindObjectOfType(System.Type.GetType(type)))
        {
            ElementEquip eQuip = (ElementEquip)FindObjectOfType(System.Type.GetType(type));

            if (!isPrimary )
            {
                if (eQuip.GetPrimary() || player.GetCatalystElements().Exists(ele => ele.name == eQuip.GetElementType())))
                {
                    Debug.Log("Already holding that element");
                    return;
                }
            }
            else if(isPrimary )
            {
                if(!eQuip.GetPrimary() || player.GetPrimaryElements().Contains((BaseElementClass)FindObjectOfType(System.Type.GetType(eQuip.GetElementType()))))
                {
                    Debug.Log("Already holding that element");
                    return;
                }
            }
        }
        

        base.OnPointerEnter(pData);
    }

    public override void ClearSlot()
    {
        if (FindObjectOfType(System.Type.GetType(type)))
        {
            ElementEquip eQuip = (ElementEquip)FindObjectOfType(System.Type.GetType(type));

            if (!isPrimary)
            {
                if (eQuip.GetPrimary() || player.GetCatalystElements().Contains((BaseElementClass)FindObjectOfType(System.Type.GetType(eQuip.GetElementType()))))
                {
                    Debug.Log("Already holding that element");
                    return;
                }
            }
            else if (isPrimary)
            {
                if (!eQuip.GetPrimary() || player.GetPrimaryElements().Contains((BaseElementClass)FindObjectOfType(System.Type.GetType(eQuip.GetElementType()))))
                {
                    Debug.Log("Already holding that element");
                    return;
                }
            }
        }

        base.ClearSlot();

        if (isPrimary)
        {
            player.GetPrimaryElements()[elementSlot] = null;
            player.GetComboElements()[elementSlot].comboElements[0] = null;
            player.GetComboElements()[elementSlot].comboElements[1] = null;
        }
        else
        {
            player.GetCatalystElements()[elementSlot] = null;
            player.GetComboElements()[0].comboElements[elementSlot] = null;
            player.GetComboElements()[1].comboElements[elementSlot] = null;
        }
        
    }
}
