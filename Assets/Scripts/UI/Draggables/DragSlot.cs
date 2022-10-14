using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragSlot : HoverOver, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    protected string type;

    protected DraggedObject equippingObject;

    protected bool slotTaken;
    public DraggedObject GetEquippingObj() { return equippingObject; }

    public Image GetEquippedIcon() { return equippedIcon; }
    public bool GetSlotTaken() { return slotTaken; }
    public void SetSlotTaken(bool tempSlotTaken) { slotTaken = tempSlotTaken; }
    public string GetStringType() { return type; }

    [SerializeField]
    Image equippedIcon;

    public void SetIcon() { equippedIcon.sprite = lVars.Icon; }

    public override void OnPointerEnter(PointerEventData pData)
    {
        base.OnPointerEnter(pData);
        Debug.Log("Hovering Over");
        if(FindObjectOfType(System.Type.GetType(type)) && !equippedIcon.enabled)
        {
            //Put that object in this slot
            equippingObject = (DraggedObject)FindObjectOfType(System.Type.GetType(type));
            equippingObject.Equip(this);
            SetIcon();
            equippedIcon.enabled = true;
            slotTaken = true;
        }
        //Do description vars and name in details box
    }

    public override void OnPointerExit(PointerEventData pData)
    {
        base.OnPointerExit(pData);
    }

    public void OnPointerDown(PointerEventData pData)
    {
        
    }

    public void OnPointerUp(PointerEventData pData)
    {
        //If the player right clicks the slot, clear it
        if(pData.button == PointerEventData.InputButton.Right)
        {
            ClearSlot();
        }
    }

    public override void ClearSlot()
    {
        base.ClearSlot();
        equippingObject = null;
        equippedIcon.enabled = false;
        slotTaken = false;
    }
    
}
