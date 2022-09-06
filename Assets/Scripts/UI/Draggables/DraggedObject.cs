using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggedObject : MonoBehaviour
{
    [System.Serializable]
    public struct LoadoutVariables
    {
        public string Description;
        public string Name;
    }

    [SerializeField]
    LoadoutVariables lVars;
    public LoadoutVariables GetVars() { return lVars; }

    [SerializeField]
    Sprite icon;

    public virtual void Equip(DragSlot slot) 
    {
        slot.SetIcon(icon);
        slot.SetLoadoutVariables(lVars);
    }

    void Update()
    {
        transform.position = Input.mousePosition;

        if(Input.GetKeyUp(KeyCode.Mouse0))
        {   
            Destroy(gameObject);
        }
    }
}
