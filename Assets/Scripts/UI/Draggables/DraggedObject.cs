using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggedObject : HoverOver
{
    

    public virtual void Equip(DragSlot slot) 
    {
        slot.SetLoadoutVariables(lVars);
        
    }

    void Update()
    {
        transform.position = Input.mousePosition;

        GetComponent<Image>().sprite = lVars.Icon;

        if(Input.GetKeyUp(KeyCode.Mouse0))
        {   
            Destroy(gameObject);
        }
    }
}
