using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [System.Serializable]
    public struct LoadoutVariables
    {
        public string Description;
        public string Name;
        public Sprite Icon;
    }

    [SerializeField]
    protected LoadoutVariables lVars;
    public LoadoutVariables GetVars() { return lVars; }

    [SerializeField]
    Image highlight;

    [SerializeField]
    Color hoveredAlpha;
    Color idleAlpha;

    [SerializeField]
    public bool hidden = false;

    private void Awake()
    {
        idleAlpha = highlight.color;
    }
    public void SetLoadoutVariables(DraggedObject.LoadoutVariables newVars) { lVars = newVars; }

    public virtual void OnPointerEnter(PointerEventData pData)
    {
        Debug.Log("Hovering Over");

        //Do description vars and name in details box
        DetailsWindow deetBox = GameObject.Find("DetailsWindow").GetComponent<DetailsWindow>();
        if(hidden)
        {
            deetBox.SetWindow("???", "???");
        }
        else
        {
            deetBox.SetWindow(lVars.Description, lVars.Name);
        }
        if(!highlight)
        {
            return;
        }
        Color hAl = highlight.color;
        hAl.a = hoveredAlpha.a;
        highlight.color = hAl;
    }

    public virtual void OnPointerExit(PointerEventData pData)
    {
        if(!highlight)
        {
            return;
        }

        Color hAl = highlight.color;
        hAl.a = idleAlpha.a;
        highlight.color = hAl;
    }

    public virtual void ClearSlot()
    {
        
        lVars = new DraggedObject.LoadoutVariables();
    }
}
