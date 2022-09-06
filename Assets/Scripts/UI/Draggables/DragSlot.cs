using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DragSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    protected string type;

    protected DraggedObject equippingObject;

    DraggedObject.LoadoutVariables lVars;

    public void SetLoadoutVariables(DraggedObject.LoadoutVariables newVars) { lVars = newVars; }

    [SerializeField]
    Image equippedIcon;

    public void SetIcon(Sprite equipping) { equippedIcon.sprite = equipping; }

    public virtual void OnPointerEnter(PointerEventData pData)
    {
        Debug.Log("Hovering Over");
        if(FindObjectOfType(System.Type.GetType(type)))
        {
            //Put that object in this slot
            equippingObject = (DraggedObject)FindObjectOfType(System.Type.GetType(type));
            equippingObject.Equip(this);
            equippedIcon.enabled = true;
        }

        //Do description vars and name in details box
    }

    public void OnPointerExit(PointerEventData pData)
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            ClearSlot();
        }   
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

    public virtual void ClearSlot()
    {
        equippingObject = null;
        equippedIcon.enabled = false;
        lVars = new DraggedObject.LoadoutVariables();
    }
    
}
