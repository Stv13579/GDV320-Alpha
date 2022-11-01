﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : HoverOver, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    GameObject draggedObject, selectedBorder;

    [SerializeField]
    List<ElementSlots> elementSlots;

    [SerializeField]
    TrinketSlot trinketSlot;
    public GameObject GetDragged() { return draggedObject;}

    bool canDrag = true;

    Color startColour;

    [SerializeField]
    Color fadedColour;

    Shooting shootingScript;

    private void Start()
    {
        startColour = GetComponent<Image>().color;
        shootingScript = Shooting.GetShooting();
    }

    private void Update()
    {
        if (elementSlots.Count > 0)
        {
            GameObject tempGameOBJ = draggedObject;
            //switch elements when chosen the first ones
            if(elementSlots[0].GetSlotTaken() == true)
            {
                shootingScript.GetCatalystElements()[shootingScript.GetRightElementIndex()].AnimationSwitch(false);
                shootingScript.TurnOffCatalystOrbs();
                shootingScript.TurnOnCatalystOrbs();
                shootingScript.GetPrimaryElements()[shootingScript.GetLeftElementIndex()].AnimationSwitch(true);
                shootingScript.TurnOffPrimaryOrbs();
                shootingScript.TurnOnPrimaryOrbs();
            }
            //else 
            if (elementSlots[0].GetPlayer().GetCatalystElements().Exists(ele => ele.GetType() == System.Type.GetType(tempGameOBJ.GetComponent<ElementEquip>().GetElementType())))
            {
                GetComponent<Image>().color = fadedColour;
                selectedBorder.SetActive(true);
            }
            else if (elementSlots[1].GetPlayer().GetCatalystElements().Exists(ele => ele.GetType() == System.Type.GetType(tempGameOBJ.GetComponent<ElementEquip>().GetElementType())))
            {
                GetComponent<Image>().color = fadedColour;
                selectedBorder.SetActive(true);
            }
            else if (elementSlots[0].GetPlayer().GetPrimaryElements().Exists(ele => ele.GetType() == System.Type.GetType(tempGameOBJ.GetComponent<ElementEquip>().GetElementType())))
            {
                GetComponent<Image>().color = fadedColour;
                selectedBorder.SetActive(true);
            }
            else if (elementSlots[1].GetPlayer().GetPrimaryElements().Exists(ele => ele.GetType() == System.Type.GetType(tempGameOBJ.GetComponent<ElementEquip>().GetElementType())))
            {
                GetComponent<Image>().color = fadedColour;
                selectedBorder.SetActive(true);
            }
            else
            {
                GetComponent<Image>().color = startColour;
                selectedBorder.SetActive(false);
            }

        }
        if (trinketSlot)
        {
            if (trinketSlot.GetSlotTaken() == true)
            {
                GetComponent<Image>().color = fadedColour;
            }
            else
            {
                GetComponent<Image>().color = startColour;
            }    
        }
    }

    public void SetDraggable(bool draggable) { canDrag = draggable; }

    public void OnPointerDown(PointerEventData eData)
    {   
        if(!canDrag)
        {
            return;
        }
        if (eData.button == PointerEventData.InputButton.Left)
        {
            //Instantiate the dragged object
            GameObject draggedOut = Instantiate(draggedObject, transform);
            draggedOut.GetComponent<DraggedObject>().SetLoadoutVariables(lVars);
        }
    }

    public void OnPointerUp(PointerEventData eData)
    {
        if (eData.button == PointerEventData.InputButton.Right)
        {
            // check if slot is taken and if icon is enabled;
            // for slot 1 (top left)
            if (elementSlots.Count > 0)
            {
                if (!elementSlots[0].GetEquippedIcon().enabled && elementSlots[0].GetSlotTaken() == false)
                {
                    GameObject tempGameOBJ = draggedObject;
                    if (!elementSlots[0].IsPrimary())
                    {
                        if (tempGameOBJ.GetComponent<ElementEquip>().GetPrimary() || elementSlots[0].GetPlayer().GetCatalystElements().Exists(ele => ele.GetType() == System.Type.GetType(tempGameOBJ.GetComponent<ElementEquip>().GetElementType())))
                        {
                            Debug.Log("Already holding that element");
                            return;
                        }
                    }
                    else if (elementSlots[0].IsPrimary())
                    {
                        if (!tempGameOBJ.GetComponent<ElementEquip>().GetPrimary() || elementSlots[0].GetPlayer().GetPrimaryElements().Exists(ele => ele.GetType() == System.Type.GetType(tempGameOBJ.GetComponent<ElementEquip>().GetElementType())))
                        {
                            Debug.Log("Already holding that element");
                            return;
                        }
                    }
                    //Put that object in this slot
                    tempGameOBJ.GetComponent<DraggedObject>().SetLoadoutVariables(lVars);
                    tempGameOBJ.GetComponent<DraggedObject>().Equip(elementSlots[0]);
                    elementSlots[0].SetIcon();
                    elementSlots[0].GetEquippedIcon().enabled = true;
                    elementSlots[0].SetSlotTaken(true);
                }
                // for slot 2 (top right)
                else if (!elementSlots[1].GetEquippedIcon().enabled && elementSlots[1].GetSlotTaken() == false)
                {
                    //Put that object in this slot
                    GameObject tempGameOBJ = draggedObject;
                    if (!elementSlots[1].IsPrimary())
                    {
                        if (tempGameOBJ.GetComponent<ElementEquip>().GetPrimary() || elementSlots[1].GetPlayer().GetCatalystElements().Exists(ele => ele.GetType() == System.Type.GetType(tempGameOBJ.GetComponent<ElementEquip>().GetElementType())))
                        {
                            Debug.Log("Already holding that element");
                            return;
                        }
                    }
                    else if (elementSlots[1].IsPrimary())
                    {
                        if (!tempGameOBJ.GetComponent<ElementEquip>().GetPrimary() || elementSlots[1].GetPlayer().GetPrimaryElements().Exists(ele => ele.GetType() == System.Type.GetType(tempGameOBJ.GetComponent<ElementEquip>().GetElementType())))
                        {
                            Debug.Log("Already holding that element");
                            return;
                        }
                    }
                    tempGameOBJ.GetComponent<DraggedObject>().SetLoadoutVariables(lVars);
                    tempGameOBJ.GetComponent<DraggedObject>().Equip(elementSlots[1]);
                    elementSlots[1].SetIcon();
                    elementSlots[1].GetEquippedIcon().enabled = true;
                    elementSlots[1].SetSlotTaken(true);
                }
            }
            if (trinketSlot)
            {
                if (!trinketSlot.GetEquippedIcon().enabled && trinketSlot.GetSlotTaken() == false && canDrag == true)
                {
                    GameObject tempGameOBJ = draggedObject;
                    tempGameOBJ.GetComponent<DraggedObject>().SetLoadoutVariables(lVars);
                    tempGameOBJ.GetComponent<DraggedObject>().Equip(trinketSlot);
                    trinketSlot.SetIcon();
                    trinketSlot.GetEquippedIcon().enabled = true;
                    trinketSlot.SetSlotTaken(true);


                }
            }
        }
    }
}
