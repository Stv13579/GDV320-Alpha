using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : HoverOver, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    GameObject draggedObject;

    bool canDrag = true;

    public void SetDraggable(bool draggable) { canDrag = draggable; }

    public void OnPointerDown(PointerEventData eData)
    {   
        if(!canDrag)
        {
            return;
        }
        //Instantiate the dragged object
        GameObject draggedOut = Instantiate(draggedObject, transform);
        draggedOut.GetComponent<DraggedObject>().SetLoadoutVariables(lVars);
    }

    public void OnPointerUp(PointerEventData eData)
    {
        
    }
}
