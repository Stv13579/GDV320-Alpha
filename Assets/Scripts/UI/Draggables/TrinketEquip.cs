using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinketEquip : DraggedObject
{
    [SerializeField]
    public string trinketName;

    private void Start()
    {
        
    }

    public override void Equip(DragSlot slot)
    {
        base.Equip(slot);

        //Add the item to the player items, removing any existing items

	    if(PlayerClass.GetPlayerClass().GetHeldItems().Count > 0)
        {

            PlayerClass.GetPlayerClass().GetHeldItems()[0].RemoveEffect();
            PlayerClass.GetPlayerClass().GetHeldItems().Clear();
            

        }

        PlayerClass.GetPlayerClass().AddItem((Item)FindObjectOfType(System.Type.GetType(trinketName)));
    }
}
