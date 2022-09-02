using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Prophecy : MonoBehaviour
{
    [SerializeField]
    string pName, effect;
    
    [SerializeField]
    Sprite image;

    bool active = false;

    [SerializeField]
    protected DropsList drops;

    private void Start()
    {
        
    }

    public void SetCard(Transform button)
    {
        button.GetChild(0).GetComponent<TextMeshProUGUI>().text = pName;
        button.GetChild(1).GetComponent<TextMeshProUGUI>().text = effect;
        button.GetChild(2).GetComponent<Image>().sprite = image;
    }

    public void SetActive()
    {
        active = true;
    }

    public bool GetActive()
    {
        return active;
    }

    public virtual void InitialEffect()
    {
        active = true;


    }

    public virtual void TriggerAction()
    {

    }




}
