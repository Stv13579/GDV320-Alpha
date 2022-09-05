using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggedObject : MonoBehaviour, IPointerUpHandler
{
    void Update()
    {
        transform.position = Input.mousePosition;

        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            Destroy(gameObject);
        }
    }

    public void OnPointerUp(PointerEventData pData)
    {
        Destroy(gameObject);
    }
}
