using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UpgradeButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private GameObject detailWindow;

    private BaseElementClass element;

    public void SetElement(BaseElementClass ele) { element = ele; }

    public void OnPointerEnter(PointerEventData peData)
    {
        detailWindow.transform.Find("ElementName").GetComponent<TextMeshProUGUI>().text = element.elementName;
        detailWindow.transform.Find("UpgradeDescription").GetComponent<TextMeshProUGUI>().text = element.upgradeDescription;
        detailWindow.transform.Find("PriceTag").GetChild(0).GetComponent<TextMeshProUGUI>().text = "$" + element.upgradeCost.ToString();

    }
    
}
