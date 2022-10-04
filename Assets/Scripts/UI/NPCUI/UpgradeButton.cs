using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UpgradeButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    GameObject detailWindow;

    BaseElementClass element;

    public void SetElement(BaseElementClass ele) { element = ele; }

    public void OnPointerEnter(PointerEventData peData)
    {
	    detailWindow.transform.Find("ElementName").GetComponent<TextMeshProUGUI>().text = element.GetName();
	    detailWindow.transform.Find("UpgradeDescription").GetComponent<TextMeshProUGUI>().text = element.GetDescription();
        detailWindow.transform.Find("PriceTag").GetChild(0).GetComponent<TextMeshProUGUI>().text = "$" + element.GetUpgradeCost().ToString();

    }
    
}
