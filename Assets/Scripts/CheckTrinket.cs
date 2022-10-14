using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckTrinket : MonoBehaviour
{
    Trinket tToCheck;


    // Start is called before the first frame update
    void Start()
    {
        tToCheck = (Trinket)FindObjectOfType(System.Type.GetType(GetComponent<Draggable>().GetDragged().GetComponent<TrinketEquip>().trinketName));
        if (tToCheck.uState == Trinket.UnlockState.Locked)
        {
            //Set the sprite to q mark and not draggable
            GetComponent<Draggable>().SetDraggable(false);
            GetComponent<Draggable>().hidden = true;
            GetComponent<Image>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tToCheck.uState == Trinket.UnlockState.Locked)
        {
            //Set the sprite to q mark and not draggable
            GetComponent<Draggable>().SetDraggable(false);
            GetComponent<Draggable>().hidden = true;

            GetComponent<Image>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(true);
        }
        if (tToCheck.uState != Trinket.UnlockState.Locked)
        {
            //Set the sprite to q mark and not draggable
            GetComponent<Draggable>().SetDraggable(true);
            GetComponent<Draggable>().hidden = false;
            GetComponent<Image>().enabled = true;
            transform.GetChild(0).gameObject.SetActive(false);
            Destroy(this);
        }

    }
}
