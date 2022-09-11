using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTestingUI : MonoBehaviour
{
    bool doneLocking = false;
    bool doneUnlocking = false;

    private void Update()
    {
        if(Cursor.lockState == CursorLockMode.Locked && !doneLocking)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            doneLocking = true;
            doneUnlocking = false;
        }
        else if (Cursor.lockState == CursorLockMode.None && !doneUnlocking)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }       
            doneLocking = false;
            doneUnlocking = true;
        }

        
    }
}
