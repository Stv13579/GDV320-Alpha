using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinketEquip : DraggedObject
{
    [SerializeField]
    string trinketName;

    private void Start()
    {
        
    }

    public override void Equip(DragSlot slot)
    {
        base.Equip(slot);

        //Add the item to the player items, removing any existing items

        if(FindObjectOfType<PlayerClass>().heldItems.Count > 0)
        {

            FindObjectOfType<PlayerClass>().heldItems.Clear();
            FindObjectOfType<PlayerClass>().heldItems[0].RemoveEffect();

        }

        FindObjectOfType<PlayerClass>().AddItem((Item)FindObjectOfType(System.Type.GetType(trinketName)));
    }
}
