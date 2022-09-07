using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ComboSlot : HoverOver, IPointerEnterHandler
{
    [SerializeField]
    Image comboIcon;

    public override void OnPointerEnter(PointerEventData pData)
    {
        base.OnPointerEnter(pData);


    }

}
