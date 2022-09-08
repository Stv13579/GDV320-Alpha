using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DetailsWindow : MonoBehaviour
{
    [SerializeField]
    GameObject descriptionBox, nameBox;

    public void SetWindow(string decriptionDetail, string nameDetail)
    {
        descriptionBox.GetComponent<TextMeshProUGUI>().text = decriptionDetail;
        nameBox.GetComponent<TextMeshProUGUI>().text = nameDetail;
    }
}
