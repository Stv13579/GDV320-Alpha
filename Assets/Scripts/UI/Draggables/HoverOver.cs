using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverOver : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    protected DraggedObject.LoadoutVariables lVars;
    public void SetLoadoutVariables(DraggedObject.LoadoutVariables newVars) { lVars = newVars; }

    public virtual void OnPointerEnter(PointerEventData pData)
    {
        Debug.Log("Hovering Over");


        //Do description vars and name in details box
        DetailsWindow deetBox = GameObject.Find("DetailsWindow").GetComponent<DetailsWindow>();
        deetBox.SetWindow(lVars.Description, lVars.Name);
        
    }

    public virtual void ClearSlot()
    {
        
        lVars = new DraggedObject.LoadoutVariables();
    }
}
