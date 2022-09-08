using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : HoverOver, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    GameObject draggedObject;

    public void OnPointerDown(PointerEventData eData)
    {
        //Instantiate the dragged object
        Instantiate(draggedObject, transform);

    }

    public void OnPointerUp(PointerEventData eData)
    {
        
    }
}
